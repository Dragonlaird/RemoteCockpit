﻿using RemoteCockpitClasses.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace InstrumentDesigner
{
    public partial class frmAnimation : Form
    {
        private IAnimationItem _animation;
        public IAnimationItem DialogValue { get { return _animation; } }
        private readonly string baseFolder;
        private bool populatingForm = false;
        public frmAnimation(IAnimationItem animation, string cockpitBaseFolder)
        {
            InitializeComponent();
            _animation = animation;
            baseFolder = cockpitBaseFolder;
            Initialize();
        }

        private void Initialize()
        {
            populatingForm = true;
            ResetForm();
            for (var tabId = 0; tabId < tabCollection.TabCount; tabId++)
            {
                ClearTab(tabId);
                PopulateTab(tabId);
                populatingForm = true;
            }
            populatingForm = false;
        }

        private void ResetForm()
        {
            cmbAnimationType.Items.Clear();
            foreach (var itemType in ((AnimationItemTypeEnum[])Enum.GetValues(typeof(AnimationItemTypeEnum))).OrderBy(x => x.ToString()))
            {
                cmbAnimationType.Items.Add(itemType.ToString());
            }
            cmbAnimationScaleMethod.Items.Clear();
            foreach (var scaleType in ((AnimationScaleMethodEnum[])Enum.GetValues(typeof(AnimationScaleMethodEnum))).OrderBy(x => x.ToString()))
            {
                cmbAnimationScaleMethod.Items.Add(scaleType.ToString());
            }
            cmbAnimationFillColor.Items.Clear();
            foreach (var color in ((KnownColor[])Enum.GetValues(typeof(KnownColor))).OrderBy(x => x.ToString()))
            {
                cmbAnimationFillColor.Items.Add(color.ToString());
            }
            cmbAnimationFillMethod.Items.Clear();
            foreach (var fillMethod in ((FillType[])Enum.GetValues(typeof(FillType))).OrderBy(x => x.ToString()))
            {
                cmbAnimationFillMethod.Items.Add(fillMethod.ToString());
            }
        }

        private void ClearTab(int tabId)
        {
            populatingForm = true;
            foreach (var ctrl in tabCollection.TabPages[tabId].Controls)
            {
                if (ctrl.GetType() == typeof(TextBox))
                {
                    ((TextBox)ctrl).Text = "";
                }
                if (ctrl.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)ctrl).SelectedIndex = 0;
                }
            }
            populatingForm = false;
        }

        private void PopulateTab(int tabId)
        {
            populatingForm = true;
            switch (tabId)
            {
                case 0:
                    gpAnimationImage.Visible = false;
                    gpAnimationDrawing.Visible = false;
                    txtAnimationName.Text = _animation.Name;
                    cmbAnimationType.SelectedIndex = cmbAnimationType.Items.IndexOf(_animation.Type.ToString());
                    if (_animation.Type == AnimationItemTypeEnum.Image)
                    {
                        gpAnimationImage.Visible = true;
                        txtAnimationImagePath.Text = ((AnimationImage)_animation).ImagePath ?? "";
                        cmbAnimationScaleMethod.SelectedIndex = cmbAnimationScaleMethod.Items.IndexOf(((AnimationImage)_animation).ScaleMethod.ToString());
                        //txtAnimationRelativeX.Text = _animation.RelativeX.ToString();
                        //txtAnimationRelativeY.Text = _animation.RelativeY.ToString();
                        try
                        {
                            var image = Image.FromFile(Path.Combine(baseFolder,txtAnimationImagePath.Text));
                            if (image != null)
                                ShowImage(image, pbAnimationImage);
                        }
                        catch { }
                    }
                    if(_animation.Type == AnimationItemTypeEnum.Drawing)
                    {
                        gpAnimationDrawing.Visible = true;
                        if(((AnimationDrawing)_animation).PointMap == null)
                        {
                            ((AnimationDrawing)_animation).PointMap = new AnimationPoint[0];
                        }
                        cmbAnimationFillColor.SelectedIndex = cmbAnimationFillColor.Items.IndexOf(((AnimationDrawing)_animation).FillColor.ToKnownColor().ToString());
                        cmbAnimationFillMethod.SelectedIndex = cmbAnimationFillMethod.Items.IndexOf(((AnimationDrawing)_animation).FillMethod.ToString());
                        dgAnimationPlotPoints.Rows.Clear();
                        foreach(var plotPoint in ((AnimationDrawing)_animation).PointMap)
                        {
                            dgAnimationPlotPoints.Rows.Add(new object[] { plotPoint.X, plotPoint.Y });
                        }
                    }
                    break;
                case 1:
                    break;
            }
            populatingForm = false;
        }

        private void ShowImage(Image image, Control imageBox)
        {
            var imageScaleFactor = (double)imageBox.Width / image.Width;
            var aspectRatio = (double)image.Height / image.Width;
            if (image.Height * imageScaleFactor > imageBox.Height)
                imageScaleFactor = (double)imageBox.Height / image.Height;
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

        private void ChangeAnimationType_Select(object sender, EventArgs e)
        {
            var selectedType = cmbAnimationType.SelectedItem?.ToString();
            gpAnimationDrawing.Visible = false;
            gpAnimationImage.Visible = false;
            if (!populatingForm)
            {
                IAnimationItem newAnimation = null;
                if (selectedType == "Drawing")
                {
                    newAnimation = new AnimationDrawing
                    {
                        Name = _animation.Name,
                        Type = AnimationItemTypeEnum.Drawing,
                        ScaleMethod = _animation.ScaleMethod,
                        FillColor = Color.White,
                        FillMethod = System.Windows.Forms.VisualStyles.FillType.Solid,
                        RelativeX = 50,
                        RelativeY = 50,
                        Triggers = _animation.Triggers
                    };
                }
                if (selectedType == "Image")
                {
                    newAnimation = new AnimationImage
                    {
                        Name = _animation.Name,
                        Type = AnimationItemTypeEnum.Image,
                        ScaleMethod = _animation.ScaleMethod,
                        ImagePath = "",
                        Triggers = _animation.Triggers
                    };
                    pbAnimationImage.BackgroundImage?.Dispose();
                    pbAnimationImage.BackgroundImage = new Bitmap(1, 1);
                    pbAnimationImage.Image?.Dispose();
                    pbAnimationImage.Image = new Bitmap(1, 1);
                }
                _animation = newAnimation;
                PopulateTab(0);
            }
            //if (_animation.Type == AnimationItemTypeEnum.Image)
            //{
            //    gpAnimationImage.Visible = true;
            //}
            //if(_animation.Type == AnimationItemTypeEnum.Drawing)
            //{
            //    gpAnimationDrawing.Visible = true;
            //}
        }

        private void UpdateName_Changed(object sender, EventArgs e)
        {
            if (!populatingForm)
            {
                if (!string.IsNullOrWhiteSpace(txtAnimationName.Text))
                    _animation.Name = txtAnimationName.Text;
                else
                {
                    MessageBox.Show("Animation Name cannot be blank", "Name is required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtAnimationName.Text = _animation.Name;
                    txtAnimationName.Focus();
                }
            }
        }

        private void LoadAnimationImage_Click(object sender, EventArgs e)
        {
            // Allow user to open a new image file for animating
            openFileDialog.Title = "Load Background Image";
            openFileDialog.Filter = "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff";
            openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.Combine(baseFolder, ((AnimationImage)_animation).ImagePath));
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var relativePath = Path.Combine(".\\InstrumentImages", Path.GetFileName(openFileDialog.FileName));
                    if (Path.GetFullPath(Path.Combine(baseFolder, relativePath)) != Path.GetFullPath(openFileDialog.FileName))
                        File.Copy(openFileDialog.FileName, Path.Combine(baseFolder, relativePath));
                    ((AnimationImage)_animation).ImagePath = relativePath;
                    ShowImage(Image.FromFile(relativePath), pbAnimationImage);
                    txtAnimationImagePath.Text = relativePath;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(string.Format("Unable to use selected file:\r{0}\rError: ", openFileDialog.FileName, ex.Message), "Invalid file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PlotPoint_Change(object sender, DataGridViewCellEventArgs e)
        {
            // Modify Config to contain all populated X/Y values
            if (_animation != null && !populatingForm)
            {
                var newPoints = new List<AnimationPoint>();
                foreach (DataGridViewRow row in dgAnimationPlotPoints.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row.Cells["pointX"].Value?.ToString()) && !string.IsNullOrWhiteSpace(row.Cells["pointY"].Value?.ToString()))
                    {
                        var xString = row.Cells["pointX"].Value.ToString();
                        var yString = row.Cells["pointY"].Value.ToString();
                        float x;
                        float y;
                        if (float.TryParse(xString, out x) && float.TryParse(yString, out y))
                        {
                            newPoints.Add(new AnimationPoint(x, y));
                        }
                    }
                }
                ((AnimationDrawing)_animation).PointMap = newPoints.ToArray();
            }
        }

        private void PlotPointAdd_Change(object sender, DataGridViewRowsAddedEventArgs e)
        {
            PlotPoint_Change(sender, new DataGridViewCellEventArgs(0, e.RowIndex));
        }

        private void PlotPointRemove_Change(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            PlotPoint_Change(sender, new DataGridViewCellEventArgs(0, e.RowIndex));
        }
    }
}
