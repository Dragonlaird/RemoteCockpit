using RemoteCockpitClasses;
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
            cmbAnimationType.DataSource = ((AnimationItemTypeEnum[])Enum.GetValues(typeof(AnimationItemTypeEnum))).OrderBy(x => x.ToString()).ToList();
            //foreach (var itemType in ((AnimationItemTypeEnum[])Enum.GetValues(typeof(AnimationItemTypeEnum))).OrderBy(x => x.ToString()))
            //{
            //    cmbAnimationType.Items.Add(itemType.ToString());
            //}
            cmbAnimationScaleMethod.Items.Clear();
            cmbAnimationScaleMethod.DataSource = ((AnimationScaleMethodEnum[])Enum.GetValues(typeof(AnimationScaleMethodEnum))).OrderBy(x => x.ToString()).ToList();
            //foreach (var scaleType in ((AnimationScaleMethodEnum[])Enum.GetValues(typeof(AnimationScaleMethodEnum))).OrderBy(x => x.ToString()))
            //{
            //    cmbAnimationScaleMethod.Items.Add(scaleType.ToString());
            //}
            cmbAnimationFillColor.Items.Clear();
            cmbAnimationFillColor.DataSource = ((KnownColor[])Enum.GetValues(typeof(KnownColor))).OrderBy(x => x.ToString()).ToList();
            //foreach (var color in ((KnownColor[])Enum.GetValues(typeof(KnownColor))).OrderBy(x => x.ToString()))
            //{
            //    cmbAnimationFillColor.Items.Add(color.ToString());
            //}
            cmbAnimationFillMethod.Items.Clear();
            cmbAnimationFillMethod.DataSource = ((FillType[])Enum.GetValues(typeof(FillType))).OrderBy(x => x.ToString()).ToList();
            //foreach (var fillMethod in ((FillType[])Enum.GetValues(typeof(FillType))).OrderBy(x => x.ToString()))
            //{
            //    cmbAnimationFillMethod.Items.Add(fillMethod.ToString());
            //}
            var triggerTypeCol = (DataGridViewComboBoxColumn)gdAnimationTriggers.Columns["Type"];
            triggerTypeCol.Items.Clear();
            triggerTypeCol.DataSource = ((AnimationTriggerTypeEnum[])Enum.GetValues(typeof(AnimationTriggerTypeEnum))).OrderBy(x => x.ToString()).ToList();
            //foreach (var triggerType in ((AnimationTriggerTypeEnum[])Enum.GetValues(typeof(AnimationTriggerTypeEnum))).OrderBy(x=> x.ToString())){
            //    triggerTypeCol.Items.Add(triggerType);
            //}
            var variables = SimVarUnits.DefaultUnits;
            cmbAnimationVariableNames.Items.Clear();
            cmbAnimationVariableNames.DataSource = variables.Keys.OrderBy(x => x).ToList();
            //foreach (string variableName in variables.Keys.OrderBy(x => x))
            //{
            //    cmbAnimationVariableNames.Items.Add(variableName);
            //}
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
                    cmbAnimationType.SelectedIndex = cmbAnimationType.Items.IndexOf(_animation.Type);
                    if (_animation.Type == AnimationItemTypeEnum.Image)
                    {
                        gpAnimationImage.Visible = true;
                        txtAnimationImagePath.Text = ((AnimationImage)_animation).ImagePath ?? "";
                        cmbAnimationScaleMethod.SelectedIndex = cmbAnimationScaleMethod.Items.IndexOf(((AnimationImage)_animation).ScaleMethod);
                        //txtAnimationRelativeX.Text = _animation.RelativeX.ToString();
                        //txtAnimationRelativeY.Text = _animation.RelativeY.ToString();
                        try
                        {
                            var image = Image.FromFile(Path.Combine(baseFolder, txtAnimationImagePath.Text));
                            if (image != null)
                                ShowImage(image, pbAnimationImage);
                        }
                        catch { }
                    }
                    if (_animation.Type == AnimationItemTypeEnum.Drawing)
                    {
                        gpAnimationDrawing.Visible = true;
                        if (((AnimationDrawing)_animation).PointMap == null)
                        {
                            ((AnimationDrawing)_animation).PointMap = new AnimationPoint[0];
                        }
                        cmbAnimationFillColor.SelectedIndex = cmbAnimationFillColor.Items.IndexOf(((AnimationDrawing)_animation).FillColor.ToKnownColor());
                        cmbAnimationFillMethod.SelectedIndex = cmbAnimationFillMethod.Items.IndexOf(((AnimationDrawing)_animation).FillMethod);
                        dgAnimationPlotPoints.Rows.Clear();
                        foreach (var plotPoint in ((AnimationDrawing)_animation).PointMap)
                        {
                            dgAnimationPlotPoints.Rows.Add(new object[] { plotPoint.X, plotPoint.Y });
                        }
                    }
                    break;
                case 1:
                    gdAnimationTriggers.Rows.Clear();
                    if (_animation.Triggers != null)
                    {
                        foreach (var trigger in _animation.Triggers)
                        {
                            gdAnimationTriggers.Rows.Add(new object[] { trigger.Name, trigger.Type });
                        }
                    }
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
            var initialFolder = Path.GetDirectoryName(string.IsNullOrEmpty(((AnimationImage)_animation).ImagePath)?Path.Combine(Directory.GetCurrentDirectory(), ".\\InstrumentImages"):Path.Combine(Directory.GetCurrentDirectory(), ((AnimationImage)_animation).ImagePath));
            openFileDialog.InitialDirectory = initialFolder;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                populatingForm = true;
                try
                {
                    var relativePath = Path.Combine(".\\InstrumentImages", Path.GetFileName(openFileDialog.FileName));
                    if (Path.GetFullPath(Path.Combine(baseFolder, relativePath)) != Path.GetFullPath(openFileDialog.FileName))
                        File.Copy(openFileDialog.FileName, Path.Combine(baseFolder, relativePath));
                    ((AnimationImage)_animation).ImagePath = relativePath;
                    ShowImage(Image.FromFile(relativePath), pbAnimationImage);
                    txtAnimationImagePath.Text = relativePath;
                    populatingForm = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Unable to use selected file:\r{0}\rError: ", openFileDialog.FileName, ex.Message), "Invalid file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                populatingForm = false;
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

        private void TabSelection_Change(object sender, EventArgs e)
        {
            // If this is the How tab - hide all controls except the warning label
            // If a Tigger is selected, dispay the correct container and hide the waring label
            lblAnimationHowNoTrigger.Visible = true;
            gpAnimationClientRequest.Visible = false;


        }

        private void RowSelection_Change(object sender, EventArgs e)
        {
            if (!populatingForm)
            {
                gpAnimationClientRequest.Visible = false;
                var senderGrid = (DataGridView)sender;
                if (senderGrid.SelectedRows.Count == 1)
                {
                    var trigger = _animation.Triggers?.FirstOrDefault(x => x.Name == senderGrid.SelectedRows[0].Cells["Trigger"].Value?.ToString());
                    switch (senderGrid.SelectedRows[0].Cells["Type"].EditedFormattedValue?.ToString())
                    {
                        case "ClientRequest":
                            gpAnimationClientRequest.Visible = true;
                            if (trigger != null)
                            {
                                var selectedIdx = cmbAnimationVariableNames.Items.IndexOf(((AnimationTriggerClientRequest)trigger).Request?.Name);
                                cmbAnimationVariableNames.SelectedIndex = selectedIdx;
                                //if (!string.IsNullOrEmpty(cmbAnimationVariableNames.Items[selectedIdx]?.ToString()))
                                //{

                                //}
                            }
                            break;
                        case "Timer":
                        case "MouseClick":
                            break;
                    }
                }
            }
        }

        private void TriggerMisconfigured(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Just used to override an error that can be ignored
        }

        private void VariableName_Change(object sender, EventArgs e)
        {
            // Display associated units
            var selectedVariableName = ((ComboBox)sender).Items[((ComboBox)sender).SelectedIndex]?.ToString();
            var selectedVariable = SimVarUnits.DefaultUnits.FirstOrDefault(x => x.Key == selectedVariableName);
            txtAnimationClientRequestUnits.Text = selectedVariable.Value?.DefaultUnit ?? "";
        }

        private void OverrideUnits_Change(object sender, EventArgs e)
        {
            txtAnimationClientRequestUnits.Enabled = ((CheckBox)sender).Checked;
        }
    }
}
