﻿using Newtonsoft.Json;
using RemoteCockpitClasses;
using RemoteCockpitClasses.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
            config = new Configuration();
            configFilePath = "";
            openFileDialog.FileName = "";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            cmbInstrumentType.Items.Clear();
            foreach (var itemType in ((InstrumentType[])Enum.GetValues(typeof(InstrumentType))).OrderBy(x => x.ToString()))
            {
                cmbInstrumentType.Items.Add(itemType.ToString());
            }
            cmbInstrumentType.SelectedIndex = 0;

        }

        private void PopulateConfigForm()
        {
            // Populate Instrument Details Group
            if (config == null)
            {
                config = new Configuration();
            }
            txtInstrumentName.Text = config.Name;
            txtAuthor.Text = config.Author;
            if (!string.IsNullOrEmpty(config.Author))
                txtAuthor.Enabled = false;
            cmbInstrumentType.SelectedIndex = cmbInstrumentType.Items.IndexOf(config.Type.ToString());
            txtUpdateMS.Text = config.AnimationUpdateInMs.ToString();
            txtCreateDate.Text = string.Format("{0:dd MMMM yyyy HH:mm}", config.CreateDate);

            // Populate Background
            txtBackgroundPath.Text = config.BackgroundImagePath;
            var image = LoadImage(Path.Combine(cockpitDirectory, config.BackgroundImagePath));
            if (image != null)
            {
                ShowImage(image, pbBackgroundImage);
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

                    
                }
            }
        }
    }
}
