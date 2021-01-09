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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCockpitWindow = new System.Windows.Forms.TabPage();
            this.grpCockpit = new System.Windows.Forms.GroupBox();
            this.txtServerPort = new System.Windows.Forms.NumericUpDown();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.txtServerAddress = new System.Windows.Forms.TextBox();
            this.lblServerAddress = new System.Windows.Forms.Label();
            this.cbConnected = new System.Windows.Forms.CheckBox();
            this.lblServerConnected = new System.Windows.Forms.Label();
            this.cbFSRunning = new System.Windows.Forms.CheckBox();
            this.lblFSRunning = new System.Windows.Forms.Label();
            this.lblAutoCockpitLayout = new System.Windows.Forms.Label();
            this.cbAutoCockpitLayout = new System.Windows.Forms.CheckBox();
            this.lblCockpitLayout = new System.Windows.Forms.Label();
            this.cmbCockpitLayout = new System.Windows.Forms.ComboBox();
            this.pbShowCockpit = new System.Windows.Forms.Button();
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
            this.tabValues = new System.Windows.Forms.TabPage();
            this.dgValues = new System.Windows.Forms.DataGridView();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.lblDebugMessages = new System.Windows.Forms.Label();
            this.txtDebugMessages = new System.Windows.Forms.TextBox();
            this.SimVar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Updated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabCockpitWindow.SuspendLayout();
            this.grpCockpit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitHeight)).BeginInit();
            this.tabValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgValues)).BeginInit();
            this.tabDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabCockpitWindow);
            this.tabControl1.Controls.Add(this.tabValues);
            this.tabControl1.Controls.Add(this.tabDebug);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(461, 333);
            this.tabControl1.TabIndex = 10;
            // 
            // tabCockpitWindow
            // 
            this.tabCockpitWindow.Controls.Add(this.grpCockpit);
            this.tabCockpitWindow.Location = new System.Drawing.Point(4, 22);
            this.tabCockpitWindow.Margin = new System.Windows.Forms.Padding(2);
            this.tabCockpitWindow.Name = "tabCockpitWindow";
            this.tabCockpitWindow.Padding = new System.Windows.Forms.Padding(2);
            this.tabCockpitWindow.Size = new System.Drawing.Size(453, 307);
            this.tabCockpitWindow.TabIndex = 0;
            this.tabCockpitWindow.Text = "Cockpit Window";
            this.tabCockpitWindow.UseVisualStyleBackColor = true;
            // 
            // grpCockpit
            // 
            this.grpCockpit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCockpit.Controls.Add(this.txtServerPort);
            this.grpCockpit.Controls.Add(this.lblServerPort);
            this.grpCockpit.Controls.Add(this.txtServerAddress);
            this.grpCockpit.Controls.Add(this.lblServerAddress);
            this.grpCockpit.Controls.Add(this.cbConnected);
            this.grpCockpit.Controls.Add(this.lblServerConnected);
            this.grpCockpit.Controls.Add(this.cbFSRunning);
            this.grpCockpit.Controls.Add(this.lblFSRunning);
            this.grpCockpit.Controls.Add(this.lblAutoCockpitLayout);
            this.grpCockpit.Controls.Add(this.cbAutoCockpitLayout);
            this.grpCockpit.Controls.Add(this.lblCockpitLayout);
            this.grpCockpit.Controls.Add(this.cmbCockpitLayout);
            this.grpCockpit.Controls.Add(this.pbShowCockpit);
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
            this.grpCockpit.Location = new System.Drawing.Point(-3, 5);
            this.grpCockpit.Name = "grpCockpit";
            this.grpCockpit.Size = new System.Drawing.Size(447, 293);
            this.grpCockpit.TabIndex = 18;
            this.grpCockpit.TabStop = false;
            this.grpCockpit.Text = "Cockpit Window";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(112, 40);
            this.txtServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(73, 20);
            this.txtServerPort.TabIndex = 27;
            this.txtServerPort.Value = new decimal(new int[] {
            5555,
            0,
            0,
            0});
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(36, 44);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(63, 13);
            this.lblServerPort.TabIndex = 26;
            this.lblServerPort.Text = "Server Port:";
            // 
            // txtServerAddress
            // 
            this.txtServerAddress.Location = new System.Drawing.Point(112, 17);
            this.txtServerAddress.Name = "txtServerAddress";
            this.txtServerAddress.Size = new System.Drawing.Size(100, 20);
            this.txtServerAddress.TabIndex = 25;
            this.txtServerAddress.Text = "127.0.0.1";
            // 
            // lblServerAddress
            // 
            this.lblServerAddress.AutoSize = true;
            this.lblServerAddress.Location = new System.Drawing.Point(24, 20);
            this.lblServerAddress.Name = "lblServerAddress";
            this.lblServerAddress.Size = new System.Drawing.Size(82, 13);
            this.lblServerAddress.TabIndex = 24;
            this.lblServerAddress.Text = "Server Address:";
            // 
            // cbConnected
            // 
            this.cbConnected.AutoSize = true;
            this.cbConnected.Enabled = false;
            this.cbConnected.Location = new System.Drawing.Point(112, 125);
            this.cbConnected.Margin = new System.Windows.Forms.Padding(2);
            this.cbConnected.Name = "cbConnected";
            this.cbConnected.Size = new System.Drawing.Size(15, 14);
            this.cbConnected.TabIndex = 23;
            this.cbConnected.UseVisualStyleBackColor = true;
            // 
            // lblServerConnected
            // 
            this.lblServerConnected.AutoSize = true;
            this.lblServerConnected.Location = new System.Drawing.Point(26, 127);
            this.lblServerConnected.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblServerConnected.Name = "lblServerConnected";
            this.lblServerConnected.Size = new System.Drawing.Size(62, 13);
            this.lblServerConnected.TabIndex = 22;
            this.lblServerConnected.Text = "Connected:";
            // 
            // cbFSRunning
            // 
            this.cbFSRunning.AutoSize = true;
            this.cbFSRunning.Enabled = false;
            this.cbFSRunning.Location = new System.Drawing.Point(112, 108);
            this.cbFSRunning.Margin = new System.Windows.Forms.Padding(2);
            this.cbFSRunning.Name = "cbFSRunning";
            this.cbFSRunning.Size = new System.Drawing.Size(15, 14);
            this.cbFSRunning.TabIndex = 21;
            this.cbFSRunning.UseVisualStyleBackColor = true;
            // 
            // lblFSRunning
            // 
            this.lblFSRunning.AutoSize = true;
            this.lblFSRunning.Location = new System.Drawing.Point(21, 110);
            this.lblFSRunning.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFSRunning.Name = "lblFSRunning";
            this.lblFSRunning.Size = new System.Drawing.Size(66, 13);
            this.lblFSRunning.TabIndex = 20;
            this.lblFSRunning.Text = "FS Running:";
            // 
            // lblAutoCockpitLayout
            // 
            this.lblAutoCockpitLayout.AutoSize = true;
            this.lblAutoCockpitLayout.Location = new System.Drawing.Point(20, 92);
            this.lblAutoCockpitLayout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAutoCockpitLayout.Name = "lblAutoCockpitLayout";
            this.lblAutoCockpitLayout.Size = new System.Drawing.Size(67, 13);
            this.lblAutoCockpitLayout.TabIndex = 19;
            this.lblAutoCockpitLayout.Text = "Auto-Layout:";
            // 
            // cbAutoCockpitLayout
            // 
            this.cbAutoCockpitLayout.AutoSize = true;
            this.cbAutoCockpitLayout.Location = new System.Drawing.Point(112, 90);
            this.cbAutoCockpitLayout.Margin = new System.Windows.Forms.Padding(2);
            this.cbAutoCockpitLayout.Name = "cbAutoCockpitLayout";
            this.cbAutoCockpitLayout.Size = new System.Drawing.Size(15, 14);
            this.cbAutoCockpitLayout.TabIndex = 18;
            this.cbAutoCockpitLayout.UseVisualStyleBackColor = true;
            this.cbAutoCockpitLayout.CheckedChanged += new System.EventHandler(this.cbAutoCockpitLayout_CheckedChanged);
            // 
            // lblCockpitLayout
            // 
            this.lblCockpitLayout.AutoSize = true;
            this.lblCockpitLayout.Location = new System.Drawing.Point(46, 69);
            this.lblCockpitLayout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCockpitLayout.Name = "lblCockpitLayout";
            this.lblCockpitLayout.Size = new System.Drawing.Size(42, 13);
            this.lblCockpitLayout.TabIndex = 17;
            this.lblCockpitLayout.Text = "Layout:";
            // 
            // cmbCockpitLayout
            // 
            this.cmbCockpitLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCockpitLayout.FormattingEnabled = true;
            this.cmbCockpitLayout.Items.AddRange(new object[] {
            "Generic"});
            this.cmbCockpitLayout.Location = new System.Drawing.Point(112, 66);
            this.cmbCockpitLayout.Margin = new System.Windows.Forms.Padding(2);
            this.cmbCockpitLayout.Name = "cmbCockpitLayout";
            this.cmbCockpitLayout.Size = new System.Drawing.Size(254, 21);
            this.cmbCockpitLayout.TabIndex = 16;
            // 
            // pbShowCockpit
            // 
            this.pbShowCockpit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbShowCockpit.Location = new System.Drawing.Point(323, 265);
            this.pbShowCockpit.Margin = new System.Windows.Forms.Padding(2);
            this.pbShowCockpit.Name = "pbShowCockpit";
            this.pbShowCockpit.Size = new System.Drawing.Size(113, 23);
            this.pbShowCockpit.TabIndex = 14;
            this.pbShowCockpit.Text = "Show Cockpit";
            this.pbShowCockpit.UseVisualStyleBackColor = true;
            this.pbShowCockpit.Click += new System.EventHandler(this.pbShowCockpit_Click);
            // 
            // cbCockpitCentre
            // 
            this.cbCockpitCentre.AutoSize = true;
            this.cbCockpitCentre.Enabled = false;
            this.cbCockpitCentre.Location = new System.Drawing.Point(112, 160);
            this.cbCockpitCentre.Name = "cbCockpitCentre";
            this.cbCockpitCentre.Size = new System.Drawing.Size(15, 14);
            this.cbCockpitCentre.TabIndex = 13;
            this.cbCockpitCentre.UseVisualStyleBackColor = true;
            this.cbCockpitCentre.CheckedChanged += new System.EventHandler(this.cbCockpitCentre_CheckedChanged);
            // 
            // lblCentre
            // 
            this.lblCentre.AutoSize = true;
            this.lblCentre.Location = new System.Drawing.Point(9, 162);
            this.lblCentre.Name = "lblCentre";
            this.lblCentre.Size = new System.Drawing.Size(78, 13);
            this.lblCentre.TabIndex = 12;
            this.lblCentre.Text = "Centre Screen:";
            // 
            // txtCockpitLeft
            // 
            this.txtCockpitLeft.Enabled = false;
            this.txtCockpitLeft.Location = new System.Drawing.Point(112, 207);
            this.txtCockpitLeft.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtCockpitLeft.Name = "txtCockpitLeft";
            this.txtCockpitLeft.ReadOnly = true;
            this.txtCockpitLeft.Size = new System.Drawing.Size(120, 20);
            this.txtCockpitLeft.TabIndex = 11;
            this.txtCockpitLeft.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // txtCockpitTop
            // 
            this.txtCockpitTop.Enabled = false;
            this.txtCockpitTop.Location = new System.Drawing.Point(112, 184);
            this.txtCockpitTop.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtCockpitTop.Name = "txtCockpitTop";
            this.txtCockpitTop.ReadOnly = true;
            this.txtCockpitTop.Size = new System.Drawing.Size(120, 20);
            this.txtCockpitTop.TabIndex = 10;
            this.txtCockpitTop.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Location = new System.Drawing.Point(37, 212);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(28, 13);
            this.lblLeft.TabIndex = 9;
            this.lblLeft.Text = "Left:";
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new System.Drawing.Point(36, 190);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(29, 13);
            this.lblTop.TabIndex = 8;
            this.lblTop.Text = "Top:";
            // 
            // txtCockpitWidth
            // 
            this.txtCockpitWidth.Enabled = false;
            this.txtCockpitWidth.Location = new System.Drawing.Point(112, 253);
            this.txtCockpitWidth.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtCockpitWidth.Name = "txtCockpitWidth";
            this.txtCockpitWidth.ReadOnly = true;
            this.txtCockpitWidth.Size = new System.Drawing.Size(120, 20);
            this.txtCockpitWidth.TabIndex = 7;
            this.txtCockpitWidth.Value = new decimal(new int[] {
            1400,
            0,
            0,
            0});
            // 
            // txtCockpitHeight
            // 
            this.txtCockpitHeight.Enabled = false;
            this.txtCockpitHeight.Location = new System.Drawing.Point(112, 230);
            this.txtCockpitHeight.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtCockpitHeight.Name = "txtCockpitHeight";
            this.txtCockpitHeight.ReadOnly = true;
            this.txtCockpitHeight.Size = new System.Drawing.Size(120, 20);
            this.txtCockpitHeight.TabIndex = 6;
            this.txtCockpitHeight.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(27, 259);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(38, 13);
            this.lblWidth.TabIndex = 4;
            this.lblWidth.Text = "Width:";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(28, 236);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(41, 13);
            this.lblHeight.TabIndex = 2;
            this.lblHeight.Text = "Height:";
            // 
            // cbFullScreen
            // 
            this.cbFullScreen.AutoSize = true;
            this.cbFullScreen.Checked = true;
            this.cbFullScreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFullScreen.Location = new System.Drawing.Point(112, 143);
            this.cbFullScreen.Name = "cbFullScreen";
            this.cbFullScreen.Size = new System.Drawing.Size(15, 14);
            this.cbFullScreen.TabIndex = 1;
            this.cbFullScreen.UseVisualStyleBackColor = true;
            this.cbFullScreen.CheckedChanged += new System.EventHandler(this.cbFullScreen_CheckedChanged);
            // 
            // lblFullScreen
            // 
            this.lblFullScreen.AutoSize = true;
            this.lblFullScreen.Location = new System.Drawing.Point(25, 145);
            this.lblFullScreen.Name = "lblFullScreen";
            this.lblFullScreen.Size = new System.Drawing.Size(63, 13);
            this.lblFullScreen.TabIndex = 0;
            this.lblFullScreen.Text = "Full Screen:";
            // 
            // tabValues
            // 
            this.tabValues.Controls.Add(this.dgValues);
            this.tabValues.Location = new System.Drawing.Point(4, 22);
            this.tabValues.Name = "tabValues";
            this.tabValues.Padding = new System.Windows.Forms.Padding(3);
            this.tabValues.Size = new System.Drawing.Size(453, 307);
            this.tabValues.TabIndex = 2;
            this.tabValues.Text = "Latest Values";
            this.tabValues.UseVisualStyleBackColor = true;
            // 
            // dgValues
            // 
            this.dgValues.AllowUserToAddRows = false;
            this.dgValues.AllowUserToDeleteRows = false;
            this.dgValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SimVar,
            this.Value,
            this.Updated});
            this.dgValues.Location = new System.Drawing.Point(7, 24);
            this.dgValues.Name = "dgValues";
            this.dgValues.ReadOnly = true;
            this.dgValues.Size = new System.Drawing.Size(440, 277);
            this.dgValues.TabIndex = 0;
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.lblDebugMessages);
            this.tabDebug.Controls.Add(this.txtDebugMessages);
            this.tabDebug.Location = new System.Drawing.Point(4, 22);
            this.tabDebug.Margin = new System.Windows.Forms.Padding(2);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(2);
            this.tabDebug.Size = new System.Drawing.Size(453, 307);
            this.tabDebug.TabIndex = 1;
            this.tabDebug.Text = "Debug";
            this.tabDebug.UseVisualStyleBackColor = true;
            // 
            // lblDebugMessages
            // 
            this.lblDebugMessages.AutoSize = true;
            this.lblDebugMessages.Location = new System.Drawing.Point(5, 5);
            this.lblDebugMessages.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDebugMessages.Name = "lblDebugMessages";
            this.lblDebugMessages.Size = new System.Drawing.Size(93, 13);
            this.lblDebugMessages.TabIndex = 1;
            this.lblDebugMessages.Text = "Debug Messages:";
            // 
            // txtDebugMessages
            // 
            this.txtDebugMessages.Location = new System.Drawing.Point(5, 19);
            this.txtDebugMessages.Margin = new System.Windows.Forms.Padding(2);
            this.txtDebugMessages.Multiline = true;
            this.txtDebugMessages.Name = "txtDebugMessages";
            this.txtDebugMessages.ReadOnly = true;
            this.txtDebugMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDebugMessages.Size = new System.Drawing.Size(449, 289);
            this.txtDebugMessages.TabIndex = 0;
            // 
            // SimVar
            // 
            this.SimVar.HeaderText = "SimVar Name";
            this.SimVar.Name = "SimVar";
            this.SimVar.ReadOnly = true;
            this.SimVar.Width = 200;
            // 
            // Value
            // 
            this.Value.HeaderText = "Latest Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // Updated
            // 
            this.Updated.HeaderText = "Updated";
            this.Updated.Name = "Updated";
            this.Updated.ReadOnly = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 346);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.Text = "Remote Cockpit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabCockpitWindow.ResumeLayout(false);
            this.grpCockpit.ResumeLayout(false);
            this.grpCockpit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCockpitHeight)).EndInit();
            this.tabValues.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgValues)).EndInit();
            this.tabDebug.ResumeLayout(false);
            this.tabDebug.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCockpitWindow;
        private System.Windows.Forms.GroupBox grpCockpit;
        private System.Windows.Forms.CheckBox cbAutoCockpitLayout;
        private System.Windows.Forms.Label lblCockpitLayout;
        private System.Windows.Forms.ComboBox cmbCockpitLayout;
        private System.Windows.Forms.Button pbShowCockpit;
        private System.Windows.Forms.CheckBox cbCockpitCentre;
        private System.Windows.Forms.Label lblCentre;
        private System.Windows.Forms.NumericUpDown txtCockpitLeft;
        private System.Windows.Forms.NumericUpDown txtCockpitTop;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.NumericUpDown txtCockpitWidth;
        private System.Windows.Forms.NumericUpDown txtCockpitHeight;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.CheckBox cbFullScreen;
        private System.Windows.Forms.Label lblFullScreen;
        private System.Windows.Forms.TabPage tabDebug;
        private System.Windows.Forms.CheckBox cbConnected;
        private System.Windows.Forms.Label lblServerConnected;
        private System.Windows.Forms.CheckBox cbFSRunning;
        private System.Windows.Forms.Label lblFSRunning;
        private System.Windows.Forms.Label lblAutoCockpitLayout;
        private System.Windows.Forms.Label lblDebugMessages;
        private System.Windows.Forms.TextBox txtDebugMessages;
        private System.Windows.Forms.NumericUpDown txtServerPort;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.TextBox txtServerAddress;
        private System.Windows.Forms.Label lblServerAddress;
        private System.Windows.Forms.TabPage tabValues;
        private System.Windows.Forms.DataGridView dgValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimVar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Updated;
    }
}