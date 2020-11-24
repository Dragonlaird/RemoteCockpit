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
            cmbAnimationActionStyle.Items.Clear();
            cmbAnimationActionStyle.DataSource = ((AnimateActionClipEnum[])Enum.GetValues(typeof(AnimateActionClipEnum))).OrderBy(x => x.ToString()).ToList();
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
            var actionTypeCol = (DataGridViewComboBoxColumn)dgAnimationActions.Columns["ActionType"];
            actionTypeCol.Items.Clear();
            actionTypeCol.DataSource = ((AnimationActionTypeEnum[])Enum.GetValues(typeof(AnimationActionTypeEnum))).OrderBy(x => x.ToString()).ToList();

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
            IAnimationTrigger trigger = null;
            switch (tabId)
            {
                case 0:
                    //What tab
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
                    // When tab
                    gdAnimationTriggers.Rows.Clear();
                    if (_animation.Triggers != null)
                    {
                        foreach (var t in _animation.Triggers)
                        {
                            trigger = t;
                            gdAnimationTriggers.Rows.Add(new object[] { trigger.Name, trigger.Type });
                        }
                    }
                    break;
                case 2:
                    // How tab
                    lblAnimationHowNoTrigger.Visible = true;
                    lblAnimationActions.Visible = false;
                    dgAnimationActions.Visible = false;
                    gpAnimationActionClip.Visible = false;
                    gpAnimationActionRotate.Visible = false;
                    if (gdAnimationTriggers.SelectedRows.Count == 1 && _animation.Triggers != null)
                    {
                        trigger = _animation.Triggers.FirstOrDefault(x => x.Name == gdAnimationTriggers.SelectedRows[0].Cells["Trigger"]?.Value?.ToString());
                        if (trigger != null)
                        {
                            lblAnimationActions.Visible = true;
                            dgAnimationActions.Visible = true;
                            lblAnimationHowNoTrigger.Visible = false;
                            dgAnimationActions.Rows.Clear();
                            if (trigger.Actions != null)
                                foreach (var action in trigger.Actions)
                                {
                                    var rowIdx = dgAnimationActions.Rows.Add();
                                    var itemToSet = ((DataGridViewComboBoxCell)dgAnimationActions.Rows[rowIdx].Cells["ActionType"]).Items[((DataGridViewComboBoxCell)dgAnimationActions.Rows[rowIdx].Cells["ActionType"]).Items.IndexOf(action.Type)];
                                    ((DataGridViewComboBoxCell)dgAnimationActions.Rows[rowIdx].Cells["ActionType"]).Value = itemToSet;
                                }
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

        //private void TabSelection_Change(object sender, EventArgs e)
        //{
        //    // If this is the How tab - hide all controls except the warning label
        //    // If a Tigger is selected, dispay the correct container and hide the waring label
        //    lblAnimationHowNoTrigger.Visible = true;
        //    gpAnimationClientRequest.Visible = false;


        //}

        private void TriggerAdded_Change(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //throw new NotImplementedException();
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
                            if (trigger != null && ((AnimationTriggerClientRequest)trigger).Request?.Name != null)
                            {
                                var selectedIdx = cmbAnimationVariableNames.Items.IndexOf(((AnimationTriggerClientRequest)trigger).Request?.Name);
                                cmbAnimationVariableNames.SelectedIndex = selectedIdx;
                            }
                            break;
                        case "Timer":
                        case "MouseClick":
                            break;
                    }
                }
                // Now update the How tab
                PopulateTab(2);
            }
        }

        private void TriggerMisconfigured(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Just used to override an error that can be ignored
        }

        private void TriggerRow_Added(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!populatingForm)
            {
                // New row being added - need to also add to Animation config
                var newAnimations = _animation.Triggers.ToList();
                newAnimations.Add(new AnimationTriggerClientRequest { Name = "", Actions = new IAnimationAction[0], Request = new ClientRequest() });
                _animation.Triggers = newAnimations.ToArray();
            }
        }

        private void VariableName_Change(object sender, EventArgs e)
        {
            // Display associated units
            var selectedVariableName = ((ComboBox)sender).Items[((ComboBox)sender).SelectedIndex]?.ToString();
            var selectedVariable = SimVarUnits.DefaultUnits.FirstOrDefault(x => x.Key == selectedVariableName);
            txtAnimationClientRequestUnits.Text = selectedVariable.Value?.DefaultUnit ?? "";
            if (!populatingForm)
            {
                var trigger = (AnimationTriggerClientRequest)_animation.Triggers.FirstOrDefault(x => x.Name == gdAnimationTriggers.SelectedRows[0].Cells["Trigger"].Value?.ToString());
                var triggerId = _animation.Triggers.ToList().IndexOf(trigger);
                trigger.Request = new ClientRequest { Name = selectedVariableName, Unit = selectedVariable.Value.DefaultUnit };
                _animation.Triggers[triggerId] = trigger;
            }
        }

        private void OverrideUnits_Change(object sender, EventArgs e)
        {
            txtAnimationClientRequestUnits.Enabled = ((CheckBox)sender).Checked;
        }

        private void gdAnimationTriggers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Find out which cell has changed - if it's the Name, change the animation trigger name, otherwise the Type Change handler will catch it
            if (_animation != null && !populatingForm)
            {
                var senderGrid = (DataGridView)sender;
                var colIdx = e.ColumnIndex;
                var colName = senderGrid.Columns[colIdx].Name?.ToString();
                if (colName == "Trigger")
                {
                    var trigger = _animation.Triggers[e.RowIndex];
                    trigger.Name = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                    _animation.Triggers[e.RowIndex] = trigger;
                }
            }
        }

        private void ActionSelect_Change(object sender, EventArgs e)
        {
            gpAnimationActionClip.Visible = false;
            gpAnimationActionRotate.Visible = false;
            var senderGrid = (DataGridView)sender;
            if(senderGrid.SelectedRows.Count == 1)
            {
                var actionType = (AnimationActionTypeEnum)senderGrid.SelectedRows[0].Cells["ActionType"].Value;
                var action = GetSelectedAction();
                switch (actionType)
                {
                    case AnimationActionTypeEnum.Clip:
                        var actionClip = (AnimateActionClip)action;
                        gpAnimationActionClip.Visible = true;
                        cmbAnimationActionStyle.SelectedIndex = cmbAnimationActionStyle.Items.IndexOf(actionClip.Style);
                        txtAnimationActionStartX.Value = (decimal)actionClip.StartPoint.X;
                        txtAnimationActionStartY.Value = (decimal)actionClip.StartPoint.Y;
                        txtAnimationActionEndX.Value = (decimal)actionClip.EndPoint.X;
                        txtAnimationActionEndY.Value = (decimal)actionClip.EndPoint.Y;

                        break;
                    case AnimationActionTypeEnum.Rotate:
                        var actionRotate = (AnimationActionRotate)action;
                        gpAnimationActionRotate.Visible = true;
                        txtAnimationActionCentrePointX.Value = (decimal)actionRotate.CentrePoint.X;
                        txtAnimationActionCentrePointY.Value = (decimal)actionRotate.CentrePoint.Y;
                        cbAnimationActionRotateClockwise.Checked = actionRotate.RotateClockwise;
                        txtAnimationActionRotateMaxVal.Value = (decimal)actionRotate.MaximumValueExpected;
                        break;
                }
            }
        }

        private IAnimationAction GetSelectedAction()
        {
            var actionType = (AnimationActionTypeEnum)dgAnimationActions.SelectedRows[0].Cells["ActionType"].Value;
            var trigger = _animation.Triggers.First(x => x.Name == gdAnimationTriggers.SelectedRows[0].Cells["Trigger"].Value?.ToString());
            return trigger.Actions.First(x => x.Type == actionType);
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        private void dataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                var senderGrid = (DataGridView)sender;
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = senderGrid.DoDragDrop(
                          senderGrid.Rows[rowIndexFromMouseDown],
                          DragDropEffects.Move);
                }
            }
        }

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = senderGrid.HitTest(e.X, e.Y).RowIndex;

            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(
                          new Point(
                            e.X - (dragSize.Width / 2),
                            e.Y - (dragSize.Height / 2)),
                      dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dataGridView_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = senderGrid.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = senderGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                senderGrid.Rows.RemoveAt(rowIndexFromMouseDown);
                senderGrid.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
            }
        }
    }
}
