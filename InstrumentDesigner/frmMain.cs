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
            pbBackgroundImage.Image = null;
            pbBackgroundImage.BackgroundImage = null;
            pbBackgroundImage.BackColor = Color.Transparent;
            dgAnimations.Rows.Clear();
        }

        private void PopulateConfigForm()
        {
            // Populate Instrument Details Group
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
            foreach(var anim in config.Animations)
            {
                var rowIdx = dgAnimations.Rows.Add();
                dgAnimations.Rows[rowIdx].Cells["What"].Value = anim.Name?.ToString();
                dgAnimations.Rows[rowIdx].Cells["When"].Value = string.Join(",", anim.Triggers.Select(x => x.Type.ToString()));
                dgAnimations.Rows[rowIdx].Cells["How"].Value = string.Join(",", anim.Triggers.SelectMany(x => x.Actions.Select(y => y.Type.ToString())));
            }
        }

        private void ShowImage(Image image, Control imageBox)
        {
            var imageScaleFactor = (double)imageBox.Width / image.Width;
            var aspectRatio = (double)image.Height / image.Width;
            if (image.Height * imageScaleFactor > imageBox.Height)
                imageScaleFactor = (double)imageBox.Height / image.Height;
            var backgroundImage = new Bitmap(imageBox.Width, imageBox.Height);
            using (Graphics gr = Graphics.FromImage(backgroundImage))
            {
                gr.DrawImage(new Bitmap(image, new Size((int)(image.Width * imageScaleFactor), (int)(image.Height * imageScaleFactor))), new Point(0, 0));
            }
            imageBox.BackColor = Color.AliceBlue; // Use this to show where image doesn't fit the control
            imageBox.BackgroundImage = backgroundImage;
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
            openFileDialog.Title = "Load Instrument Configuraion";
            openFileDialog.Filter = "Instrument Configurations|*.json";

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
                    MessageBox.Show(string.Format("Error reading Configuration file:\r\r{0}", fileName), "Invalid File Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void focusInstrumentName(object sender, EventArgs e)
        {
            txtInstrumentName.Focus();
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
                txtBackgroundPath.Text = "";
                config.BackgroundImagePath = "";
            }
        }

        private void cmdLoadBackground_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(config.BackgroundImagePath) || MessageBox.Show("This action will clear the existing background image and import a new image into the Configuration Instrument Images folder.\r\rAre you sure?", "Clear and Load Background Image", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                openFileDialog.Title = "Load Background Image";
                openFileDialog.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
                var dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    var backgroundAbsolutePath = openFileDialog.FileName;
                    txtBackgroundPath.Text = backgroundAbsolutePath;
                    LoadImage(backgroundAbsolutePath);
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
            config.Name = txtInstrumentName.Text;
        }

        private void InstrumentAuthor_Changed(object sender, EventArgs e)
        {
            config.Author = txtAuthor.Text;
        }

        private void InstrumentType_Changed(object sender, EventArgs e)
        {
            if (cmbInstrumentType.Items?.Count > 0)
            {
                var selectedType = cmbInstrumentType.Items[cmbInstrumentType.SelectedIndex].ToString();
                if (config != null && !string.IsNullOrEmpty(selectedType))
                    config.Type = (InstrumentType)Enum.Parse(typeof(InstrumentType), selectedType);
            }
        }

        private void UpdateMS_Changed(object sender, EventArgs e)
        {
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
                    config.Aircraft = allowedAircraft.OrderBy(x => x).ToArray();
                    dgAircraft.Rows.Clear();
                    foreach(var allowed in config.Aircraft)
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
                config.Aircraft = allowedAircraft.ToArray();
            }
        }
    }
}
