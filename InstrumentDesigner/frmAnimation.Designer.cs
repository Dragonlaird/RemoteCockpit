﻿namespace InstrumentDesigner
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
            this.lblAnimationScaleMethod = new System.Windows.Forms.Label();
            this.cmbAnimationScaleMethod = new System.Windows.Forms.ComboBox();
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
            this.SuspendLayout();
            // 
            // tabCollection
            // 
            this.tabCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCollection.Controls.Add(this.tabWhat);
            this.tabCollection.Controls.Add(this.tabWhen);
            this.tabCollection.Location = new System.Drawing.Point(12, 12);
            this.tabCollection.Name = "tabCollection";
            this.tabCollection.SelectedIndex = 0;
            this.tabCollection.Size = new System.Drawing.Size(474, 364);
            this.tabCollection.TabIndex = 0;
            // 
            // tabWhat
            // 
            this.tabWhat.Controls.Add(this.lblAnimationTypeWarning);
            this.tabWhat.Controls.Add(this.lblAnimationScaleMethod);
            this.tabWhat.Controls.Add(this.cmbAnimationScaleMethod);
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
            // lblAnimationScaleMethod
            // 
            this.lblAnimationScaleMethod.AutoSize = true;
            this.lblAnimationScaleMethod.Location = new System.Drawing.Point(25, 62);
            this.lblAnimationScaleMethod.Name = "lblAnimationScaleMethod";
            this.lblAnimationScaleMethod.Size = new System.Drawing.Size(37, 13);
            this.lblAnimationScaleMethod.TabIndex = 13;
            this.lblAnimationScaleMethod.Text = "Scale:";
            // 
            // cmbAnimationScaleMethod
            // 
            this.cmbAnimationScaleMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationScaleMethod.FormattingEnabled = true;
            this.cmbAnimationScaleMethod.Location = new System.Drawing.Point(69, 59);
            this.cmbAnimationScaleMethod.Name = "cmbAnimationScaleMethod";
            this.cmbAnimationScaleMethod.Size = new System.Drawing.Size(186, 21);
            this.cmbAnimationScaleMethod.TabIndex = 12;
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
            this.dgAnimationPlotPoints.Size = new System.Drawing.Size(178, 195);
            this.dgAnimationPlotPoints.TabIndex = 11;
            this.dgAnimationPlotPoints.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PlotPoint_Change);
            this.dgAnimationPlotPoints.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.PlotPointAdd_Change);
            this.dgAnimationPlotPoints.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.PlotPointRemove_Change);
            // 
            // pointX
            // 
            this.pointX.HeaderText = "X";
            this.pointX.Name = "pointX";
            this.pointX.Width = 50;
            // 
            // pointY
            // 
            this.pointY.HeaderText = "Y";
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
            this.tabWhen.Location = new System.Drawing.Point(4, 22);
            this.tabWhen.Name = "tabWhen";
            this.tabWhen.Padding = new System.Windows.Forms.Padding(3);
            this.tabWhen.Size = new System.Drawing.Size(466, 338);
            this.tabWhen.TabIndex = 1;
            this.tabWhen.Text = "When";
            this.tabWhen.UseVisualStyleBackColor = true;
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
        private System.Windows.Forms.Label lblAnimationScaleMethod;
        private System.Windows.Forms.ComboBox cmbAnimationScaleMethod;
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
    }
}