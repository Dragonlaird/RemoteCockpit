namespace CockpitDisplay
{
    partial class frmMain
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
            this.lblFSRunning = new System.Windows.Forms.Label();
            this.lblServerConnected = new System.Windows.Forms.Label();
            this.cbConnected = new System.Windows.Forms.CheckBox();
            this.cbFSRunning = new System.Windows.Forms.CheckBox();
            this.lblCockpitLayout = new System.Windows.Forms.Label();
            this.cmbCockpitLayout = new System.Windows.Forms.ComboBox();
            this.lblAutoCockpitLayout = new System.Windows.Forms.Label();
            this.cbAutoCockpitLayout = new System.Windows.Forms.CheckBox();
            this.pbShowCockpit = new System.Windows.Forms.Button();
            this.grpCockpit = new System.Windows.Forms.GroupBox();
            this.cbCockpitCentre = new System.Windows.Forms.CheckBox();
            this.lblCentre = new System.Windows.Forms.Label();
            this.txtCockpitLeft = new System.Windows.Forms.NumericUpDown();
            this.txtCockpitTop = new System.Windows.Forms.NumericUpDown();
            this.lblLeft = new System.Windows.Forms.Label();
            this.lblTop = new System.Windows.Forms.Label();
            this.txtCockpitWidth = new System.Windows.Forms.NumericUpDown();
            this.txtCockpitHeight = new System.Windows.Forms.NumericUpDown();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.cbFullScreen = new System.Windows.Forms.CheckBox();
            this.lblFullScreen = new System.Windows.Forms.Label();
            this.grpCockpit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFSRunning
            // 
            this.lblFSRunning.AutoSize = true;
            this.lblFSRunning.Location = new System.Drawing.Point(14, 55);
            this.lblFSRunning.Name = "lblFSRunning";
            this.lblFSRunning.Size = new System.Drawing.Size(98, 20);
            this.lblFSRunning.TabIndex = 0;
            this.lblFSRunning.Text = "FS Running:";
            // 
            // lblServerConnected
            // 
            this.lblServerConnected.AutoSize = true;
            this.lblServerConnected.Location = new System.Drawing.Point(20, 12);
            this.lblServerConnected.Name = "lblServerConnected";
            this.lblServerConnected.Size = new System.Drawing.Size(91, 20);
            this.lblServerConnected.TabIndex = 1;
            this.lblServerConnected.Text = "Connected:";
            // 
            // cbConnected
            // 
            this.cbConnected.AutoSize = true;
            this.cbConnected.Enabled = false;
            this.cbConnected.Location = new System.Drawing.Point(117, 12);
            this.cbConnected.Name = "cbConnected";
            this.cbConnected.Size = new System.Drawing.Size(22, 21);
            this.cbConnected.TabIndex = 2;
            this.cbConnected.UseVisualStyleBackColor = true;
            // 
            // cbFSRunning
            // 
            this.cbFSRunning.AutoSize = true;
            this.cbFSRunning.Enabled = false;
            this.cbFSRunning.Location = new System.Drawing.Point(117, 55);
            this.cbFSRunning.Name = "cbFSRunning";
            this.cbFSRunning.Size = new System.Drawing.Size(22, 21);
            this.cbFSRunning.TabIndex = 3;
            this.cbFSRunning.UseVisualStyleBackColor = true;
            // 
            // lblCockpitLayout
            // 
            this.lblCockpitLayout.AutoSize = true;
            this.lblCockpitLayout.Location = new System.Drawing.Point(50, 105);
            this.lblCockpitLayout.Name = "lblCockpitLayout";
            this.lblCockpitLayout.Size = new System.Drawing.Size(61, 20);
            this.lblCockpitLayout.TabIndex = 4;
            this.lblCockpitLayout.Text = "Layout:";
            // 
            // cmbCockpitLayout
            // 
            this.cmbCockpitLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCockpitLayout.FormattingEnabled = true;
            this.cmbCockpitLayout.Items.AddRange(new object[] {
            "Generic"});
            this.cmbCockpitLayout.Location = new System.Drawing.Point(117, 102);
            this.cmbCockpitLayout.Name = "cmbCockpitLayout";
            this.cmbCockpitLayout.Size = new System.Drawing.Size(403, 28);
            this.cmbCockpitLayout.TabIndex = 5;
            // 
            // lblAutoCockpitLayout
            // 
            this.lblAutoCockpitLayout.AutoSize = true;
            this.lblAutoCockpitLayout.Location = new System.Drawing.Point(10, 149);
            this.lblAutoCockpitLayout.Name = "lblAutoCockpitLayout";
            this.lblAutoCockpitLayout.Size = new System.Drawing.Size(100, 20);
            this.lblAutoCockpitLayout.TabIndex = 6;
            this.lblAutoCockpitLayout.Text = "Auto-Layout:";
            // 
            // cbAutoCockpitLayout
            // 
            this.cbAutoCockpitLayout.AutoSize = true;
            this.cbAutoCockpitLayout.Location = new System.Drawing.Point(117, 149);
            this.cbAutoCockpitLayout.Name = "cbAutoCockpitLayout";
            this.cbAutoCockpitLayout.Size = new System.Drawing.Size(22, 21);
            this.cbAutoCockpitLayout.TabIndex = 7;
            this.cbAutoCockpitLayout.UseVisualStyleBackColor = true;
            this.cbAutoCockpitLayout.CheckedChanged += new System.EventHandler(this.cbAutoCockpitLayout_CheckedChanged);
            // 
            // pbShowCockpit
            // 
            this.pbShowCockpit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbShowCockpit.Location = new System.Drawing.Point(384, 462);
            this.pbShowCockpit.Name = "pbShowCockpit";
            this.pbShowCockpit.Size = new System.Drawing.Size(170, 35);
            this.pbShowCockpit.TabIndex = 8;
            this.pbShowCockpit.Text = "Show Cockpit";
            this.pbShowCockpit.UseVisualStyleBackColor = true;
            this.pbShowCockpit.Click += new System.EventHandler(this.pbShowCockpit_Click);
            // 
            // grpCockpit
            // 
            this.grpCockpit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCockpit.Controls.Add(this.cbCockpitCentre);
            this.grpCockpit.Controls.Add(this.lblCentre);
            this.grpCockpit.Controls.Add(this.txtCockpitLeft);
            this.grpCockpit.Controls.Add(this.txtCockpitTop);
            this.grpCockpit.Controls.Add(this.lblLeft);
            this.grpCockpit.Controls.Add(this.lblTop);
            this.grpCockpit.Controls.Add(this.txtCockpitWidth);
            this.grpCockpit.Controls.Add(this.txtCockpitHeight);
            this.grpCockpit.Controls.Add(this.lblWidth);
            this.grpCockpit.Controls.Add(this.lblHeight);
            this.grpCockpit.Controls.Add(this.cbFullScreen);
            this.grpCockpit.Controls.Add(this.lblFullScreen);
            this.grpCockpit.Location = new System.Drawing.Point(15, 178);
            this.grpCockpit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCockpit.Name = "grpCockpit";
            this.grpCockpit.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpCockpit.Size = new System.Drawing.Size(538, 258);
            this.grpCockpit.TabIndex = 9;
            this.grpCockpit.TabStop = false;
            this.grpCockpit.Text = "Cockpit Window";
            // 
            // cbCockpitCentre
            // 
            this.cbCockpitCentre.AutoSize = true;
            this.cbCockpitCentre.Enabled = false;
            this.cbCockpitCentre.Location = new System.Drawing.Point(130, 60);
            this.cbCockpitCentre.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbCockpitCentre.Name = "cbCockpitCentre";
            this.cbCockpitCentre.Size = new System.Drawing.Size(22, 21);
            this.cbCockpitCentre.TabIndex = 13;
            this.cbCockpitCentre.UseVisualStyleBackColor = true;
            this.cbCockpitCentre.CheckedChanged += new System.EventHandler(this.cbCockpitCentre_CheckedChanged);
            // 
            // lblCentre
            // 
            this.lblCentre.AutoSize = true;
            this.lblCentre.Location = new System.Drawing.Point(9, 60);
            this.lblCentre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCentre.Name = "lblCentre";
            this.lblCentre.Size = new System.Drawing.Size(116, 20);
            this.lblCentre.TabIndex = 12;
            this.lblCentre.Text = "Centre Screen:";
            // 
            // txtCockpitLeft
            // 
            this.txtCockpitLeft.Enabled = false;
            this.txtCockpitLeft.Location = new System.Drawing.Point(106, 129);
            this.txtCockpitLeft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCockpitLeft.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtCockpitLeft.Name = "txtCockpitLeft";
            this.txtCockpitLeft.ReadOnly = true;
            this.txtCockpitLeft.Size = new System.Drawing.Size(180, 26);
            this.txtCockpitLeft.TabIndex = 11;
            this.txtCockpitLeft.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.txtCockpitLeft.ValueChanged += new System.EventHandler(this.cbCockpitCentre_CheckedChanged);
            // 
            // txtCockpitTop
            // 
            this.txtCockpitTop.Enabled = false;
            this.txtCockpitTop.Location = new System.Drawing.Point(106, 94);
            this.txtCockpitTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCockpitTop.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtCockpitTop.Name = "txtCockpitTop";
            this.txtCockpitTop.ReadOnly = true;
            this.txtCockpitTop.Size = new System.Drawing.Size(180, 26);
            this.txtCockpitTop.TabIndex = 10;
            this.txtCockpitTop.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.txtCockpitTop.ValueChanged += new System.EventHandler(this.cbCockpitCentre_CheckedChanged);
            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Location = new System.Drawing.Point(50, 135);
            this.lblLeft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(41, 20);
            this.lblLeft.TabIndex = 9;
            this.lblLeft.Text = "Left:";
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new System.Drawing.Point(48, 100);
            this.lblTop.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(40, 20);
            this.lblTop.TabIndex = 8;
            this.lblTop.Text = "Top:";
            // 
            // txtCockpitWidth
            // 
            this.txtCockpitWidth.Enabled = false;
            this.txtCockpitWidth.Location = new System.Drawing.Point(106, 200);
            this.txtCockpitWidth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCockpitWidth.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtCockpitWidth.Name = "txtCockpitWidth";
            this.txtCockpitWidth.ReadOnly = true;
            this.txtCockpitWidth.Size = new System.Drawing.Size(180, 26);
            this.txtCockpitWidth.TabIndex = 7;
            this.txtCockpitWidth.Value = new decimal(new int[] {
            1400,
            0,
            0,
            0});
            this.txtCockpitWidth.ValueChanged += new System.EventHandler(this.cbFullScreen_CheckedChanged);
            // 
            // txtCockpitHeight
            // 
            this.txtCockpitHeight.Enabled = false;
            this.txtCockpitHeight.Location = new System.Drawing.Point(106, 165);
            this.txtCockpitHeight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCockpitHeight.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtCockpitHeight.Name = "txtCockpitHeight";
            this.txtCockpitHeight.ReadOnly = true;
            this.txtCockpitHeight.Size = new System.Drawing.Size(180, 26);
            this.txtCockpitHeight.TabIndex = 6;
            this.txtCockpitHeight.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtCockpitHeight.ValueChanged += new System.EventHandler(this.cbFullScreen_CheckedChanged);
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(34, 206);
            this.lblWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(54, 20);
            this.lblWidth.TabIndex = 4;
            this.lblWidth.Text = "Width:";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(36, 171);
            this.lblHeight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(60, 20);
            this.lblHeight.TabIndex = 2;
            this.lblHeight.Text = "Height:";
            // 
            // cbFullScreen
            // 
            this.cbFullScreen.AutoSize = true;
            this.cbFullScreen.Checked = true;
            this.cbFullScreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFullScreen.Location = new System.Drawing.Point(130, 29);
            this.cbFullScreen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbFullScreen.Name = "cbFullScreen";
            this.cbFullScreen.Size = new System.Drawing.Size(22, 21);
            this.cbFullScreen.TabIndex = 1;
            this.cbFullScreen.UseVisualStyleBackColor = true;
            this.cbFullScreen.CheckedChanged += new System.EventHandler(this.cbFullScreen_CheckedChanged);
            // 
            // lblFullScreen
            // 
            this.lblFullScreen.AutoSize = true;
            this.lblFullScreen.Location = new System.Drawing.Point(32, 29);
            this.lblFullScreen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFullScreen.Name = "lblFullScreen";
            this.lblFullScreen.Size = new System.Drawing.Size(93, 20);
            this.lblFullScreen.TabIndex = 0;
            this.lblFullScreen.Text = "Full Screen:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 514);
            this.Controls.Add(this.grpCockpit);
            this.Controls.Add(this.pbShowCockpit);
            this.Controls.Add(this.cbAutoCockpitLayout);
            this.Controls.Add(this.lblAutoCockpitLayout);
            this.Controls.Add(this.cmbCockpitLayout);
            this.Controls.Add(this.lblCockpitLayout);
            this.Controls.Add(this.cbFSRunning);
            this.Controls.Add(this.cbConnected);
            this.Controls.Add(this.lblServerConnected);
            this.Controls.Add(this.lblFSRunning);
            this.Name = "frmMain";
            this.Text = "Remote Cockpit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.grpCockpit.ResumeLayout(false);
            this.grpCockpit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFSRunning;
        private System.Windows.Forms.Label lblServerConnected;
        private System.Windows.Forms.CheckBox cbConnected;
        private System.Windows.Forms.CheckBox cbFSRunning;
        private System.Windows.Forms.Label lblCockpitLayout;
        private System.Windows.Forms.ComboBox cmbCockpitLayout;
        private System.Windows.Forms.Label lblAutoCockpitLayout;
        private System.Windows.Forms.CheckBox cbAutoCockpitLayout;
        private System.Windows.Forms.Button pbShowCockpit;
        private System.Windows.Forms.GroupBox grpCockpit;
        private System.Windows.Forms.CheckBox cbFullScreen;
        private System.Windows.Forms.Label lblFullScreen;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.NumericUpDown txtCockpitWidth;
        private System.Windows.Forms.NumericUpDown txtCockpitHeight;
        private System.Windows.Forms.NumericUpDown txtCockpitLeft;
        private System.Windows.Forms.NumericUpDown txtCockpitTop;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.CheckBox cbCockpitCentre;
        private System.Windows.Forms.Label lblCentre;
    }
}