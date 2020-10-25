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
            this.SuspendLayout();
            // 
            // lblFSRunning
            // 
            this.lblFSRunning.AutoSize = true;
            this.lblFSRunning.Location = new System.Drawing.Point(13, 56);
            this.lblFSRunning.Name = "lblFSRunning";
            this.lblFSRunning.Size = new System.Drawing.Size(98, 20);
            this.lblFSRunning.TabIndex = 0;
            this.lblFSRunning.Text = "FS Running:";
            // 
            // lblServerConnected
            // 
            this.lblServerConnected.AutoSize = true;
            this.lblServerConnected.Location = new System.Drawing.Point(20, 13);
            this.lblServerConnected.Name = "lblServerConnected";
            this.lblServerConnected.Size = new System.Drawing.Size(91, 20);
            this.lblServerConnected.TabIndex = 1;
            this.lblServerConnected.Text = "Connected:";
            // 
            // cbConnected
            // 
            this.cbConnected.AutoSize = true;
            this.cbConnected.Enabled = false;
            this.cbConnected.Location = new System.Drawing.Point(117, 13);
            this.cbConnected.Name = "cbConnected";
            this.cbConnected.Size = new System.Drawing.Size(22, 21);
            this.cbConnected.TabIndex = 2;
            this.cbConnected.UseVisualStyleBackColor = true;
            // 
            // cbFSRunning
            // 
            this.cbFSRunning.AutoSize = true;
            this.cbFSRunning.Enabled = false;
            this.cbFSRunning.Location = new System.Drawing.Point(117, 56);
            this.cbFSRunning.Name = "cbFSRunning";
            this.cbFSRunning.Size = new System.Drawing.Size(22, 21);
            this.cbFSRunning.TabIndex = 3;
            this.cbFSRunning.UseVisualStyleBackColor = true;
            // 
            // lblCockpitLayout
            // 
            this.lblCockpitLayout.AutoSize = true;
            this.lblCockpitLayout.Location = new System.Drawing.Point(50, 104);
            this.lblCockpitLayout.Name = "lblCockpitLayout";
            this.lblCockpitLayout.Size = new System.Drawing.Size(61, 20);
            this.lblCockpitLayout.TabIndex = 4;
            this.lblCockpitLayout.Text = "Layout:";
            // 
            // cmbCockpitLayout
            // 
            this.cmbCockpitLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCockpitLayout.FormattingEnabled = true;
            this.cmbCockpitLayout.Location = new System.Drawing.Point(117, 101);
            this.cmbCockpitLayout.Name = "cmbCockpitLayout";
            this.cmbCockpitLayout.Size = new System.Drawing.Size(403, 28);
            this.cmbCockpitLayout.TabIndex = 5;
            // 
            // lblAutoCockpitLayout
            // 
            this.lblAutoCockpitLayout.AutoSize = true;
            this.lblAutoCockpitLayout.Location = new System.Drawing.Point(11, 149);
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
            this.pbShowCockpit.Location = new System.Drawing.Point(24, 195);
            this.pbShowCockpit.Name = "pbShowCockpit";
            this.pbShowCockpit.Size = new System.Drawing.Size(169, 36);
            this.pbShowCockpit.TabIndex = 8;
            this.pbShowCockpit.Text = "Show Cockpit";
            this.pbShowCockpit.UseVisualStyleBackColor = true;
            this.pbShowCockpit.Click += new System.EventHandler(this.pbShowCockpit_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 273);
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
    }
}