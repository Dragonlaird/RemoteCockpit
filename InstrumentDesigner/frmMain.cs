using Newtonsoft.Json;
using RemoteCockpitClasses;
using RemoteCockpitClasses.Animations;
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

            // Populate Background
            txtBackgroundPath.Text = config.BackgroundImagePath;
            pbBackgroundImage.BackgroundImage = null;
            if (!string.IsNullOrEmpty(config?.BackgroundImagePath))
            {
                var image = LoadImage(Path.Combine(cockpitDirectory, config.BackgroundImagePath));
                if (image != null)
                {
                    ShowImage(image, pbBackgroundImage);
                }
            }
            foreach (var aircraft in config.Aircraft)
            {
                var idx = dgAircraft.Rows.Add();
                dgAircraft.Rows[idx].Cells["Aircraft"].Value = aircraft;
            }
            foreach (var anim in config.Animations)
            {
                var rowIdx = dgAnimations.Rows.Add();
                dgAnimations.Rows[rowIdx].Cells["What"].Value = anim.Name?.ToString();
                dgAnimations.Rows[rowIdx].Cells["When"].Value = string.Join(",", anim.Triggers.Select(x => x.Type.ToString()));
                dgAnimations.Rows[rowIdx].Cells["How"].Value = string.Join(",", anim.Triggers.SelectMany(x => x.Actions.Select(y => y.Type.ToString())));
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
                MessageBox.Show(string.Format("Unable to load Image:\r\r{0}", imagePath), "Load File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return image;
        }

        private void LoadConfig(object sender, EventArgs e)
        {
            openFileDialog.Title = "Load Instrument Configuration";
            openFileDialog.Filter = "Instrument Configurations|*.json";
            openFileDialog.FileName = "";

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
                    MessageBox.Show(string.Format("Error opening Configuration file:\r\r{0}", fileName), "File Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        var animation = config.Animations?.FirstOrDefault(x => x.Name == name) ?? new AnimationDrawing();
                        using (frm = new frmAnimation(animation, cockpitDirectory))
                        {
                            result = frm.ShowDialog(this);
                            if (result == DialogResult.OK)
                            {
                                var newAnimation = ((frmAnimation)frm).DialogValue;
                                if(config.Animations.Any(x=>x.Name == animation.Name))
                                {
                                    var cleanAnimations = config.Animations.Where(x => x.Name != animation.Name).ToList();
                                    config.Animations = cleanAnimations.ToArray();
                                }
                                var currentAnimations = config.Animations.ToList();
                                currentAnimations.Add(newAnimation);
                                config.Animations = currentAnimations.ToArray();
                            }
                        }
                        break;
                    case "X":
                        result = MessageBox.Show("Are you sure you want to delete this animation?", "Delete Animation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if(result == DialogResult.OK)
                        {
                            var newAnimations = config.Animations.ToList();
                            newAnimations.RemoveAt(newAnimations.IndexOf(newAnimations.First(x => x.Name == name)));
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
            var rowIdx = dgAnimations.Rows.Add();
            dgAnimations.Rows[rowIdx].Cells["What"].Value = "New...";
            var colIdx = dgAnimations.Columns.IndexOf(dgAnimations.Columns["Edit"]);
            EditDeleteAnimation_Click(dgAnimations, new DataGridViewCellEventArgs(colIdx, rowIdx));
        }

        private void SaveConfig(object sender, EventArgs e)
        {
            var configJson = JsonConvert.SerializeObject(config);
            if(sender == saveAsToolStripMenuItem)
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(configFilePath);
                openFileDialog.Title = "Save Instrument Configuraion";
                openFileDialog.Filter = "Instrument Configurations|*.json";
                openFileDialog.FileName = "";

                var result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                    configFilePath = openFileDialog.FileName;
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
            MessageBox.Show("Configuration Saved.\r\rTo use this configuration, use the Publish menu.", "Config Saved");
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
    }
}
