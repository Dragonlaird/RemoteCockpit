using Newtonsoft.Json;
using RemoteCockpitClasses;
using RemoteCockpitClasses.Animations;
using RemoteCockpitClasses.Animations.Actions;
using RemoteCockpitClasses.Animations.Items;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstrumentDesigner
{
    public partial class frmInstrumentDesigner : Form
    {
        private Configuration config;
        private string configFilePath;
        private string cockpitDirectory;
        private bool populatingForm = false;
        public frmInstrumentDesigner()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {

            configFilePath = "";
            openFileDialog.FileName = "";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            ResetForm();
            config = new Configuration();
        }

        private void ResetForm()
        {
            txtInstrumentName.Text = "";
            txtAuthor.Text = "";
            cmbInstrumentType.Items.Clear();
            foreach (var itemType in ((InstrumentType[])Enum.GetValues(typeof(InstrumentType))).OrderBy(x => x.ToString()))
            {
                cmbInstrumentType.Items.Add(itemType.ToString());
            }
            cmbInstrumentType.SelectedIndex = 0;
            txtUpdateMS.Text = "";
            txtCreateDate.Text = "";
            dgAircraft.Rows.Clear();
            txtBackgroundPath.Text = "";
            pbBackgroundImage.Image?.Dispose();
            pbBackgroundImage.Image = new Bitmap(1,1);
            pbBackgroundImage.BackgroundImage?.Dispose();
            pbBackgroundImage.BackgroundImage = new Bitmap(1,1);
            pbBackgroundImage.BackColor = Color.Transparent;
            dgAnimations.Rows.Clear();
        }

        private void PopulateConfigForm()
        {
            // Populate Instrument Details Group
            populatingForm = true;
            if (config == null)
            {
                config = new Configuration();
            }
            ResetForm();
            txtInstrumentName.Text = config.Name;
            txtAuthor.Text = config.Author;
            if (!string.IsNullOrEmpty(config.Author))
                txtAuthor.Enabled = false;
            cmbInstrumentType.SelectedIndex = cmbInstrumentType.Items.IndexOf(config.Type.ToString());
            txtUpdateMS.Text = config.AnimationUpdateInMs.ToString();
            txtCreateDate.Text = string.Format("{0:dd MMMM yyyy HH:mm}", config.CreateDate);
            var currentActivity = string.Empty;
            try { 
                // Populate Background
                currentActivity = "Setting Instrument Batchground Path";
                txtBackgroundPath.Text = config.BackgroundImagePath;
                currentActivity = "Clearing Instrument Background Image";
                pbBackgroundImage.BackgroundImage = null;
                if (!string.IsNullOrEmpty(config?.BackgroundImagePath))
                {
                    currentActivity = "Loading Instrument Background Image";
                    var image = LoadImage(Path.Combine(cockpitDirectory, config.BackgroundImagePath));
                    if (image != null)
                    {
                        currentActivity = "Displaying Instrument Background Image";
                        ShowImage(image, pbBackgroundImage);
                    }
                }
                foreach (var aircraft in config.Aircraft)
                {
                    currentActivity = string.Format("Setting Instrument Aircraft: {0}", aircraft);
                    var idx = dgAircraft.Rows.Add();
                    dgAircraft.Rows[idx].Cells["Aircraft"].Value = aircraft;
                }
                currentActivity = "Loading Instrument Animations";
                foreach (var anim in config.Animations)
                {
                    var animation  = (IAnimationItem)anim;
                    currentActivity = string.Format("Loading Instrument Animation: {0}", animation?.Name);
                    var rowIdx = dgAnimations.Rows.Add();
                    dgAnimations.Rows[rowIdx].Cells["What"].Value = animation.Name?.ToString();
                    if (animation.Triggers != null)
                    {
                        currentActivity = string.Format("Loading Instrument Animation '{0}' Triggers ({1} triggers)", animation?.Name, animation.Triggers?.Count());
                        dgAnimations.Rows[rowIdx].Cells["When"].Value = string.Join(",", animation.Triggers?.Select(x => ((IAnimationTrigger)x).Type.ToString()));
                        dgAnimations.Rows[rowIdx].Cells["How"].Value = string.Join(",", animation.Triggers?.SelectMany(x => ((IAnimationTrigger)x).Actions?.Where(z => z != null && ((IAnimationAction)z).Type != null).Select(y => ((IAnimationAction)y)?.Type.ToString())));
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Failed to load Config.\r\nStep {0}", currentActivity), "Config Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            populatingForm = false;
        }

        private void ShowImage(Image image, Control imageBox)
        {
            imageBox.BackgroundImage?.Dispose();
            imageBox.BackgroundImage = new Bitmap(1, 1);
            if (image != null)
            {
                var imageScaleFactor = (double)imageBox.Width / image.Width;
                var aspectRatio = (double)image.Height / image.Width;
                if (image.Height * imageScaleFactor > imageBox.Height)
                    imageScaleFactor = (double)imageBox.Height / image.Height;
                try
                {
                    using (var backgroundImage = new Bitmap(imageBox.Width, imageBox.Height))
                    {
                        using (Graphics gr = Graphics.FromImage(backgroundImage))
                        {
                            gr.DrawImage(new Bitmap(image, new Size((int)(image.Width * imageScaleFactor), (int)(image.Height * imageScaleFactor))), new Point(0, 0));
                        }
                        imageBox.BackColor = Color.AliceBlue; // Use this to show where image doesn't fit the control
                        imageBox.BackgroundImage = (Image)backgroundImage.Clone();
                    }
                }
                catch { }
            }
        }

        private Image LoadImage(string imagePath)
        {
            Image image = null;
            try
            {
                using (var fileStream = File.OpenRead(imagePath))
                {
                    image = Image.FromStream(fileStream);
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Unable to load Image:\r\r{0}\rError: {1}", imagePath, ex.Message), "Load File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return image;
        }

        private void LoadConfig(object sender, EventArgs e)
        {
            openFileDialog.Title = "Load Instrument Configuration";
            openFileDialog.Filter = "Instrument Configurations|*.json";
            openFileDialog.FileName = "";
            openFileDialog.InitialDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".\\GenericInstruments"));
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                try
                {
                    var configJson = File.ReadAllText(fileName);
                    config = JsonConvert.DeserializeObject<Configuration>(configJson);
                    config.HasChanged = false;
                    if (config == null || config.Name == null)
                    {
                        throw new Exception("Configuration Load Failed");
                    }
                    configFilePath = fileName;
                    cockpitDirectory = Path.Combine(Path.GetDirectoryName(configFilePath), "..");
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(configFilePath);
                    PopulateConfigForm();
                }
                catch (IOException ex)
                {
                    config = new Configuration();
                    MessageBox.Show(string.Format("Error opening Configuration file:\r\r{0}\r\rError: {1}", fileName, ex.Message), "File Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    config = new Configuration();
                    MessageBox.Show(string.Format("Error reading Configuration file:\r\r{0}\r\rError: {1}", fileName, ex.Message), "Invalid File Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void CloseForm(object sender, EventArgs e)
        {
            this.FindForm().Close();
            this.FindForm().Dispose();
        }

        private void cmdClearBackground_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(config.BackgroundImagePath) || MessageBox.Show("Clear the current background image.\r\rAre you sure?", "Clear Background Image", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                pbBackgroundImage.Image?.Dispose();
                pbBackgroundImage.Image = new Bitmap(1, 1);
                pbBackgroundImage.BackgroundImage?.Dispose();
                pbBackgroundImage.BackgroundImage = new Bitmap(1, 1);
                txtBackgroundPath.Text = "";
                if (!populatingForm)
                    config.BackgroundImagePath = "";
            }
        }

        private void cmdLoadBackground_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(config.BackgroundImagePath) || MessageBox.Show("This action will clear the existing background image and import a new image into the Configuration Instrument Images folder.\r\rAre you sure?", "Clear and Load Background Image", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                openFileDialog.Title = "Load Background Image";
                openFileDialog.Filter = "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff";
                openFileDialog.FileName = "";
                openFileDialog.InitialDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".\\InstrumentImages"));

                var dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    var backgroundAbsolutePath = openFileDialog.FileName;
                     var backgroundImage = LoadImage(backgroundAbsolutePath);
                    if (backgroundImage != null)
                    {
                        ShowImage(backgroundImage, pbBackgroundImage);
                        var relativePath = Path.Combine(".\\InstrumentImages", Path.GetFileName(backgroundAbsolutePath));
                        if (Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relativePath)) != Path.GetFullPath(backgroundAbsolutePath))
                            File.Copy(backgroundAbsolutePath, Path.Combine(Directory.GetCurrentDirectory(), relativePath));
                        txtBackgroundPath.Text = relativePath;
                    }
                }
            }
        }
        private void NewInstrument_Click(object sender, EventArgs e)
        {
            if (config == null || !config.HasChanged || MessageBox.Show("Are you sure you want to discard the current Instrument Configuraion and define a new Instrument?", "Discard existing Configuration?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                config = new Configuration();
                PopulateConfigForm();
            }
        }

        private void InstrumentName_Changed(object sender, EventArgs e)
        {
            if (!populatingForm)
                config.Name = txtInstrumentName.Text;
        }

        private void InstrumentAuthor_Changed(object sender, EventArgs e)
        {
            if (!populatingForm)
                config.Author = txtAuthor.Text;
        }

        private void InstrumentType_Changed(object sender, EventArgs e)
        {
            if (cmbInstrumentType.Items?.Count > 0)
            {
                var selectedType = cmbInstrumentType.Items[cmbInstrumentType.SelectedIndex].ToString();
                if (config != null && !string.IsNullOrEmpty(selectedType) && !populatingForm)
                        config.Type = (InstrumentType)Enum.Parse(typeof(InstrumentType), selectedType);
            }
        }

        private void UpdateMS_Changed(object sender, EventArgs e)
        {
            if (!populatingForm)
                config.AnimationUpdateInMs = int.Parse(txtUpdateMS.Text);
        }

        private void DeleteGridRow_Click(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var aircraft = senderGrid.Rows[e.RowIndex].Cells["Aircraft"].Value?.ToString();
                //senderGrid.Rows.RemoveAt(e.RowIndex);
                if (!string.IsNullOrEmpty(aircraft))
                {
                    var allowedAircraft = config.Aircraft.ToList();
                    allowedAircraft.Remove(aircraft);
                    if (!populatingForm)
                        config.Aircraft = allowedAircraft.OrderBy(x => x).ToArray();
                    dgAircraft.Rows.Clear();
                    foreach (var allowed in config.Aircraft)
                    {
                        var rowIdx = dgAircraft.Rows.Add();
                        dgAircraft.Rows[rowIdx].Cells["Aircraft"].Value = allowed;
                    }
                }
            }
        }

        private void EditGridRow_Change(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewTextBoxColumn &&
                e.RowIndex >= 0)
            {
                var allowedAircraft = new List<string>();
                foreach (DataGridViewRow row in senderGrid.Rows)
                {
                    var val = row.Cells[e.ColumnIndex].Value?.ToString();
                    if (!string.IsNullOrEmpty(val))
                        allowedAircraft.Add(val);
                }
                if (!populatingForm)
                    config.Aircraft = allowedAircraft.ToArray();
            }
        }

        private void FormSize_Changed(object sender, EventArgs e)
        {
            gpAnimations.Left = gpBackground.Right + 5;
        }

        private void EditDeleteAnimation_Click(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex >= 0 && senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var action = senderGrid.Columns[e.ColumnIndex].HeaderText?.ToString();
                var name = senderGrid.Rows[e.RowIndex].Cells["What"].Value?.ToString();
                Form frm;
                DialogResult result = DialogResult.Abort;
                switch (action)
                {
                    case "E":
                        if (config.Animations == null || !config.Animations.Any(x => ((IAnimationItem)x).Name == name))
                        {
                            var newAnimation = new AnimationDrawing
                            {
                                Name = name,
                                Type = AnimationItemTypeEnum.Drawing,
                                Triggers = new IAnimationTrigger[0]
                            };
                            newAnimation.Triggers.ToList().Add(
                                            (IAnimationTrigger)new AnimationTriggerClientRequest
                                            {
                                                Type = AnimationTriggerTypeEnum.ClientRequest,
                                                Actions = new IAnimationAction[0]
                                            });
                            config.Animations = new IAnimationItem[0]; ;
                            config.Animations.ToList().Add(newAnimation);
                        }
                        var animation = ObjectClone.Clone<IAnimationItem>((IAnimationItem)config.Animations.First(x => ((IAnimationItem)x).Name == name));
                            //ObjectClone.Clone(config.Animations?.First(x => x.Name == name));
                        using (frm = new frmAnimation((IAnimationItem)animation, cockpitDirectory))
                        {
                            result = frm.ShowDialog(this);
                            if (result == DialogResult.OK)
                            {
                                var newAnimation = ((frmAnimation)frm).DialogValue;
                                // Replace existing animation with the modified version
                                var currentAnimations = config.Animations.ToList();
                                currentAnimations[currentAnimations.IndexOf(currentAnimations.First(x => ((IAnimationItem)x).Name == name))] = newAnimation;
                                config.Animations = currentAnimations.ToArray();
                            }
                        }
                        break;
                    case "X":
                        result = MessageBox.Show("Are you sure you want to delete this animation?", "Delete Animation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if(result == DialogResult.OK)
                        {
                            var newAnimations = config.Animations.ToList();
                            newAnimations.RemoveAt(newAnimations.IndexOf(newAnimations.First(x => ((IAnimationItem)x).Name == name)));
                            config.Animations = newAnimations.ToArray();
                        }
                        break;
                }
                if(result == DialogResult.OK)
                {
                    PopulateConfigForm();
                }
            }
        }

        private void NewAnimation_Click(object sender, EventArgs e)
        {
            var rowIdx = dgAnimations.Rows.Add(new object[] {"New..." });
            var colIdx = dgAnimations.Columns.IndexOf(dgAnimations.Columns["Edit"]);
            var animations = config.Animations.ToList();
            animations.Add(new AnimationDrawing { 
                Name = "New...", 
                Type = AnimationItemTypeEnum.Drawing,
                Triggers = new IAnimationTrigger[0],
                OffsetX = 0,
                OffsetY = 0,
                FillColor = Color.White,
                FillMethod = FillTypeEnum.Solid,
                PointMap = new AnimationPoint[0] 
            });
            config.Animations = animations.ToArray();
            EditDeleteAnimation_Click(dgAnimations, new DataGridViewCellEventArgs(colIdx, rowIdx));
        }

        private void SaveConfig(object sender, EventArgs e)
        {
            var configJson = JsonConvert.SerializeObject(config);
            if(sender == saveAsToolStripMenuItem || string.IsNullOrEmpty(configFilePath))
            {
                
                saveFileDialog.InitialDirectory = string.IsNullOrEmpty(configFilePath) ? Directory.GetCurrentDirectory() : Path.GetDirectoryName(configFilePath);
                saveFileDialog.Title = "Save Instrument Configuraion";
                saveFileDialog.Filter = "Instrument Configurations|*.json";
                saveFileDialog.FileName = "";

                var result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                    configFilePath = saveFileDialog.FileName;
                else
                    return;
                if (File.Exists(configFilePath) && MessageBox.Show("File already exists.\rContinuing will overwrite this file.\r\rAre you sure?", "Replace File?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    return;
            }
            if (File.Exists(configFilePath))
                File.Delete(configFilePath);
            var file = File.OpenWrite(configFilePath);
            file.Write(Encoding.ASCII.GetBytes(configJson), 0, configJson.Length);
            file.Close();
            config.HasChanged = false;
            MessageBox.Show("Configuration Saved.", "Config Saved");
        }

        private void Close_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(config?.Name) && config.HasChanged)
            {
                if(MessageBox.Show("This will lose all Configuration changes.\r\rAre you sure?", "Config has changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    return;
                }
            }
            ResetForm();
            config = new Configuration();
            configFilePath = "";
        }

        private void testInstrumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInstrumentTest frmTest = new frmInstrumentTest(config);
            frmTest.ShowDialog(this);
        }
    }
}
