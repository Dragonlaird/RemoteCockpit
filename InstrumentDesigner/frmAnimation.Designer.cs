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
            this.lblAnimationActionMoveXPercent = new System.Windows.Forms.Label();
            this.txtAnimationActionMoveMax = new System.Windows.Forms.NumericUpDown();
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
            this.cbAnimationActionMoveInvert = new System.Windows.Forms.CheckBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionMoveMax)).BeginInit();
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
            this.tabCollection.Location = new System.Drawing.Point(18, 18);
            this.tabCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabCollection.Name = "tabCollection";
            this.tabCollection.SelectedIndex = 0;
            this.tabCollection.Size = new System.Drawing.Size(711, 560);
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
            this.tabWhat.Location = new System.Drawing.Point(4, 29);
            this.tabWhat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabWhat.Name = "tabWhat";
            this.tabWhat.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabWhat.Size = new System.Drawing.Size(703, 527);
            this.tabWhat.TabIndex = 0;
            this.tabWhat.Text = "What";
            this.tabWhat.UseVisualStyleBackColor = true;
            // 
            // lblAnimationTypeWarning
            // 
            this.lblAnimationTypeWarning.AutoSize = true;
            this.lblAnimationTypeWarning.ForeColor = System.Drawing.Color.Red;
            this.lblAnimationTypeWarning.Location = new System.Drawing.Point(384, 55);
            this.lblAnimationTypeWarning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationTypeWarning.Name = "lblAnimationTypeWarning";
            this.lblAnimationTypeWarning.Size = new System.Drawing.Size(305, 20);
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
            this.gpAnimationDrawing.Location = new System.Drawing.Point(9, 151);
            this.gpAnimationDrawing.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationDrawing.Name = "gpAnimationDrawing";
            this.gpAnimationDrawing.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationDrawing.Size = new System.Drawing.Size(663, 360);
            this.gpAnimationDrawing.TabIndex = 7;
            this.gpAnimationDrawing.TabStop = false;
            this.gpAnimationDrawing.Text = "Drawing";
            this.gpAnimationDrawing.Visible = false;
            // 
            // lblAnimationPointMap
            // 
            this.lblAnimationPointMap.AutoSize = true;
            this.lblAnimationPointMap.Location = new System.Drawing.Point(387, 26);
            this.lblAnimationPointMap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationPointMap.Name = "lblAnimationPointMap";
            this.lblAnimationPointMap.Size = new System.Drawing.Size(160, 20);
            this.lblAnimationPointMap.TabIndex = 12;
            this.lblAnimationPointMap.Text = "Cartesian Plot Points:";
            // 
            // dgAnimationPlotPoints
            // 
            this.dgAnimationPlotPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAnimationPlotPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pointX,
            this.pointY});
            this.dgAnimationPlotPoints.Location = new System.Drawing.Point(387, 51);
            this.dgAnimationPlotPoints.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgAnimationPlotPoints.Name = "dgAnimationPlotPoints";
            this.dgAnimationPlotPoints.RowHeadersWidth = 62;
            this.dgAnimationPlotPoints.Size = new System.Drawing.Size(267, 300);
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
            this.cmbAnimationFillMethod.Location = new System.Drawing.Point(104, 100);
            this.cmbAnimationFillMethod.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAnimationFillMethod.Name = "cmbAnimationFillMethod";
            this.cmbAnimationFillMethod.Size = new System.Drawing.Size(180, 28);
            this.cmbAnimationFillMethod.TabIndex = 10;
            // 
            // lblAnimationFillMethod
            // 
            this.lblAnimationFillMethod.AutoSize = true;
            this.lblAnimationFillMethod.Location = new System.Drawing.Point(3, 105);
            this.lblAnimationFillMethod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationFillMethod.Name = "lblAnimationFillMethod";
            this.lblAnimationFillMethod.Size = new System.Drawing.Size(90, 20);
            this.lblAnimationFillMethod.TabIndex = 9;
            this.lblAnimationFillMethod.Text = "Fill Method:";
            // 
            // cmbAnimationFillColor
            // 
            this.cmbAnimationFillColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationFillColor.FormattingEnabled = true;
            this.cmbAnimationFillColor.Location = new System.Drawing.Point(104, 51);
            this.cmbAnimationFillColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAnimationFillColor.Name = "cmbAnimationFillColor";
            this.cmbAnimationFillColor.Size = new System.Drawing.Size(180, 28);
            this.cmbAnimationFillColor.TabIndex = 8;
            this.cmbAnimationFillColor.SelectedIndexChanged += new System.EventHandler(this.FillColor_Change);
            // 
            // lblAnimationFillColor
            // 
            this.lblAnimationFillColor.AutoSize = true;
            this.lblAnimationFillColor.Location = new System.Drawing.Point(21, 55);
            this.lblAnimationFillColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationFillColor.Name = "lblAnimationFillColor";
            this.lblAnimationFillColor.Size = new System.Drawing.Size(73, 20);
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
            this.gpAnimationImage.Location = new System.Drawing.Point(9, 151);
            this.gpAnimationImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationImage.Name = "gpAnimationImage";
            this.gpAnimationImage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationImage.Size = new System.Drawing.Size(663, 360);
            this.gpAnimationImage.TabIndex = 6;
            this.gpAnimationImage.TabStop = false;
            this.gpAnimationImage.Text = "Image";
            // 
            // cmdLoadAnimationImage
            // 
            this.cmdLoadAnimationImage.Location = new System.Drawing.Point(372, 109);
            this.cmdLoadAnimationImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdLoadAnimationImage.Name = "cmdLoadAnimationImage";
            this.cmdLoadAnimationImage.Size = new System.Drawing.Size(112, 35);
            this.cmdLoadAnimationImage.TabIndex = 24;
            this.cmdLoadAnimationImage.Text = "...";
            this.cmdLoadAnimationImage.UseVisualStyleBackColor = true;
            this.cmdLoadAnimationImage.Click += new System.EventHandler(this.LoadAnimationImage_Click);
            // 
            // txtAnimationRelativeY
            // 
            this.txtAnimationRelativeY.Location = new System.Drawing.Point(84, 66);
            this.txtAnimationRelativeY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationRelativeY.Name = "txtAnimationRelativeY";
            this.txtAnimationRelativeY.Size = new System.Drawing.Size(180, 26);
            this.txtAnimationRelativeY.TabIndex = 23;
            // 
            // txtAnimationRelativeX
            // 
            this.txtAnimationRelativeX.Location = new System.Drawing.Point(84, 31);
            this.txtAnimationRelativeX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationRelativeX.Name = "txtAnimationRelativeX";
            this.txtAnimationRelativeX.Size = new System.Drawing.Size(180, 26);
            this.txtAnimationRelativeX.TabIndex = 22;
            // 
            // lblAnimationRelativeY
            // 
            this.lblAnimationRelativeY.AutoSize = true;
            this.lblAnimationRelativeY.Location = new System.Drawing.Point(2, 69);
            this.lblAnimationRelativeY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationRelativeY.Name = "lblAnimationRelativeY";
            this.lblAnimationRelativeY.Size = new System.Drawing.Size(72, 20);
            this.lblAnimationRelativeY.TabIndex = 20;
            this.lblAnimationRelativeY.Text = "Offset Y:";
            // 
            // lblAnimationRelativeX
            // 
            this.lblAnimationRelativeX.AutoSize = true;
            this.lblAnimationRelativeX.Location = new System.Drawing.Point(2, 34);
            this.lblAnimationRelativeX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationRelativeX.Name = "lblAnimationRelativeX";
            this.lblAnimationRelativeX.Size = new System.Drawing.Size(72, 20);
            this.lblAnimationRelativeX.TabIndex = 18;
            this.lblAnimationRelativeX.Text = "Offset X:";
            // 
            // pbAnimationImage
            // 
            this.pbAnimationImage.Location = new System.Drawing.Point(84, 149);
            this.pbAnimationImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbAnimationImage.Name = "pbAnimationImage";
            this.pbAnimationImage.Size = new System.Drawing.Size(249, 194);
            this.pbAnimationImage.TabIndex = 10;
            this.pbAnimationImage.TabStop = false;
            // 
            // txtAnimationImagePath
            // 
            this.txtAnimationImagePath.Enabled = false;
            this.txtAnimationImagePath.Location = new System.Drawing.Point(82, 109);
            this.txtAnimationImagePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationImagePath.Name = "txtAnimationImagePath";
            this.txtAnimationImagePath.Size = new System.Drawing.Size(277, 26);
            this.txtAnimationImagePath.TabIndex = 7;
            // 
            // lblAnimationImagePath
            // 
            this.lblAnimationImagePath.AutoSize = true;
            this.lblAnimationImagePath.Location = new System.Drawing.Point(15, 117);
            this.lblAnimationImagePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationImagePath.Name = "lblAnimationImagePath";
            this.lblAnimationImagePath.Size = new System.Drawing.Size(58, 20);
            this.lblAnimationImagePath.TabIndex = 6;
            this.lblAnimationImagePath.Text = "Image:";
            // 
            // cmbAnimationType
            // 
            this.cmbAnimationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationType.FormattingEnabled = true;
            this.cmbAnimationType.Location = new System.Drawing.Point(104, 49);
            this.cmbAnimationType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAnimationType.Name = "cmbAnimationType";
            this.cmbAnimationType.Size = new System.Drawing.Size(276, 28);
            this.cmbAnimationType.TabIndex = 3;
            this.cmbAnimationType.SelectedIndexChanged += new System.EventHandler(this.ChangeAnimationType_Select);
            // 
            // lblAnimationType
            // 
            this.lblAnimationType.AutoSize = true;
            this.lblAnimationType.Location = new System.Drawing.Point(42, 55);
            this.lblAnimationType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationType.Name = "lblAnimationType";
            this.lblAnimationType.Size = new System.Drawing.Size(47, 20);
            this.lblAnimationType.TabIndex = 2;
            this.lblAnimationType.Text = "Type:";
            // 
            // txtAnimationName
            // 
            this.txtAnimationName.Location = new System.Drawing.Point(104, 11);
            this.txtAnimationName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationName.Name = "txtAnimationName";
            this.txtAnimationName.Size = new System.Drawing.Size(276, 26);
            this.txtAnimationName.TabIndex = 1;
            this.txtAnimationName.TextChanged += new System.EventHandler(this.UpdateName_Changed);
            // 
            // lblAnimationName
            // 
            this.lblAnimationName.AutoSize = true;
            this.lblAnimationName.Location = new System.Drawing.Point(36, 15);
            this.lblAnimationName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationName.Name = "lblAnimationName";
            this.lblAnimationName.Size = new System.Drawing.Size(55, 20);
            this.lblAnimationName.TabIndex = 0;
            this.lblAnimationName.Text = "Name:";
            // 
            // tabWhen
            // 
            this.tabWhen.Controls.Add(this.gpAnimationClientRequest);
            this.tabWhen.Controls.Add(this.gdAnimationTriggers);
            this.tabWhen.Location = new System.Drawing.Point(4, 29);
            this.tabWhen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabWhen.Name = "tabWhen";
            this.tabWhen.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabWhen.Size = new System.Drawing.Size(703, 527);
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
            this.gpAnimationClientRequest.Location = new System.Drawing.Point(10, 246);
            this.gpAnimationClientRequest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationClientRequest.Name = "gpAnimationClientRequest";
            this.gpAnimationClientRequest.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationClientRequest.Size = new System.Drawing.Size(680, 265);
            this.gpAnimationClientRequest.TabIndex = 2;
            this.gpAnimationClientRequest.TabStop = false;
            this.gpAnimationClientRequest.Text = "Client Request";
            this.gpAnimationClientRequest.Visible = false;
            // 
            // cbAnimationClientRequestUnitsOverride
            // 
            this.cbAnimationClientRequestUnitsOverride.AutoSize = true;
            this.cbAnimationClientRequestUnitsOverride.Location = new System.Drawing.Point(375, 74);
            this.cbAnimationClientRequestUnitsOverride.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbAnimationClientRequestUnitsOverride.Name = "cbAnimationClientRequestUnitsOverride";
            this.cbAnimationClientRequestUnitsOverride.Size = new System.Drawing.Size(103, 24);
            this.cbAnimationClientRequestUnitsOverride.TabIndex = 4;
            this.cbAnimationClientRequestUnitsOverride.Text = "Override?";
            this.cbAnimationClientRequestUnitsOverride.UseVisualStyleBackColor = true;
            this.cbAnimationClientRequestUnitsOverride.Visible = false;
            this.cbAnimationClientRequestUnitsOverride.CheckedChanged += new System.EventHandler(this.OverrideUnits_Change);
            // 
            // txtAnimationClientRequestUnits
            // 
            this.txtAnimationClientRequestUnits.Enabled = false;
            this.txtAnimationClientRequestUnits.Location = new System.Drawing.Point(138, 69);
            this.txtAnimationClientRequestUnits.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationClientRequestUnits.Name = "txtAnimationClientRequestUnits";
            this.txtAnimationClientRequestUnits.Size = new System.Drawing.Size(224, 26);
            this.txtAnimationClientRequestUnits.TabIndex = 3;
            // 
            // lblAnimationClientRequestUnits
            // 
            this.lblAnimationClientRequestUnits.AutoSize = true;
            this.lblAnimationClientRequestUnits.Location = new System.Drawing.Point(78, 74);
            this.lblAnimationClientRequestUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationClientRequestUnits.Name = "lblAnimationClientRequestUnits";
            this.lblAnimationClientRequestUnits.Size = new System.Drawing.Size(50, 20);
            this.lblAnimationClientRequestUnits.TabIndex = 2;
            this.lblAnimationClientRequestUnits.Text = "Units:";
            // 
            // cmbAnimationVariableNames
            // 
            this.cmbAnimationVariableNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationVariableNames.FormattingEnabled = true;
            this.cmbAnimationVariableNames.Location = new System.Drawing.Point(138, 26);
            this.cmbAnimationVariableNames.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAnimationVariableNames.Name = "cmbAnimationVariableNames";
            this.cmbAnimationVariableNames.Size = new System.Drawing.Size(390, 28);
            this.cmbAnimationVariableNames.TabIndex = 1;
            this.cmbAnimationVariableNames.SelectedIndexChanged += new System.EventHandler(this.VariableName_Change);
            // 
            // lblAnimationClientRequestName
            // 
            this.lblAnimationClientRequestName.AutoSize = true;
            this.lblAnimationClientRequestName.Location = new System.Drawing.Point(10, 31);
            this.lblAnimationClientRequestName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationClientRequestName.Name = "lblAnimationClientRequestName";
            this.lblAnimationClientRequestName.Size = new System.Drawing.Size(117, 20);
            this.lblAnimationClientRequestName.TabIndex = 0;
            this.lblAnimationClientRequestName.Text = "Variable Name:";
            // 
            // gdAnimationTriggers
            // 
            this.gdAnimationTriggers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdAnimationTriggers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Trigger,
            this.Type});
            this.gdAnimationTriggers.Location = new System.Drawing.Point(9, 5);
            this.gdAnimationTriggers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gdAnimationTriggers.Name = "gdAnimationTriggers";
            this.gdAnimationTriggers.RowHeadersWidth = 62;
            this.gdAnimationTriggers.Size = new System.Drawing.Size(681, 231);
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
            this.tabHow.Location = new System.Drawing.Point(4, 29);
            this.tabHow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabHow.Name = "tabHow";
            this.tabHow.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabHow.Size = new System.Drawing.Size(703, 527);
            this.tabHow.TabIndex = 2;
            this.tabHow.Text = "How";
            this.tabHow.UseVisualStyleBackColor = true;
            // 
            // gpAnimationActionMoveX
            // 
            this.gpAnimationActionMoveX.Controls.Add(this.cbAnimationActionMoveInvert);
            this.gpAnimationActionMoveX.Controls.Add(this.lblAnimationActionMoveXPercent);
            this.gpAnimationActionMoveX.Controls.Add(this.txtAnimationActionMoveMax);
            this.gpAnimationActionMoveX.Controls.Add(this.lblAnimationActionMoveXMax);
            this.gpAnimationActionMoveX.Location = new System.Drawing.Point(9, 272);
            this.gpAnimationActionMoveX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationActionMoveX.Name = "gpAnimationActionMoveX";
            this.gpAnimationActionMoveX.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationActionMoveX.Size = new System.Drawing.Size(681, 238);
            this.gpAnimationActionMoveX.TabIndex = 5;
            this.gpAnimationActionMoveX.TabStop = false;
            this.gpAnimationActionMoveX.Text = "MoveX";
            // 
            // lblAnimationActionMoveXPercent
            // 
            this.lblAnimationActionMoveXPercent.AutoSize = true;
            this.lblAnimationActionMoveXPercent.Location = new System.Drawing.Point(30, 65);
            this.lblAnimationActionMoveXPercent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionMoveXPercent.Name = "lblAnimationActionMoveXPercent";
            this.lblAnimationActionMoveXPercent.Size = new System.Drawing.Size(53, 20);
            this.lblAnimationActionMoveXPercent.TabIndex = 2;
            this.lblAnimationActionMoveXPercent.Text = "Invert:";
            // 
            // txtAnimationActionMoveMax
            // 
            this.txtAnimationActionMoveMax.Location = new System.Drawing.Point(110, 28);
            this.txtAnimationActionMoveMax.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationActionMoveMax.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtAnimationActionMoveMax.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.txtAnimationActionMoveMax.Name = "txtAnimationActionMoveMax";
            this.txtAnimationActionMoveMax.Size = new System.Drawing.Size(180, 26);
            this.txtAnimationActionMoveMax.TabIndex = 1;
            this.txtAnimationActionMoveMax.ValueChanged += new System.EventHandler(this.AnimationActionMoveMax_Changed);
            // 
            // lblAnimationActionMoveXMax
            // 
            this.lblAnimationActionMoveXMax.AutoSize = true;
            this.lblAnimationActionMoveXMax.Location = new System.Drawing.Point(10, 31);
            this.lblAnimationActionMoveXMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionMoveXMax.Name = "lblAnimationActionMoveXMax";
            this.lblAnimationActionMoveXMax.Size = new System.Drawing.Size(87, 20);
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
            this.gpAnimationActionClip.Location = new System.Drawing.Point(9, 271);
            this.gpAnimationActionClip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationActionClip.Name = "gpAnimationActionClip";
            this.gpAnimationActionClip.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationActionClip.Size = new System.Drawing.Size(681, 238);
            this.gpAnimationActionClip.TabIndex = 4;
            this.gpAnimationActionClip.TabStop = false;
            this.gpAnimationActionClip.Text = "Clip";
            this.gpAnimationActionClip.Visible = false;
            // 
            // txtAnimationActionEndY
            // 
            this.txtAnimationActionEndY.Location = new System.Drawing.Point(375, 140);
            this.txtAnimationActionEndY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationActionEndY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionEndY.Name = "txtAnimationActionEndY";
            this.txtAnimationActionEndY.Size = new System.Drawing.Size(102, 26);
            this.txtAnimationActionEndY.TabIndex = 11;
            this.txtAnimationActionEndY.ValueChanged += new System.EventHandler(this.AnimationActionClipEndY_Changed);
            // 
            // lblAnimationActionEndY
            // 
            this.lblAnimationActionEndY.AutoSize = true;
            this.lblAnimationActionEndY.Location = new System.Drawing.Point(340, 143);
            this.lblAnimationActionEndY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionEndY.Name = "lblAnimationActionEndY";
            this.lblAnimationActionEndY.Size = new System.Drawing.Size(24, 20);
            this.lblAnimationActionEndY.TabIndex = 10;
            this.lblAnimationActionEndY.Text = "Y:";
            // 
            // txtAnimationActionEndX
            // 
            this.txtAnimationActionEndX.Location = new System.Drawing.Point(375, 100);
            this.txtAnimationActionEndX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationActionEndX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionEndX.Name = "txtAnimationActionEndX";
            this.txtAnimationActionEndX.Size = new System.Drawing.Size(102, 26);
            this.txtAnimationActionEndX.TabIndex = 9;
            this.txtAnimationActionEndX.ValueChanged += new System.EventHandler(this.AnimationActionClipEndX_Changed);
            // 
            // lblAnimationActionEndX
            // 
            this.lblAnimationActionEndX.AutoSize = true;
            this.lblAnimationActionEndX.Location = new System.Drawing.Point(340, 103);
            this.lblAnimationActionEndX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionEndX.Name = "lblAnimationActionEndX";
            this.lblAnimationActionEndX.Size = new System.Drawing.Size(24, 20);
            this.lblAnimationActionEndX.TabIndex = 8;
            this.lblAnimationActionEndX.Text = "X:";
            // 
            // lblAnimationActionEndPoint
            // 
            this.lblAnimationActionEndPoint.AutoSize = true;
            this.lblAnimationActionEndPoint.Location = new System.Drawing.Point(285, 72);
            this.lblAnimationActionEndPoint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionEndPoint.Name = "lblAnimationActionEndPoint";
            this.lblAnimationActionEndPoint.Size = new System.Drawing.Size(82, 20);
            this.lblAnimationActionEndPoint.TabIndex = 7;
            this.lblAnimationActionEndPoint.Text = "End Point:";
            // 
            // txtAnimationActionStartY
            // 
            this.txtAnimationActionStartY.Location = new System.Drawing.Point(110, 140);
            this.txtAnimationActionStartY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationActionStartY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionStartY.Name = "txtAnimationActionStartY";
            this.txtAnimationActionStartY.Size = new System.Drawing.Size(102, 26);
            this.txtAnimationActionStartY.TabIndex = 6;
            this.txtAnimationActionStartY.ValueChanged += new System.EventHandler(this.AnimationActionClipStartY_Changed);
            // 
            // lblAnimationActionStartY
            // 
            this.lblAnimationActionStartY.AutoSize = true;
            this.lblAnimationActionStartY.Location = new System.Drawing.Point(75, 143);
            this.lblAnimationActionStartY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionStartY.Name = "lblAnimationActionStartY";
            this.lblAnimationActionStartY.Size = new System.Drawing.Size(24, 20);
            this.lblAnimationActionStartY.TabIndex = 5;
            this.lblAnimationActionStartY.Text = "Y:";
            // 
            // txtAnimationActionStartX
            // 
            this.txtAnimationActionStartX.Location = new System.Drawing.Point(110, 100);
            this.txtAnimationActionStartX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationActionStartX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionStartX.Name = "txtAnimationActionStartX";
            this.txtAnimationActionStartX.Size = new System.Drawing.Size(102, 26);
            this.txtAnimationActionStartX.TabIndex = 4;
            this.txtAnimationActionStartX.ValueChanged += new System.EventHandler(this.AnimationActionClipStartX_Changed);
            // 
            // lblAnimationActionStartX
            // 
            this.lblAnimationActionStartX.AutoSize = true;
            this.lblAnimationActionStartX.Location = new System.Drawing.Point(75, 103);
            this.lblAnimationActionStartX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionStartX.Name = "lblAnimationActionStartX";
            this.lblAnimationActionStartX.Size = new System.Drawing.Size(24, 20);
            this.lblAnimationActionStartX.TabIndex = 3;
            this.lblAnimationActionStartX.Text = "X:";
            // 
            // lblAnimationActionStartPoint
            // 
            this.lblAnimationActionStartPoint.AutoSize = true;
            this.lblAnimationActionStartPoint.Location = new System.Drawing.Point(20, 72);
            this.lblAnimationActionStartPoint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionStartPoint.Name = "lblAnimationActionStartPoint";
            this.lblAnimationActionStartPoint.Size = new System.Drawing.Size(88, 20);
            this.lblAnimationActionStartPoint.TabIndex = 2;
            this.lblAnimationActionStartPoint.Text = "Start Point:";
            // 
            // cmbAnimationActionStyle
            // 
            this.cmbAnimationActionStyle.FormattingEnabled = true;
            this.cmbAnimationActionStyle.Location = new System.Drawing.Point(82, 26);
            this.cmbAnimationActionStyle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAnimationActionStyle.Name = "cmbAnimationActionStyle";
            this.cmbAnimationActionStyle.Size = new System.Drawing.Size(180, 28);
            this.cmbAnimationActionStyle.TabIndex = 1;
            this.cmbAnimationActionStyle.SelectedIndexChanged += new System.EventHandler(this.AnimationClipStyle_Changed);
            // 
            // lblAnimationActionStyle
            // 
            this.lblAnimationActionStyle.AutoSize = true;
            this.lblAnimationActionStyle.Location = new System.Drawing.Point(24, 31);
            this.lblAnimationActionStyle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionStyle.Name = "lblAnimationActionStyle";
            this.lblAnimationActionStyle.Size = new System.Drawing.Size(48, 20);
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
            this.gpAnimationActionRotate.Location = new System.Drawing.Point(9, 272);
            this.gpAnimationActionRotate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationActionRotate.Name = "gpAnimationActionRotate";
            this.gpAnimationActionRotate.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAnimationActionRotate.Size = new System.Drawing.Size(681, 238);
            this.gpAnimationActionRotate.TabIndex = 3;
            this.gpAnimationActionRotate.TabStop = false;
            this.gpAnimationActionRotate.Text = "Rotate";
            this.gpAnimationActionRotate.Visible = false;
            // 
            // txtAnimationActionCentrePointY
            // 
            this.txtAnimationActionCentrePointY.Location = new System.Drawing.Point(110, 92);
            this.txtAnimationActionCentrePointY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationActionCentrePointY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionCentrePointY.Name = "txtAnimationActionCentrePointY";
            this.txtAnimationActionCentrePointY.Size = new System.Drawing.Size(102, 26);
            this.txtAnimationActionCentrePointY.TabIndex = 11;
            this.txtAnimationActionCentrePointY.ValueChanged += new System.EventHandler(this.AnimationActionRotateCentreY_Changed);
            // 
            // lblAnimationActionCentrePointY
            // 
            this.lblAnimationActionCentrePointY.AutoSize = true;
            this.lblAnimationActionCentrePointY.Location = new System.Drawing.Point(75, 95);
            this.lblAnimationActionCentrePointY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionCentrePointY.Name = "lblAnimationActionCentrePointY";
            this.lblAnimationActionCentrePointY.Size = new System.Drawing.Size(24, 20);
            this.lblAnimationActionCentrePointY.TabIndex = 10;
            this.lblAnimationActionCentrePointY.Text = "Y:";
            // 
            // txtAnimationActionCentrePointX
            // 
            this.txtAnimationActionCentrePointX.Location = new System.Drawing.Point(110, 52);
            this.txtAnimationActionCentrePointX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAnimationActionCentrePointX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtAnimationActionCentrePointX.Name = "txtAnimationActionCentrePointX";
            this.txtAnimationActionCentrePointX.Size = new System.Drawing.Size(102, 26);
            this.txtAnimationActionCentrePointX.TabIndex = 9;
            this.txtAnimationActionCentrePointX.ValueChanged += new System.EventHandler(this.AnimationActionRotateCentreX_Changed);
            // 
            // lblAnimationActionCentrePointX
            // 
            this.lblAnimationActionCentrePointX.AutoSize = true;
            this.lblAnimationActionCentrePointX.Location = new System.Drawing.Point(75, 55);
            this.lblAnimationActionCentrePointX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionCentrePointX.Name = "lblAnimationActionCentrePointX";
            this.lblAnimationActionCentrePointX.Size = new System.Drawing.Size(24, 20);
            this.lblAnimationActionCentrePointX.TabIndex = 8;
            this.lblAnimationActionCentrePointX.Text = "X:";
            // 
            // lblAnimationActionCentrePoint
            // 
            this.lblAnimationActionCentrePoint.AutoSize = true;
            this.lblAnimationActionCentrePoint.Location = new System.Drawing.Point(20, 25);
            this.lblAnimationActionCentrePoint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionCentrePoint.Name = "lblAnimationActionCentrePoint";
            this.lblAnimationActionCentrePoint.Size = new System.Drawing.Size(101, 20);
            this.lblAnimationActionCentrePoint.TabIndex = 7;
            this.lblAnimationActionCentrePoint.Text = "Centre Point:";
            // 
            // txtAnimationActionRotateMaxVal
            // 
            this.txtAnimationActionRotateMaxVal.Location = new System.Drawing.Point(104, 192);
            this.txtAnimationActionRotateMaxVal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.txtAnimationActionRotateMaxVal.Size = new System.Drawing.Size(104, 26);
            this.txtAnimationActionRotateMaxVal.TabIndex = 3;
            this.txtAnimationActionRotateMaxVal.ValueChanged += new System.EventHandler(this.AnimationActionRotateMaxVal_Changed);
            // 
            // lblAnimationActionRotateMaxVal
            // 
            this.lblAnimationActionRotateMaxVal.AutoSize = true;
            this.lblAnimationActionRotateMaxVal.Location = new System.Drawing.Point(4, 195);
            this.lblAnimationActionRotateMaxVal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActionRotateMaxVal.Name = "lblAnimationActionRotateMaxVal";
            this.lblAnimationActionRotateMaxVal.Size = new System.Drawing.Size(87, 20);
            this.lblAnimationActionRotateMaxVal.TabIndex = 2;
            this.lblAnimationActionRotateMaxVal.Text = "Max Value:";
            // 
            // cbAnimationActionRotateClockwise
            // 
            this.cbAnimationActionRotateClockwise.AutoSize = true;
            this.cbAnimationActionRotateClockwise.Location = new System.Drawing.Point(105, 157);
            this.cbAnimationActionRotateClockwise.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbAnimationActionRotateClockwise.Name = "cbAnimationActionRotateClockwise";
            this.cbAnimationActionRotateClockwise.Size = new System.Drawing.Size(22, 21);
            this.cbAnimationActionRotateClockwise.TabIndex = 1;
            this.cbAnimationActionRotateClockwise.UseVisualStyleBackColor = true;
            this.cbAnimationActionRotateClockwise.CheckedChanged += new System.EventHandler(this.AnimationActionRotateClockwise_Changed);
            // 
            // txtAnimationActionRotateClockwise
            // 
            this.txtAnimationActionRotateClockwise.AutoSize = true;
            this.txtAnimationActionRotateClockwise.Location = new System.Drawing.Point(4, 155);
            this.txtAnimationActionRotateClockwise.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtAnimationActionRotateClockwise.Name = "txtAnimationActionRotateClockwise";
            this.txtAnimationActionRotateClockwise.Size = new System.Drawing.Size(92, 20);
            this.txtAnimationActionRotateClockwise.TabIndex = 0;
            this.txtAnimationActionRotateClockwise.Text = "Clockwise?:";
            // 
            // lblAnimationActions
            // 
            this.lblAnimationActions.AutoSize = true;
            this.lblAnimationActions.Location = new System.Drawing.Point(28, 26);
            this.lblAnimationActions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationActions.Name = "lblAnimationActions";
            this.lblAnimationActions.Size = new System.Drawing.Size(66, 20);
            this.lblAnimationActions.TabIndex = 2;
            this.lblAnimationActions.Text = "Actions:";
            // 
            // dgAnimationActions
            // 
            this.dgAnimationActions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAnimationActions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActionType});
            this.dgAnimationActions.Location = new System.Drawing.Point(9, 55);
            this.dgAnimationActions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgAnimationActions.Name = "dgAnimationActions";
            this.dgAnimationActions.RowHeadersWidth = 62;
            this.dgAnimationActions.Size = new System.Drawing.Size(250, 206);
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
            this.ActionType.MinimumWidth = 8;
            this.ActionType.Name = "ActionType";
            this.ActionType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ActionType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ActionType.Width = 150;
            // 
            // lblAnimationHowNoTrigger
            // 
            this.lblAnimationHowNoTrigger.AutoSize = true;
            this.lblAnimationHowNoTrigger.ForeColor = System.Drawing.Color.Red;
            this.lblAnimationHowNoTrigger.Location = new System.Drawing.Point(153, 31);
            this.lblAnimationHowNoTrigger.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnimationHowNoTrigger.Name = "lblAnimationHowNoTrigger";
            this.lblAnimationHowNoTrigger.Size = new System.Drawing.Size(345, 20);
            this.lblAnimationHowNoTrigger.TabIndex = 0;
            this.lblAnimationHowNoTrigger.Text = "You must select a Trigger on the When tab first.";
            // 
            // cmdAnimationSave
            // 
            this.cmdAnimationSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdAnimationSave.Location = new System.Drawing.Point(482, 586);
            this.cmdAnimationSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdAnimationSave.Name = "cmdAnimationSave";
            this.cmdAnimationSave.Size = new System.Drawing.Size(112, 35);
            this.cmdAnimationSave.TabIndex = 1;
            this.cmdAnimationSave.Text = "Save";
            this.cmdAnimationSave.UseVisualStyleBackColor = true;
            // 
            // cmdAnimationCancel
            // 
            this.cmdAnimationCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdAnimationCancel.Location = new System.Drawing.Point(604, 588);
            this.cmdAnimationCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdAnimationCancel.Name = "cmdAnimationCancel";
            this.cmdAnimationCancel.Size = new System.Drawing.Size(112, 35);
            this.cmdAnimationCancel.TabIndex = 2;
            this.cmdAnimationCancel.Text = "Cancel";
            this.cmdAnimationCancel.UseVisualStyleBackColor = true;
            // 
            // cbAnimationActionMoveInvert
            // 
            this.cbAnimationActionMoveInvert.AutoSize = true;
            this.cbAnimationActionMoveInvert.Location = new System.Drawing.Point(110, 62);
            this.cbAnimationActionMoveInvert.Name = "cbAnimationActionMoveInvert";
            this.cbAnimationActionMoveInvert.Size = new System.Drawing.Size(22, 21);
            this.cbAnimationActionMoveInvert.TabIndex = 3;
            this.cbAnimationActionMoveInvert.UseVisualStyleBackColor = true;
            this.cbAnimationActionMoveInvert.CheckedChanged += new System.EventHandler(this.AnimationActionMoveInvert_Change);
            // 
            // frmAnimation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 640);
            this.Controls.Add(this.cmdAnimationCancel);
            this.Controls.Add(this.cmdAnimationSave);
            this.Controls.Add(this.tabCollection);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationActionMoveMax)).EndInit();
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
        private Label lblAnimationActionMoveXPercent;
        private NumericUpDown txtAnimationActionMoveMax;
        private Label lblAnimationActionMoveXMax;
        private CheckBox cbAnimationActionMoveInvert;
    }
}