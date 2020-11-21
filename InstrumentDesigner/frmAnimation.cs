using RemoteCockpitClasses.Animations;
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

namespace InstrumentDesigner
{
    public partial class frmAnimation : Form
    {
        private readonly IAnimationItem _animation;
        public IAnimationItem DialogValue { get { return _animation; } }
        private readonly string baseFolder;
        public frmAnimation(IAnimationItem animation, string cockpitBaseFolder)
        {
            InitializeComponent();
            _animation = animation;
            baseFolder = cockpitBaseFolder;
            Initialize();
        }

        private void Initialize()
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
            foreach(var color in ((KnownColor[])Enum.GetValues(typeof(KnownColor))).OrderBy(x => x.ToString()))
            {
                cmbAnimationFillColor.Items.Add(color.ToString());
            }
            cmbAnimationFillMethod.Items.Clear();
            foreach(var fillMethod in ((FillMode[])Enum.GetValues(typeof(FillMode))).OrderBy(x => x.ToString()))
            {
                cmbAnimationFillMethod.Items.Add(fillMethod.ToString());
            }
            for (var tabId = 0; tabId < tabCollection.TabCount; tabId++)
            {
                ClearTab(tabId);
                PopulateTab(tabId);
            }
        }

        private void ClearTab(int tabId)
        {
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
        }

        private void PopulateTab(int tabId)
        {
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
                        txtAnimationImagePath.Text = ((AnimationImage)_animation).ImagePath;
                        cmbAnimationScaleMethod.SelectedIndex = cmbAnimationScaleMethod.Items.IndexOf(((AnimationImage)_animation).ScaleMethod.ToString());
                        //txtAnimationRelativeX.Text = _animation.RelativeX.ToString();
                        //txtAnimationRelativeY.Text = _animation.RelativeY.ToString();
                        try
                        {
                            var image = Image.FromFile(Path.Combine(baseFolder,txtAnimationImagePath.Text));
                            ShowImage(image, pbAnimationImage);
                        }
                        catch { }
                    }
                    if(_animation.Type == AnimationItemTypeEnum.Drawing)
                    {
                        gpAnimationDrawing.Visible = true;
                        cmbAnimationFillColor.SelectedIndex = cmbAnimationFillColor.Items.IndexOf(((AnimationDrawing)_animation).FillColor.ToKnownColor().ToString());
                        cmbAnimationFillMethod.SelectedIndex = cmbAnimationFillMethod.Items.IndexOf(((AnimationDrawing)_animation).FillMethod.ToString());

                    }
                    break;
                case 1:
                    break;
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

        private void ChangeAnimationType_Select(object sender, EventArgs e)
        {
            var selectedType = cmbAnimationType.SelectedItem?.ToString();
            gpAnimationDrawing.Visible = false;
            gpAnimationImage.Visible = false;
            if (selectedType == "Image")
            {
                gpAnimationImage.Visible = true;
            }
            if(selectedType == "Drawing")
            {
                gpAnimationDrawing.Visible = true;
            }
        }
    }
}
