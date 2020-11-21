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
            this.lblAnimationScaleMethod = new System.Windows.Forms.Label();
            this.cmbAnimationScaleMethod = new System.Windows.Forms.ComboBox();
            this.gpAnimationDrawing = new System.Windows.Forms.GroupBox();
            this.cmbAnimationFillMethod = new System.Windows.Forms.ComboBox();
            this.lblAnimationFillMethod = new System.Windows.Forms.Label();
            this.cmbAnimationFillColor = new System.Windows.Forms.ComboBox();
            this.lblAnimationFillColor = new System.Windows.Forms.Label();
            this.gpAnimationImage = new System.Windows.Forms.GroupBox();
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
            this.txtAnimationRelativeX = new System.Windows.Forms.NumericUpDown();
            this.txtAnimationRelativeY = new System.Windows.Forms.NumericUpDown();
            this.tabCollection.SuspendLayout();
            this.tabWhat.SuspendLayout();
            this.gpAnimationDrawing.SuspendLayout();
            this.gpAnimationImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimationImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationRelativeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationRelativeY)).BeginInit();
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
            this.tabWhat.Controls.Add(this.lblAnimationScaleMethod);
            this.tabWhat.Controls.Add(this.cmbAnimationScaleMethod);
            this.tabWhat.Controls.Add(this.gpAnimationDrawing);
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
            // lblAnimationScaleMethod
            // 
            this.lblAnimationScaleMethod.AutoSize = true;
            this.lblAnimationScaleMethod.Location = new System.Drawing.Point(25, 66);
            this.lblAnimationScaleMethod.Name = "lblAnimationScaleMethod";
            this.lblAnimationScaleMethod.Size = new System.Drawing.Size(37, 13);
            this.lblAnimationScaleMethod.TabIndex = 13;
            this.lblAnimationScaleMethod.Text = "Scale:";
            // 
            // cmbAnimationScaleMethod
            // 
            this.cmbAnimationScaleMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationScaleMethod.FormattingEnabled = true;
            this.cmbAnimationScaleMethod.Location = new System.Drawing.Point(69, 63);
            this.cmbAnimationScaleMethod.Name = "cmbAnimationScaleMethod";
            this.cmbAnimationScaleMethod.Size = new System.Drawing.Size(186, 21);
            this.cmbAnimationScaleMethod.TabIndex = 12;
            // 
            // gpAnimationDrawing
            // 
            this.gpAnimationDrawing.Controls.Add(this.cmbAnimationFillMethod);
            this.gpAnimationDrawing.Controls.Add(this.lblAnimationFillMethod);
            this.gpAnimationDrawing.Controls.Add(this.cmbAnimationFillColor);
            this.gpAnimationDrawing.Controls.Add(this.lblAnimationFillColor);
            this.gpAnimationDrawing.Controls.Add(this.gpAnimationImage);
            this.gpAnimationDrawing.Location = new System.Drawing.Point(14, 90);
            this.gpAnimationDrawing.Name = "gpAnimationDrawing";
            this.gpAnimationDrawing.Size = new System.Drawing.Size(442, 236);
            this.gpAnimationDrawing.TabIndex = 7;
            this.gpAnimationDrawing.TabStop = false;
            this.gpAnimationDrawing.Text = "Drawing";
            this.gpAnimationDrawing.Visible = false;
            // 
            // cmbAnimationFillMethod
            // 
            this.cmbAnimationFillMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationFillMethod.FormattingEnabled = true;
            this.cmbAnimationFillMethod.Location = new System.Drawing.Point(68, 49);
            this.cmbAnimationFillMethod.Name = "cmbAnimationFillMethod";
            this.cmbAnimationFillMethod.Size = new System.Drawing.Size(121, 21);
            this.cmbAnimationFillMethod.TabIndex = 10;
            // 
            // lblAnimationFillMethod
            // 
            this.lblAnimationFillMethod.AutoSize = true;
            this.lblAnimationFillMethod.Location = new System.Drawing.Point(1, 52);
            this.lblAnimationFillMethod.Name = "lblAnimationFillMethod";
            this.lblAnimationFillMethod.Size = new System.Drawing.Size(61, 13);
            this.lblAnimationFillMethod.TabIndex = 9;
            this.lblAnimationFillMethod.Text = "Fill Method:";
            // 
            // cmbAnimationFillColor
            // 
            this.cmbAnimationFillColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnimationFillColor.FormattingEnabled = true;
            this.cmbAnimationFillColor.Location = new System.Drawing.Point(68, 17);
            this.cmbAnimationFillColor.Name = "cmbAnimationFillColor";
            this.cmbAnimationFillColor.Size = new System.Drawing.Size(121, 21);
            this.cmbAnimationFillColor.TabIndex = 8;
            // 
            // lblAnimationFillColor
            // 
            this.lblAnimationFillColor.AutoSize = true;
            this.lblAnimationFillColor.Location = new System.Drawing.Point(13, 20);
            this.lblAnimationFillColor.Name = "lblAnimationFillColor";
            this.lblAnimationFillColor.Size = new System.Drawing.Size(49, 13);
            this.lblAnimationFillColor.TabIndex = 7;
            this.lblAnimationFillColor.Text = "Fill Color:";
            // 
            // gpAnimationImage
            // 
            this.gpAnimationImage.Controls.Add(this.txtAnimationRelativeY);
            this.gpAnimationImage.Controls.Add(this.txtAnimationRelativeX);
            this.gpAnimationImage.Controls.Add(this.lblAnimationRelativeY);
            this.gpAnimationImage.Controls.Add(this.lblAnimationRelativeX);
            this.gpAnimationImage.Controls.Add(this.pbAnimationImage);
            this.gpAnimationImage.Controls.Add(this.txtAnimationImagePath);
            this.gpAnimationImage.Controls.Add(this.lblAnimationImagePath);
            this.gpAnimationImage.Location = new System.Drawing.Point(144, 107);
            this.gpAnimationImage.Name = "gpAnimationImage";
            this.gpAnimationImage.Size = new System.Drawing.Size(442, 234);
            this.gpAnimationImage.TabIndex = 6;
            this.gpAnimationImage.TabStop = false;
            this.gpAnimationImage.Text = "Image";
            // 
            // lblAnimationRelativeY
            // 
            this.lblAnimationRelativeY.AutoSize = true;
            this.lblAnimationRelativeY.Location = new System.Drawing.Point(1, 45);
            this.lblAnimationRelativeY.Name = "lblAnimationRelativeY";
            this.lblAnimationRelativeY.Size = new System.Drawing.Size(47, 13);
            this.lblAnimationRelativeY.TabIndex = 20;
            this.lblAnimationRelativeY.Text = "Scale Y:";
            // 
            // lblAnimationRelativeX
            // 
            this.lblAnimationRelativeX.AutoSize = true;
            this.lblAnimationRelativeX.Location = new System.Drawing.Point(1, 22);
            this.lblAnimationRelativeX.Name = "lblAnimationRelativeX";
            this.lblAnimationRelativeX.Size = new System.Drawing.Size(47, 13);
            this.lblAnimationRelativeX.TabIndex = 18;
            this.lblAnimationRelativeX.Text = "Scale X:";
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
            this.cmbAnimationType.Location = new System.Drawing.Point(69, 36);
            this.cmbAnimationType.Name = "cmbAnimationType";
            this.cmbAnimationType.Size = new System.Drawing.Size(185, 21);
            this.cmbAnimationType.TabIndex = 3;
            this.cmbAnimationType.SelectedIndexChanged += new System.EventHandler(this.ChangeAnimationType_Select);
            // 
            // lblAnimationType
            // 
            this.lblAnimationType.AutoSize = true;
            this.lblAnimationType.Location = new System.Drawing.Point(28, 39);
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
            // txtAnimationRelativeX
            // 
            this.txtAnimationRelativeX.Location = new System.Drawing.Point(56, 20);
            this.txtAnimationRelativeX.Name = "txtAnimationRelativeX";
            this.txtAnimationRelativeX.Size = new System.Drawing.Size(120, 20);
            this.txtAnimationRelativeX.TabIndex = 22;
            // 
            // txtAnimationRelativeY
            // 
            this.txtAnimationRelativeY.Location = new System.Drawing.Point(56, 43);
            this.txtAnimationRelativeY.Name = "txtAnimationRelativeY";
            this.txtAnimationRelativeY.Size = new System.Drawing.Size(120, 20);
            this.txtAnimationRelativeY.TabIndex = 23;
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
            this.gpAnimationImage.ResumeLayout(false);
            this.gpAnimationImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimationImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationRelativeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnimationRelativeY)).EndInit();
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
    }
}