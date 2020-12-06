using System;
using System.Windows.Forms;

namespace InstrumentDesigner
{
    partial class frmAnimation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabCollection = new System.Windows.Forms.TabControl();
            this.tabWhat = new System.Windows.Forms.TabPage();
            this.lblAnimationTypeWarning = new System.Windows.Forms.Label();
            this.gpAnimationDrawing = new System.Windows.Forms.GroupBox();
            this.lblAnimationPointMap = new System.Windows.Forms.Label();
            this.dgAnimationPlotPoints = new System.Windows.Forms.DataGridView();
            this.pointX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pointY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbAnimationFillMethod = new System.Windows.Forms.ComboBox();
            this.lblAnimationFillMethod = new System.Windows.Forms.Label();
            this.cmbAnimationFillColor = new System.Windows.Forms.ComboBox();
            this.lblAnimationFillColor = new System.Windows.Forms.Label();
            this.gpAnimationImage = new System.Windows.Forms.GroupBox();
            this.cmdLoadAnimationImage = new System.Windows.Forms.Button();
            this.txtAnimationRelativeY = new System.Windows.Forms.NumericUpDown();
            this.txtAnimationRelativeX = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationRelativeY = new System.Windows.Forms.Label();
            this.lblAnimationRelativeX = new System.Windows.Forms.Label();
            this.pbAnimationImage = new System.Windows.Forms.PictureBox();
            this.txtAnimationImagePath = new System.Windows.Forms.TextBox();
            this.lblAnimationImagePath = new System.Windows.Forms.Label();
            this.cmbAnimationType = new System.Windows.Forms.ComboBox();
            this.lblAnimationType = new System.Windows.Forms.Label();
            this.txtAnimationName = new System.Windows.Forms.TextBox();
            this.lblAnimationName = new System.Windows.Forms.Label();
            this.tabWhen = new System.Windows.Forms.TabPage();
            this.gpAnimationClientRequest = new System.Windows.Forms.GroupBox();
            this.cbAnimationClientRequestUnitsOverride = new System.Windows.Forms.CheckBox();
            this.txtAnimationClientRequestUnits = new System.Windows.Forms.TextBox();
            this.lblAnimationClientRequestUnits = new System.Windows.Forms.Label();
            this.cmbAnimationVariableNames = new System.Windows.Forms.ComboBox();
            this.lblAnimationClientRequestName = new System.Windows.Forms.Label();
            this.gdAnimationTriggers = new System.Windows.Forms.DataGridView();
            this.Trigger = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tabHow = new System.Windows.Forms.TabPage();
            this.gpAnimationActionMoveX = new System.Windows.Forms.GroupBox();
            this.txtAnimationActionMoveXValue = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationActionMoveXPercent = new System.Windows.Forms.Label();
            this.txtAnimationActionMoveXMax = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationActionMoveXMax = new System.Windows.Forms.Label();
            this.gpAnimationActionClip = new System.Windows.Forms.GroupBox();
            this.txtAnimationActionEndY = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationActionEndY = new System.Windows.Forms.Label();
            this.txtAnimationActionEndX = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationActionEndX = new System.Windows.Forms.Label();
            this.lblAnimationActionEndPoint = new System.Windows.Forms.Label();
            this.txtAnimationActionStartY = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationActionStartY = new System.Windows.Forms.Label();
            this.txtAnimationActionStartX = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationActionStartX = new System.Windows.Forms.Label();
            this.lblAnimationActionStartPoint = new System.Windows.Forms.Label();
            this.cmbAnimationActionStyle = new System.Windows.Forms.ComboBox();
            this.lblAnimationActionStyle = new System.Windows.Forms.Label();
            this.gpAnimationActionRotate = new System.Windows.Forms.GroupBox();
            this.txtAnimationActionCentrePointY = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationActionCentrePointY = new System.Windows.Forms.Label();
            this.txtAnimationActionCentrePointX = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationActionCentrePointX = new System.Windows.Forms.Label();
            this.lblAnimationActionCentrePoint = new System.Windows.Forms.Label();
            this.txtAnimationActionRotateMaxVal = new System.Windows.Forms.NumericUpDown();
            this.lblAnimationActionRotateMaxVal = new System.Windows.Forms.Label();
            this.cbAnimationActionRotateClockwise = new System.Windows.Forms.CheckBox();
            this.txtAnimationActionRotateClockwise = new System.Windows.Forms.Label();
            this.lblAnimationActions = new System.Windows.Forms.Label();
            this.dgAnimationActions = new System.Windows.Forms.DataGridView();
            this.ActionType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lblAnimationHowNoTrigger = new System.Windows.Forms.Label();
            this.cmdAnimationSave = new System.Windows.Forms.Button();
            this.cmdAnimationCancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabCollection.SuspendLayout();
            this.tabWhat.SuspendLayout();
            this.gpAnimationDrawing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAnimationPlotPoints)).BeginInit();
            this.gpAnimationImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationRelativeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationRelativeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimationImage)).BeginInit();
            this.tabWhen.SuspendLayout();
            this.gpAnimationClientRequest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdAnimationTriggers)).BeginInit();
            this.tabHow.SuspendLayout();
            this.gpAnimationActionMoveX.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionMoveXValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionMoveXMax)).BeginInit();
            this.gpAnimationActionClip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionEndY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionEndX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionStartY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionStartX)).BeginInit();
            this.gpAnimationActionRotate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionCentrePointY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionCentrePointX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionRotateMaxVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgAnimationActions)).BeginInit();
            this.SuspendLayout();
            // 
            // tabCollection
            // 
            this.tabCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCollection.Controls.Add(this.tabWhat);
            this.tabCollection.Controls.Add(this.tabWhen);
            this.tabCollection.Controls.Add(this.tabHow);
            this.tabCollection.Location = new System.Drawing.Point(12, 12);
            this.tabCollection.Name = "tabCollection";
            this.tabCollection.SelectedIndex = 0;
            this.tabCollection.Size = new System.Drawing.Size(474, 364);
            this.tabCollection.TabIndex = 0;
            // 
            // tabWhat
            // 
            this.tabWhat.Controls.Add(this.lblAnimationTypeWarning);
            this.tabWhat.Controls.Add(this.gpAnimationDrawing);
            this.tabWhat.Controls.Add(this.gpAnimationImage);
            this.tabWhat.Controls.Add(this.cmbAnimationType);
            this.tabWhat.Controls.Add(this.lblAnimationType);
            this.tabWhat.Controls.Add(this.txtAnimationName);
            this.tabWhat.Controls.Add(this.lblAnimationName);
            this.tabWhat.Location = new System.Drawing.Point(4, 22);
            this.tabWhat.Name = "tabWhat";
            this.tabWhat.Padding = new System.Windows.Forms.Padding(3);
            this.tabWhat.Size = new System.Drawing.Size(466, 338);
            this.tabWhat.TabIndex = 0;
            this.tabWhat.Text = "What";
            this.tabWhat.UseVisualStyleBackColor = true;
            // 
            // lblAnimationTypeWarning
            // 
            this.lblAnimationTypeWarning.AutoSize = true;
            this.lblAnimationTypeWarning.ForeColor = System.Drawing.Color.Red;
            this.lblAnimationTypeWarning.Location = new System.Drawing.Point(256, 36);
            this.lblAnimationTypeWarning.Name = "lblAnimationTypeWarning";
            this.lblAnimationTypeWarning.Size = new System.Drawing.Size(207, 13);
            this.lblAnimationTypeWarning.TabIndex = 14;
            this.lblAnimationTypeWarning.Text = "Warning: Changing this removes all values";
            // 
            // gpAnimationDrawing
            // 
            this.gpAnimationDrawing.Controls.Add(this.lblAnimationPointMap);
            this.gpAnimationDrawing.Controls.Add(this.dgAnimationPlotPoints);
            this.gpAnimationDrawing.Controls.Add(this.cmbAnimationFillMethod);
            this.gpAnimationDrawing.Controls.Add(this.lblAnimationFillMethod);
            this.gpAnimationDrawing.Controls.Add(this.cmbAnimationFillColor);
            this.gpAnimationDrawing.Controls.Add(this.lblAnimationFillColor);
            this.gpAnimationDrawing.Location = new System.Drawing.Point(6, 98);
            this.gpAnimationDrawing.Name = "gpAnimationDrawing";
            this.gpAnimationDrawing.Size = new System.Drawing.Size(442, 234);
            this.gpAnimationDrawing.TabIndex = 7;
            this.gpAnimationDrawing.TabStop = false;
            this.gpAnimationDrawing.Text = "Drawing";
            this.gpAnimationDrawing.Visible = false;
            // 
            // lblAnimationPointMap
            // 
            this.lblAnimationPointMap.AutoSize = true;
            this.lblAnimationPointMap.Location = new System.Drawing.Point(258, 17);
            this.lblAnimationPointMap.Name = "lblAnimationPointMap";
            this.lblAnimationPointMap.Size = new System.Drawing.Size(107, 13);
            this.lblAnimationPointMap.TabIndex = 12;
            this.lblAnimationPointMap.Text = "Cartesian Plot Points:";
            // 
            // dgAnimationPlotPoints
            // 
            this.dgAnimationPlotPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAnimationPlotPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pointX,
            this.pointY});
            this.dgAnimationPlotPoints.Location = new System.Drawing.Point(258, 33);
            this.dgAnimationPlotPoints.Name = "dgAnimationPlotPoints";
            this.dgAnimationPlotPoints.RowHeadersWidth = 62;
            this.dgAnimationPlotPoints.Size = new System.Drawing.Size(178, 195);
            this.dgAnimationPlotPoints.TabIndex = 11;
            this.dgAnimationPlotPoints.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PlotPoint_Change);
            this.dgAnimationPlotPoints.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.PlotPointAdd_Change);
            this.dgAnimationPlotPoints.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.PlotPointRemove_Change);
            this.dgAnimationPlotPoints.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dgAnimationPlotPoints.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragOver);
            this.dgAnimationPlotPoints.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseDown);
            this.dgAnimationPlotPoints.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseMove);
            // 
            // pointX
            // 
            this.pointX.HeaderText = "X";
            this.pointX.MinimumWidth = 8;
            this.pointX.Name = "pointX";
            this.pointX.Width = 50;
            // 
            // pointY
            // 
            this.pointY.HeaderText = "Y";
            this.pointY.MinimumWidth = 8;
            this.pointY.Name = "pointY";
            this.pointY.Width = 50;
            // 
            // cmbAnimationFillMethod
            // 
            this.cmbAnimationFillMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationFillMethod.FormattingEnabled = true;
            this.cmbAnimationFillMethod.Location = new System.Drawing.Point(69, 65);
            this.cmbAnimationFillMethod.Name = "cmbAnimationFillMethod";
            this.cmbAnimationFillMethod.Size = new System.Drawing.Size(121, 21);
            this.cmbAnimationFillMethod.TabIndex = 10;
            // 
            // lblAnimationFillMethod
            // 
            this.lblAnimationFillMethod.AutoSize = true;
            this.lblAnimationFillMethod.Location = new System.Drawing.Point(2, 68);
            this.lblAnimationFillMethod.Name = "lblAnimationFillMethod";
            this.lblAnimationFillMethod.Size = new System.Drawing.Size(61, 13);
            this.lblAnimationFillMethod.TabIndex = 9;
            this.lblAnimationFillMethod.Text = "Fill Method:";
            // 
            // cmbAnimationFillColor
            // 
            this.cmbAnimationFillColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationFillColor.FormattingEnabled = true;
            this.cmbAnimationFillColor.Location = new System.Drawing.Point(69, 33);
            this.cmbAnimationFillColor.Name = "cmbAnimationFillColor";
            this.cmbAnimationFillColor.Size = new System.Drawing.Size(121, 21);
            this.cmbAnimationFillColor.TabIndex = 8;
            this.cmbAnimationFillColor.SelectedIndexChanged += new System.EventHandler(this.FillColor_Change);
            // 
            // lblAnimationFillColor
            // 
            this.lblAnimationFillColor.AutoSize = true;
            this.lblAnimationFillColor.Location = new System.Drawing.Point(14, 36);
            this.lblAnimationFillColor.Name = "lblAnimationFillColor";
            this.lblAnimationFillColor.Size = new System.Drawing.Size(49, 13);
            this.lblAnimationFillColor.TabIndex = 7;
            this.lblAnimationFillColor.Text = "Fill Color:";
            // 
            // gpAnimationImage
            // 
            this.gpAnimationImage.Controls.Add(this.cmdLoadAnimationImage);
            this.gpAnimationImage.Controls.Add(this.txtAnimationRelativeY);
            this.gpAnimationImage.Controls.Add(this.txtAnimationRelativeX);
            this.gpAnimationImage.Controls.Add(this.lblAnimationRelativeY);
            this.gpAnimationImage.Controls.Add(this.lblAnimationRelativeX);
            this.gpAnimationImage.Controls.Add(this.pbAnimationImage);
            this.gpAnimationImage.Controls.Add(this.txtAnimationImagePath);
            this.gpAnimationImage.Controls.Add(this.lblAnimationImagePath);
            this.gpAnimationImage.Location = new System.Drawing.Point(6, 98);
            this.gpAnimationImage.Name = "gpAnimationImage";
            this.gpAnimationImage.Size = new System.Drawing.Size(442, 234);
            this.gpAnimationImage.TabIndex = 6;
            this.gpAnimationImage.TabStop = false;
            this.gpAnimationImage.Text = "Image";
            // 
            // cmdLoadAnimationImage
            // 
            this.cmdLoadAnimationImage.Location = new System.Drawing.Point(248, 71);
            this.cmdLoadAnimationImage.Name = "cmdLoadAnimationImage";
            this.cmdLoadAnimationImage.Size = new System.Drawing.Size(75, 23);
            this.cmdLoadAnimationImage.TabIndex = 24;
            this.cmdLoadAnimationImage.Text = "...";
            this.cmdLoadAnimationImage.UseVisualStyleBackColor = true;
            this.cmdLoadAnimationImage.Click += new System.EventHandler(this.LoadAnimationImage_Click);
            // 
            // txtAnimationRelativeY
            // 
            this.txtAnimationRelativeY.Location = new System.Drawing.Point(56, 43);
            this.txtAnimationRelativeY.Name = "txtAnimationRelativeY";
            this.txtAnimationRelativeY.Size = new System.Drawing.Size(120, 20);
            this.txtAnimationRelativeY.TabIndex = 23;
            // 
            // txtAnimationRelativeX
            // 
            this.txtAnimationRelativeX.Location = new System.Drawing.Point(56, 20);
            this.txtAnimationRelativeX.Name = "txtAnimationRelativeX";
            this.txtAnimationRelativeX.Size = new System.Drawing.Size(120, 20);
            this.txtAnimationRelativeX.TabIndex = 22;
            // 
            // lblAnimationRelativeY
            // 
            this.lblAnimationRelativeY.AutoSize = true;
            this.lblAnimationRelativeY.Location = new System.Drawing.Point(1, 45);
            this.lblAnimationRelativeY.Name = "lblAnimationRelativeY";
            this.lblAnimationRelativeY.Size = new System.Drawing.Size(48, 13);
            this.lblAnimationRelativeY.TabIndex = 20;
            this.lblAnimationRelativeY.Text = "Offset Y:";
            // 
            // lblAnimationRelativeX
            // 
            this.lblAnimationRelativeX.AutoSize = true;
            this.lblAnimationRelativeX.Location = new System.Drawing.Point(1, 22);
            this.lblAnimationRelativeX.Name = "lblAnimationRelativeX";
            this.lblAnimationRelativeX.Size = new System.Drawing.Size(48, 13);
            this.lblAnimationRelativeX.TabIndex = 18;
            this.lblAnimationRelativeX.Text = "Offset X:";
            // 
            // pbAnimationImage
            // 
            this.pbAnimationImage.Location = new System.Drawing.Point(56, 97);
            this.pbAnimationImage.Name = "pbAnimationImage";
            this.pbAnimationImage.Size = new System.Drawing.Size(166, 126);
            this.pbAnimationImage.TabIndex = 10;
            this.pbAnimationImage.TabStop = false;
            // 
            // txtAnimationImagePath
            // 
            this.txtAnimationImagePath.Enabled = false;
            this.txtAnimationImagePath.Location = new System.Drawing.Point(55, 71);
            this.txtAnimationImagePath.Name = "txtAnimationImagePath";
            this.txtAnimationImagePath.Size = new System.Drawing.Size(186, 20);
            this.txtAnimationImagePath.TabIndex = 7;
            // 
            // lblAnimationImagePath
            // 
            this.lblAnimationImagePath.AutoSize = true;
            this.lblAnimationImagePath.Location = new System.Drawing.Point(10, 76);
            this.lblAnimationImagePath.Name = "lblAnimationImagePath";
            this.lblAnimationImagePath.Size = new System.Drawing.Size(39, 13);
            this.lblAnimationImagePath.TabIndex = 6;
            this.lblAnimationImagePath.Text = "Image:";
            // 
            // cmbAnimationType
            // 
            this.cmbAnimationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationType.FormattingEnabled = true;
            this.cmbAnimationType.Location = new System.Drawing.Point(69, 32);
            this.cmbAnimationType.Name = "cmbAnimationType";
            this.cmbAnimationType.Size = new System.Drawing.Size(185, 21);
            this.cmbAnimationType.TabIndex = 3;
            this.cmbAnimationType.SelectedIndexChanged += new System.EventHandler(this.ChangeAnimationType_Select);
            // 
            // lblAnimationType
            // 
            this.lblAnimationType.AutoSize = true;
            this.lblAnimationType.Location = new System.Drawing.Point(28, 36);
            this.lblAnimationType.Name = "lblAnimationType";
            this.lblAnimationType.Size = new System.Drawing.Size(34, 13);
            this.lblAnimationType.TabIndex = 2;
            this.lblAnimationType.Text = "Type:";
            // 
            // txtAnimationName
            // 
            this.txtAnimationName.Location = new System.Drawing.Point(69, 7);
            this.txtAnimationName.Name = "txtAnimationName";
            this.txtAnimationName.Size = new System.Drawing.Size(185, 20);
            this.txtAnimationName.TabIndex = 1;
            this.txtAnimationName.TextChanged += new System.EventHandler(this.UpdateName_Changed);
            // 
            // lblAnimationName
            // 
            this.lblAnimationName.AutoSize = true;
            this.lblAnimationName.Location = new System.Drawing.Point(24, 10);
            this.lblAnimationName.Name = "lblAnimationName";
            this.lblAnimationName.Size = new System.Drawing.Size(38, 13);
            this.lblAnimationName.TabIndex = 0;
            this.lblAnimationName.Text = "Name:";
            // 
            // tabWhen
            // 
            this.tabWhen.Controls.Add(this.gpAnimationClientRequest);
            this.tabWhen.Controls.Add(this.gdAnimationTriggers);
            this.tabWhen.Location = new System.Drawing.Point(4, 22);
            this.tabWhen.Name = "tabWhen";
            this.tabWhen.Padding = new System.Windows.Forms.Padding(3);
            this.tabWhen.Size = new System.Drawing.Size(466, 338);
            this.tabWhen.TabIndex = 1;
            this.tabWhen.Text = "When";
            this.tabWhen.UseVisualStyleBackColor = true;
            // 
            // gpAnimationClientRequest
            // 
            this.gpAnimationClientRequest.Controls.Add(this.cbAnimationClientRequestUnitsOverride);
            this.gpAnimationClientRequest.Controls.Add(this.txtAnimationClientRequestUnits);
            this.gpAnimationClientRequest.Controls.Add(this.lblAnimationClientRequestUnits);
            this.gpAnimationClientRequest.Controls.Add(this.cmbAnimationVariableNames);
            this.gpAnimationClientRequest.Controls.Add(this.lblAnimationClientRequestName);
            this.gpAnimationClientRequest.Location = new System.Drawing.Point(7, 160);
            this.gpAnimationClientRequest.Name = "gpAnimationClientRequest";
            this.gpAnimationClientRequest.Size = new System.Drawing.Size(453, 172);
            this.gpAnimationClientRequest.TabIndex = 2;
            this.gpAnimationClientRequest.TabStop = false;
            this.gpAnimationClientRequest.Text = "Client Request";
            this.gpAnimationClientRequest.Visible = false;
            // 
            // cbAnimationClientRequestUnitsOverride
            // 
            this.cbAnimationClientRequestUnitsOverride.AutoSize = true;
            this.cbAnimationClientRequestUnitsOverride.Location = new System.Drawing.Point(250, 48);
            this.cbAnimationClientRequestUnitsOverride.Name = "cbAnimationClientRequestUnitsOverride";
            this.cbAnimationClientRequestUnitsOverride.Size = new System.Drawing.Size(72, 17);
            this.cbAnimationClientRequestUnitsOverride.TabIndex = 4;
            this.cbAnimationClientRequestUnitsOverride.Text = "Override?";
            this.cbAnimationClientRequestUnitsOverride.UseVisualStyleBackColor = true;
            this.cbAnimationClientRequestUnitsOverride.Visible = false;
            this.cbAnimationClientRequestUnitsOverride.CheckedChanged += new System.EventHandler(this.OverrideUnits_Change);
            // 
            // txtAnimationClientRequestUnits
            // 
            this.txtAnimationClientRequestUnits.Enabled = false;
            this.txtAnimationClientRequestUnits.Location = new System.Drawing.Point(92, 45);
            this.txtAnimationClientRequestUnits.Name = "txtAnimationClientRequestUnits";
            this.txtAnimationClientRequestUnits.Size = new System.Drawing.Size(151, 20);
            this.txtAnimationClientRequestUnits.TabIndex = 3;
            // 
            // lblAnimationClientRequestUnits
            // 
            this.lblAnimationClientRequestUnits.AutoSize = true;
            this.lblAnimationClientRequestUnits.Location = new System.Drawing.Point(52, 48);
            this.lblAnimationClientRequestUnits.Name = "lblAnimationClientRequestUnits";
            this.lblAnimationClientRequestUnits.Size = new System.Drawing.Size(34, 13);
            this.lblAnimationClientRequestUnits.TabIndex = 2;
            this.lblAnimationClientRequestUnits.Text = "Units:";
            // 
            // cmbAnimationVariableNames
            // 
            this.cmbAnimationVariableNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationVariableNames.FormattingEnabled = true;
            this.cmbAnimationVariableNames.Location = new System.Drawing.Point(92, 17);
            this.cmbAnimationVariableNames.Name = "cmbAnimationVariableNames";
            this.cmbAnimationVariableNames.Size = new System.Drawing.Size(261, 21);
            this.cmbAnimationVariableNames.TabIndex = 1;
            this.cmbAnimationVariableNames.SelectedIndexChanged += new System.EventHandler(this.VariableName_Change);
            // 
            // lblAnimationClientRequestName
            // 
            this.lblAnimationClientRequestName.AutoSize = true;
            this.lblAnimationClientRequestName.Location = new System.Drawing.Point(7, 20);
            this.lblAnimationClientRequestName.Name = "lblAnimationClientRequestName";
            this.lblAnimationClientRequestName.Size = new System.Drawing.Size(79, 13);
            this.lblAnimationClientRequestName.TabIndex = 0;
            this.lblAnimationClientRequestName.Text = "Variable Name:";
            // 
            // gdAnimationTriggers
            // 
            this.gdAnimationTriggers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdAnimationTriggers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Trigger,
            this.Type});
            this.gdAnimationTriggers.Location = new System.Drawing.Point(6, 3);
            this.gdAnimationTriggers.Name = "gdAnimationTriggers";
            this.gdAnimationTriggers.RowHeadersWidth = 62;
            this.gdAnimationTriggers.Size = new System.Drawing.Size(454, 150);
            this.gdAnimationTriggers.TabIndex = 1;
            this.gdAnimationTriggers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gdAnimationTriggers_CellValueChanged);
            this.gdAnimationTriggers.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.TriggerMisconfigured);
            this.gdAnimationTriggers.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.TriggerRow_Added);
            this.gdAnimationTriggers.SelectionChanged += new System.EventHandler(this.RowSelection_Change);
            this.gdAnimationTriggers.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.gdAnimationTriggers.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragOver);
            this.gdAnimationTriggers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseDown);
            this.gdAnimationTriggers.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseMove);
            // 
            // Trigger
            // 
            this.Trigger.HeaderText = "Trigger";
            this.Trigger.MinimumWidth = 8;
            this.Trigger.Name = "Trigger";
            this.Trigger.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Trigger.Width = 150;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.MinimumWidth = 8;
            this.Type.Name = "Type";
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Type.Width = 150;
            // 
            // tabHow
            // 
            this.tabHow.Controls.Add(this.gpAnimationActionMoveX);
            this.tabHow.Controls.Add(this.gpAnimationActionClip);
            this.tabHow.Controls.Add(this.gpAnimationActionRotate);
            this.tabHow.Controls.Add(this.lblAnimationActions);
            this.tabHow.Controls.Add(this.dgAnimationActions);
            this.tabHow.Controls.Add(this.lblAnimationHowNoTrigger);
            this.tabHow.Location = new System.Drawing.Point(4, 22);
            this.tabHow.Name = "tabHow";
            this.tabHow.Padding = new System.Windows.Forms.Padding(3);
            this.tabHow.Size = new System.Drawing.Size(466, 338);
            this.tabHow.TabIndex = 2;
            this.tabHow.Text = "How";
            this.tabHow.UseVisualStyleBackColor = true;
            // 
            // gpAnimationActionMoveX
            // 
            this.gpAnimationActionMoveX.Controls.Add(this.txtAnimationActionMoveXValue);
            this.gpAnimationActionMoveX.Controls.Add(this.lblAnimationActionMoveXPercent);
            this.gpAnimationActionMoveX.Controls.Add(this.txtAnimationActionMoveXMax);
            this.gpAnimationActionMoveX.Controls.Add(this.lblAnimationActionMoveXMax);
            this.gpAnimationActionMoveX.Location = new System.Drawing.Point(6, 177);
            this.gpAnimationActionMoveX.Name = "gpAnimationActionMoveX";
            this.gpAnimationActionMoveX.Size = new System.Drawing.Size(454, 155);
            this.gpAnimationActionMoveX.TabIndex = 5;
            this.gpAnimationActionMoveX.TabStop = false;
            this.gpAnimationActionMoveX.Text = "MoveX";
            // 
            // txtAnimationActionMoveXValue
            // 
            this.txtAnimationActionMoveXValue.Location = new System.Drawing.Point(73, 40);
            this.txtAnimationActionMoveXValue.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.txtAnimationActionMoveXValue.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            -2147483648});
            this.txtAnimationActionMoveXValue.Name = "txtAnimationActionMoveXValue";
            this.txtAnimationActionMoveXValue.Size = new System.Drawing.Size(120, 20);
            this.txtAnimationActionMoveXValue.TabIndex = 3;
            this.txtAnimationActionMoveXValue.ValueChanged += new System.EventHandler(this.AnimationActionMovePercent_Changed);
            // 
            // lblAnimationActionMoveXPercent
            // 
            this.lblAnimationActionMoveXPercent.AutoSize = true;
            this.lblAnimationActionMoveXPercent.Location = new System.Drawing.Point(20, 42);
            this.lblAnimationActionMoveXPercent.Name = "lblAnimationActionMoveXPercent";
            this.lblAnimationActionMoveXPercent.Size = new System.Drawing.Size(47, 13);
            this.lblAnimationActionMoveXPercent.TabIndex = 2;
            this.lblAnimationActionMoveXPercent.Text = "Percent:";
            // 
            // txtAnimationActionMoveXMax
            // 
            this.txtAnimationActionMoveXMax.Location = new System.Drawing.Point(73, 18);
            this.txtAnimationActionMoveXMax.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtAnimationActionMoveXMax.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.txtAnimationActionMoveXMax.Name = "txtAnimationActionMoveXMax";
            this.txtAnimationActionMoveXMax.Size = new System.Drawing.Size(120, 20);
            this.txtAnimationActionMoveXMax.TabIndex = 1;
            this.txtAnimationActionMoveXMax.ValueChanged += new System.EventHandler(this.AnimationActionMoveMax_Changed);
            // 
            // lblAnimationActionMoveXMax
            // 
            this.lblAnimationActionMoveXMax.AutoSize = true;
            this.lblAnimationActionMoveXMax.Location = new System.Drawing.Point(7, 20);
            this.lblAnimationActionMoveXMax.Name = "lblAnimationActionMoveXMax";
            this.lblAnimationActionMoveXMax.Size = new System.Drawing.Size(60, 13);
            this.lblAnimationActionMoveXMax.TabIndex = 0;
            this.lblAnimationActionMoveXMax.Text = "Max Value:";
            // 
            // gpAnimationActionClip
            // 
            this.gpAnimationActionClip.Controls.Add(this.txtAnimationActionEndY);
            this.gpAnimationActionClip.Controls.Add(this.lblAnimationActionEndY);
            this.gpAnimationActionClip.Controls.Add(this.txtAnimationActionEndX);
            this.gpAnimationActionClip.Controls.Add(this.lblAnimationActionEndX);
            this.gpAnimationActionClip.Controls.Add(this.lblAnimationActionEndPoint);
            this.gpAnimationActionClip.Controls.Add(this.txtAnimationActionStartY);
            this.gpAnimationActionClip.Controls.Add(this.lblAnimationActionStartY);
            this.gpAnimationActionClip.Controls.Add(this.txtAnimationActionStartX);
            this.gpAnimationActionClip.Controls.Add(this.lblAnimationActionStartX);
            this.gpAnimationActionClip.Controls.Add(this.lblAnimationActionStartPoint);
            this.gpAnimationActionClip.Controls.Add(this.cmbAnimationActionStyle);
            this.gpAnimationActionClip.Controls.Add(this.lblAnimationActionStyle);
            this.gpAnimationActionClip.Location = new System.Drawing.Point(6, 176);
            this.gpAnimationActionClip.Name = "gpAnimationActionClip";
            this.gpAnimationActionClip.Size = new System.Drawing.Size(454, 155);
            this.gpAnimationActionClip.TabIndex = 4;
            this.gpAnimationActionClip.TabStop = false;
            this.gpAnimationActionClip.Text = "Clip";
            this.gpAnimationActionClip.Visible = false;
            // 
            // txtAnimationActionEndY
            // 
            this.txtAnimationActionEndY.Location = new System.Drawing.Point(250, 91);
            this.txtAnimationActionEndY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionEndY.Name = "txtAnimationActionEndY";
            this.txtAnimationActionEndY.Size = new System.Drawing.Size(68, 20);
            this.txtAnimationActionEndY.TabIndex = 11;
            this.txtAnimationActionEndY.ValueChanged += new System.EventHandler(this.AnimationActionClipEndY_Changed);
            // 
            // lblAnimationActionEndY
            // 
            this.lblAnimationActionEndY.AutoSize = true;
            this.lblAnimationActionEndY.Location = new System.Drawing.Point(227, 93);
            this.lblAnimationActionEndY.Name = "lblAnimationActionEndY";
            this.lblAnimationActionEndY.Size = new System.Drawing.Size(17, 13);
            this.lblAnimationActionEndY.TabIndex = 10;
            this.lblAnimationActionEndY.Text = "Y:";
            // 
            // txtAnimationActionEndX
            // 
            this.txtAnimationActionEndX.Location = new System.Drawing.Point(250, 65);
            this.txtAnimationActionEndX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionEndX.Name = "txtAnimationActionEndX";
            this.txtAnimationActionEndX.Size = new System.Drawing.Size(68, 20);
            this.txtAnimationActionEndX.TabIndex = 9;
            this.txtAnimationActionEndX.ValueChanged += new System.EventHandler(this.AnimationActionClipEndX_Changed);
            // 
            // lblAnimationActionEndX
            // 
            this.lblAnimationActionEndX.AutoSize = true;
            this.lblAnimationActionEndX.Location = new System.Drawing.Point(227, 67);
            this.lblAnimationActionEndX.Name = "lblAnimationActionEndX";
            this.lblAnimationActionEndX.Size = new System.Drawing.Size(17, 13);
            this.lblAnimationActionEndX.TabIndex = 8;
            this.lblAnimationActionEndX.Text = "X:";
            // 
            // lblAnimationActionEndPoint
            // 
            this.lblAnimationActionEndPoint.AutoSize = true;
            this.lblAnimationActionEndPoint.Location = new System.Drawing.Point(190, 47);
            this.lblAnimationActionEndPoint.Name = "lblAnimationActionEndPoint";
            this.lblAnimationActionEndPoint.Size = new System.Drawing.Size(56, 13);
            this.lblAnimationActionEndPoint.TabIndex = 7;
            this.lblAnimationActionEndPoint.Text = "End Point:";
            // 
            // txtAnimationActionStartY
            // 
            this.txtAnimationActionStartY.Location = new System.Drawing.Point(73, 91);
            this.txtAnimationActionStartY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionStartY.Name = "txtAnimationActionStartY";
            this.txtAnimationActionStartY.Size = new System.Drawing.Size(68, 20);
            this.txtAnimationActionStartY.TabIndex = 6;
            this.txtAnimationActionStartY.ValueChanged += new System.EventHandler(this.AnimationActionClipStartY_Changed);
            // 
            // lblAnimationActionStartY
            // 
            this.lblAnimationActionStartY.AutoSize = true;
            this.lblAnimationActionStartY.Location = new System.Drawing.Point(50, 93);
            this.lblAnimationActionStartY.Name = "lblAnimationActionStartY";
            this.lblAnimationActionStartY.Size = new System.Drawing.Size(17, 13);
            this.lblAnimationActionStartY.TabIndex = 5;
            this.lblAnimationActionStartY.Text = "Y:";
            // 
            // txtAnimationActionStartX
            // 
            this.txtAnimationActionStartX.Location = new System.Drawing.Point(73, 65);
            this.txtAnimationActionStartX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionStartX.Name = "txtAnimationActionStartX";
            this.txtAnimationActionStartX.Size = new System.Drawing.Size(68, 20);
            this.txtAnimationActionStartX.TabIndex = 4;
            this.txtAnimationActionStartX.ValueChanged += new System.EventHandler(this.AnimationActionClipStartX_Changed);
            // 
            // lblAnimationActionStartX
            // 
            this.lblAnimationActionStartX.AutoSize = true;
            this.lblAnimationActionStartX.Location = new System.Drawing.Point(50, 67);
            this.lblAnimationActionStartX.Name = "lblAnimationActionStartX";
            this.lblAnimationActionStartX.Size = new System.Drawing.Size(17, 13);
            this.lblAnimationActionStartX.TabIndex = 3;
            this.lblAnimationActionStartX.Text = "X:";
            // 
            // lblAnimationActionStartPoint
            // 
            this.lblAnimationActionStartPoint.AutoSize = true;
            this.lblAnimationActionStartPoint.Location = new System.Drawing.Point(13, 47);
            this.lblAnimationActionStartPoint.Name = "lblAnimationActionStartPoint";
            this.lblAnimationActionStartPoint.Size = new System.Drawing.Size(59, 13);
            this.lblAnimationActionStartPoint.TabIndex = 2;
            this.lblAnimationActionStartPoint.Text = "Start Point:";
            // 
            // cmbAnimationActionStyle
            // 
            this.cmbAnimationActionStyle.FormattingEnabled = true;
            this.cmbAnimationActionStyle.Location = new System.Drawing.Point(55, 17);
            this.cmbAnimationActionStyle.Name = "cmbAnimationActionStyle";
            this.cmbAnimationActionStyle.Size = new System.Drawing.Size(121, 21);
            this.cmbAnimationActionStyle.TabIndex = 1;
            this.cmbAnimationActionStyle.SelectedIndexChanged += new System.EventHandler(this.AnimationClipStyle_Changed);
            // 
            // lblAnimationActionStyle
            // 
            this.lblAnimationActionStyle.AutoSize = true;
            this.lblAnimationActionStyle.Location = new System.Drawing.Point(16, 20);
            this.lblAnimationActionStyle.Name = "lblAnimationActionStyle";
            this.lblAnimationActionStyle.Size = new System.Drawing.Size(33, 13);
            this.lblAnimationActionStyle.TabIndex = 0;
            this.lblAnimationActionStyle.Text = "Style:";
            // 
            // gpAnimationActionRotate
            // 
            this.gpAnimationActionRotate.Controls.Add(this.txtAnimationActionCentrePointY);
            this.gpAnimationActionRotate.Controls.Add(this.lblAnimationActionCentrePointY);
            this.gpAnimationActionRotate.Controls.Add(this.txtAnimationActionCentrePointX);
            this.gpAnimationActionRotate.Controls.Add(this.lblAnimationActionCentrePointX);
            this.gpAnimationActionRotate.Controls.Add(this.lblAnimationActionCentrePoint);
            this.gpAnimationActionRotate.Controls.Add(this.txtAnimationActionRotateMaxVal);
            this.gpAnimationActionRotate.Controls.Add(this.lblAnimationActionRotateMaxVal);
            this.gpAnimationActionRotate.Controls.Add(this.cbAnimationActionRotateClockwise);
            this.gpAnimationActionRotate.Controls.Add(this.txtAnimationActionRotateClockwise);
            this.gpAnimationActionRotate.Location = new System.Drawing.Point(6, 177);
            this.gpAnimationActionRotate.Name = "gpAnimationActionRotate";
            this.gpAnimationActionRotate.Size = new System.Drawing.Size(454, 155);
            this.gpAnimationActionRotate.TabIndex = 3;
            this.gpAnimationActionRotate.TabStop = false;
            this.gpAnimationActionRotate.Text = "Rotate";
            this.gpAnimationActionRotate.Visible = false;
            // 
            // txtAnimationActionCentrePointY
            // 
            this.txtAnimationActionCentrePointY.Location = new System.Drawing.Point(73, 60);
            this.txtAnimationActionCentrePointY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionCentrePointY.Name = "txtAnimationActionCentrePointY";
            this.txtAnimationActionCentrePointY.Size = new System.Drawing.Size(68, 20);
            this.txtAnimationActionCentrePointY.TabIndex = 11;
            this.txtAnimationActionCentrePointY.ValueChanged += new System.EventHandler(this.AnimationActionRotateCentreY_Changed);
            // 
            // lblAnimationActionCentrePointY
            // 
            this.lblAnimationActionCentrePointY.AutoSize = true;
            this.lblAnimationActionCentrePointY.Location = new System.Drawing.Point(50, 62);
            this.lblAnimationActionCentrePointY.Name = "lblAnimationActionCentrePointY";
            this.lblAnimationActionCentrePointY.Size = new System.Drawing.Size(17, 13);
            this.lblAnimationActionCentrePointY.TabIndex = 10;
            this.lblAnimationActionCentrePointY.Text = "Y:";
            // 
            // txtAnimationActionCentrePointX
            // 
            this.txtAnimationActionCentrePointX.Location = new System.Drawing.Point(73, 34);
            this.txtAnimationActionCentrePointX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionCentrePointX.Name = "txtAnimationActionCentrePointX";
            this.txtAnimationActionCentrePointX.Size = new System.Drawing.Size(68, 20);
            this.txtAnimationActionCentrePointX.TabIndex = 9;
            this.txtAnimationActionCentrePointX.ValueChanged += new System.EventHandler(this.AnimationActionRotateCentreX_Changed);
            // 
            // lblAnimationActionCentrePointX
            // 
            this.lblAnimationActionCentrePointX.AutoSize = true;
            this.lblAnimationActionCentrePointX.Location = new System.Drawing.Point(50, 36);
            this.lblAnimationActionCentrePointX.Name = "lblAnimationActionCentrePointX";
            this.lblAnimationActionCentrePointX.Size = new System.Drawing.Size(17, 13);
            this.lblAnimationActionCentrePointX.TabIndex = 8;
            this.lblAnimationActionCentrePointX.Text = "X:";
            // 
            // lblAnimationActionCentrePoint
            // 
            this.lblAnimationActionCentrePoint.AutoSize = true;
            this.lblAnimationActionCentrePoint.Location = new System.Drawing.Point(13, 16);
            this.lblAnimationActionCentrePoint.Name = "lblAnimationActionCentrePoint";
            this.lblAnimationActionCentrePoint.Size = new System.Drawing.Size(68, 13);
            this.lblAnimationActionCentrePoint.TabIndex = 7;
            this.lblAnimationActionCentrePoint.Text = "Centre Point:";
            // 
            // txtAnimationActionRotateMaxVal
            // 
            this.txtAnimationActionRotateMaxVal.Location = new System.Drawing.Point(69, 125);
            this.txtAnimationActionRotateMaxVal.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.txtAnimationActionRotateMaxVal.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.txtAnimationActionRotateMaxVal.Name = "txtAnimationActionRotateMaxVal";
            this.txtAnimationActionRotateMaxVal.Size = new System.Drawing.Size(69, 20);
            this.txtAnimationActionRotateMaxVal.TabIndex = 3;
            this.txtAnimationActionRotateMaxVal.ValueChanged += new System.EventHandler(this.AnimationActionRotateMaxVal_Changed);
            // 
            // lblAnimationActionRotateMaxVal
            // 
            this.lblAnimationActionRotateMaxVal.AutoSize = true;
            this.lblAnimationActionRotateMaxVal.Location = new System.Drawing.Point(3, 127);
            this.lblAnimationActionRotateMaxVal.Name = "lblAnimationActionRotateMaxVal";
            this.lblAnimationActionRotateMaxVal.Size = new System.Drawing.Size(60, 13);
            this.lblAnimationActionRotateMaxVal.TabIndex = 2;
            this.lblAnimationActionRotateMaxVal.Text = "Max Value:";
            // 
            // cbAnimationActionRotateClockwise
            // 
            this.cbAnimationActionRotateClockwise.AutoSize = true;
            this.cbAnimationActionRotateClockwise.Location = new System.Drawing.Point(70, 102);
            this.cbAnimationActionRotateClockwise.Name = "cbAnimationActionRotateClockwise";
            this.cbAnimationActionRotateClockwise.Size = new System.Drawing.Size(15, 14);
            this.cbAnimationActionRotateClockwise.TabIndex = 1;
            this.cbAnimationActionRotateClockwise.UseVisualStyleBackColor = true;
            this.cbAnimationActionRotateClockwise.CheckedChanged += new System.EventHandler(this.AnimationActionRotateClockwise_Changed);
            // 
            // txtAnimationActionRotateClockwise
            // 
            this.txtAnimationActionRotateClockwise.AutoSize = true;
            this.txtAnimationActionRotateClockwise.Location = new System.Drawing.Point(3, 101);
            this.txtAnimationActionRotateClockwise.Name = "txtAnimationActionRotateClockwise";
            this.txtAnimationActionRotateClockwise.Size = new System.Drawing.Size(64, 13);
            this.txtAnimationActionRotateClockwise.TabIndex = 0;
            this.txtAnimationActionRotateClockwise.Text = "Clockwise?:";
            // 
            // lblAnimationActions
            // 
            this.lblAnimationActions.AutoSize = true;
            this.lblAnimationActions.Location = new System.Drawing.Point(19, 17);
            this.lblAnimationActions.Name = "lblAnimationActions";
            this.lblAnimationActions.Size = new System.Drawing.Size(45, 13);
            this.lblAnimationActions.TabIndex = 2;
            this.lblAnimationActions.Text = "Actions:";
            // 
            // dgAnimationActions
            // 
            this.dgAnimationActions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAnimationActions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActionType});
            this.dgAnimationActions.Location = new System.Drawing.Point(6, 36);
            this.dgAnimationActions.Name = "dgAnimationActions";
            this.dgAnimationActions.Size = new System.Drawing.Size(167, 134);
            this.dgAnimationActions.TabIndex = 1;
            this.dgAnimationActions.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.ActionType_Validated);
            this.dgAnimationActions.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.ActionType_Validate);
            this.dgAnimationActions.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ActionType_Changed);
            this.dgAnimationActions.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataError_Error);
            this.dgAnimationActions.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.AnimateAction_Delete);
            this.dgAnimationActions.SelectionChanged += new System.EventHandler(this.ActionSelect_Change);
            this.dgAnimationActions.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dgAnimationActions.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragOver);
            this.dgAnimationActions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseDown);
            this.dgAnimationActions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseMove);
            // 
            // ActionType
            // 
            this.ActionType.HeaderText = "Type";
            this.ActionType.Name = "ActionType";
            this.ActionType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ActionType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lblAnimationHowNoTrigger
            // 
            this.lblAnimationHowNoTrigger.AutoSize = true;
            this.lblAnimationHowNoTrigger.ForeColor = System.Drawing.Color.Red;
            this.lblAnimationHowNoTrigger.Location = new System.Drawing.Point(102, 20);
            this.lblAnimationHowNoTrigger.Name = "lblAnimationHowNoTrigger";
            this.lblAnimationHowNoTrigger.Size = new System.Drawing.Size(232, 13);
            this.lblAnimationHowNoTrigger.TabIndex = 0;
            this.lblAnimationHowNoTrigger.Text = "You must select a Trigger on the When tab first.";
            // 
            // cmdAnimationSave
            // 
            this.cmdAnimationSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdAnimationSave.Location = new System.Drawing.Point(321, 381);
            this.cmdAnimationSave.Name = "cmdAnimationSave";
            this.cmdAnimationSave.Size = new System.Drawing.Size(75, 23);
            this.cmdAnimationSave.TabIndex = 1;
            this.cmdAnimationSave.Text = "Save";
            this.cmdAnimationSave.UseVisualStyleBackColor = true;
            // 
            // cmdAnimationCancel
            // 
            this.cmdAnimationCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdAnimationCancel.Location = new System.Drawing.Point(403, 382);
            this.cmdAnimationCancel.Name = "cmdAnimationCancel";
            this.cmdAnimationCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdAnimationCancel.TabIndex = 2;
            this.cmdAnimationCancel.Text = "Cancel";
            this.cmdAnimationCancel.UseVisualStyleBackColor = true;
            // 
            // frmAnimation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 416);
            this.Controls.Add(this.cmdAnimationCancel);
            this.Controls.Add(this.cmdAnimationSave);
            this.Controls.Add(this.tabCollection);
            this.Name = "frmAnimation";
            this.Text = "Animation";
            this.tabCollection.ResumeLayout(false);
            this.tabWhat.ResumeLayout(false);
            this.tabWhat.PerformLayout();
            this.gpAnimationDrawing.ResumeLayout(false);
            this.gpAnimationDrawing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAnimationPlotPoints)).EndInit();
            this.gpAnimationImage.ResumeLayout(false);
            this.gpAnimationImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationRelativeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationRelativeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimationImage)).EndInit();
            this.tabWhen.ResumeLayout(false);
            this.gpAnimationClientRequest.ResumeLayout(false);
            this.gpAnimationClientRequest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdAnimationTriggers)).EndInit();
            this.tabHow.ResumeLayout(false);
            this.tabHow.PerformLayout();
            this.gpAnimationActionMoveX.ResumeLayout(false);
            this.gpAnimationActionMoveX.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionMoveXValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionMoveXMax)).EndInit();
            this.gpAnimationActionClip.ResumeLayout(false);
            this.gpAnimationActionClip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionEndY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionEndX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionStartY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionStartX)).EndInit();
            this.gpAnimationActionRotate.ResumeLayout(false);
            this.gpAnimationActionRotate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionCentrePointY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionCentrePointX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionRotateMaxVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgAnimationActions)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TabControl tabCollection;
        private System.Windows.Forms.TabPage tabWhat;
        private System.Windows.Forms.TabPage tabWhen;
        private System.Windows.Forms.TextBox txtAnimationName;
        private System.Windows.Forms.Label lblAnimationName;
        private System.Windows.Forms.ComboBox cmbAnimationType;
        private System.Windows.Forms.Label lblAnimationType;
        private System.Windows.Forms.GroupBox gpAnimationDrawing;
        private System.Windows.Forms.GroupBox gpAnimationImage;
        private System.Windows.Forms.TextBox txtAnimationImagePath;
        private System.Windows.Forms.Label lblAnimationImagePath;
        private System.Windows.Forms.PictureBox pbAnimationImage;
        private System.Windows.Forms.Label lblAnimationRelativeY;
        private System.Windows.Forms.Label lblAnimationRelativeX;
        private System.Windows.Forms.ComboBox cmbAnimationFillColor;
        private System.Windows.Forms.Label lblAnimationFillColor;
        private System.Windows.Forms.ComboBox cmbAnimationFillMethod;
        private System.Windows.Forms.Label lblAnimationFillMethod;
        private System.Windows.Forms.Button cmdAnimationSave;
        private System.Windows.Forms.Button cmdAnimationCancel;
        private System.Windows.Forms.NumericUpDown txtAnimationRelativeY;
        private System.Windows.Forms.NumericUpDown txtAnimationRelativeX;
        private System.Windows.Forms.Label lblAnimationTypeWarning;
        private System.Windows.Forms.Button cmdLoadAnimationImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label lblAnimationPointMap;
        private System.Windows.Forms.DataGridView dgAnimationPlotPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn pointX;
        private System.Windows.Forms.DataGridViewTextBoxColumn pointY;
        private System.Windows.Forms.DataGridView gdAnimationTriggers;
        private System.Windows.Forms.TabPage tabHow;
        private System.Windows.Forms.Label lblAnimationHowNoTrigger;
        private System.Windows.Forms.DataGridViewTextBoxColumn Trigger;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.GroupBox gpAnimationClientRequest;
        private System.Windows.Forms.ComboBox cmbAnimationVariableNames;
        private System.Windows.Forms.Label lblAnimationClientRequestName;
        private System.Windows.Forms.Label lblAnimationClientRequestUnits;
        private System.Windows.Forms.TextBox txtAnimationClientRequestUnits;
        private System.Windows.Forms.CheckBox cbAnimationClientRequestUnitsOverride;
        private DataGridView dgAnimationActions;
        private Label lblAnimationActions;
        private DataGridViewComboBoxColumn ActionType;
        private GroupBox gpAnimationActionClip;
        private GroupBox gpAnimationActionRotate;
        private ComboBox cmbAnimationActionStyle;
        private Label lblAnimationActionStyle;
        private NumericUpDown txtAnimationActionEndY;
        private Label lblAnimationActionEndY;
        private NumericUpDown txtAnimationActionEndX;
        private Label lblAnimationActionEndX;
        private Label lblAnimationActionEndPoint;
        private NumericUpDown txtAnimationActionStartY;
        private Label lblAnimationActionStartY;
        private NumericUpDown txtAnimationActionStartX;
        private Label lblAnimationActionStartX;
        private Label lblAnimationActionStartPoint;
        private Label txtAnimationActionRotateClockwise;
        private CheckBox cbAnimationActionRotateClockwise;
        private NumericUpDown txtAnimationActionCentrePointY;
        private Label lblAnimationActionCentrePointY;
        private NumericUpDown txtAnimationActionCentrePointX;
        private Label lblAnimationActionCentrePointX;
        private Label lblAnimationActionCentrePoint;
        private NumericUpDown txtAnimationActionRotateMaxVal;
        private Label lblAnimationActionRotateMaxVal;
        private GroupBox gpAnimationActionMoveX;
        private NumericUpDown txtAnimationActionMoveXValue;
        private Label lblAnimationActionMoveXPercent;
        private NumericUpDown txtAnimationActionMoveXMax;
        private Label lblAnimationActionMoveXMax;
    }
}