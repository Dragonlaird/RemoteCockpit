using System;

namespace InstrumentDesigner
{
    partial class frmInstrumentDesigner
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
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItemSep = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.gpBasicInfo = new System.Windows.Forms.GroupBox();
            this.lblAircraft = new System.Windows.Forms.Label();
            this.dgAircraft = new System.Windows.Forms.DataGridView();
            this.Aircraft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtCreateDate = new System.Windows.Forms.Label();
            this.lblCreateDate = new System.Windows.Forms.Label();
            this.txtUpdateMS = new System.Windows.Forms.TextBox();
            this.lblUpdateMS = new System.Windows.Forms.Label();
            this.cmbInstrumentType = new System.Windows.Forms.ComboBox();
            this.lblInstrumentType = new System.Windows.Forms.Label();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.txtInstrumentName = new System.Windows.Forms.TextBox();
            this.lblInstrumentName = new System.Windows.Forms.Label();
            this.gpBackground = new System.Windows.Forms.GroupBox();
            this.cmdLoadBackground = new System.Windows.Forms.Button();
            this.cmdClearBackground = new System.Windows.Forms.Button();
            this.txtBackgroundPath = new System.Windows.Forms.TextBox();
            this.lblBackgroundPath = new System.Windows.Forms.Label();
            this.pbBackgroundImage = new System.Windows.Forms.PictureBox();
            this.mnuMain.SuspendLayout();
            this.gpBasicInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAircraft)).BeginInit();
            this.gpBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundImage)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuBackground});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(974, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.fileToolStripMenuItemSep,
            this.exitToolStripMenuItem});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.LoadConfig);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            // 
            // fileToolStripMenuItemSep
            // 
            this.fileToolStripMenuItemSep.Name = "fileToolStripMenuItemSep";
            this.fileToolStripMenuItemSep.Size = new System.Drawing.Size(111, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.CloseForm);
            // 
            // mnuBackground
            // 
            this.mnuBackground.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.importImageToolStripMenuItem,
            this.resizeToolStripMenuItem});
            this.mnuBackground.Name = "mnuBackground";
            this.mnuBackground.Size = new System.Drawing.Size(83, 20);
            this.mnuBackground.Text = "&Background";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.clearToolStripMenuItem.Text = "C&lear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.cmdClearBackground_Click);
            // 
            // importImageToolStripMenuItem
            // 
            this.importImageToolStripMenuItem.Name = "importImageToolStripMenuItem";
            this.importImageToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.importImageToolStripMenuItem.Text = "&Load Image";
            this.importImageToolStripMenuItem.Click += new System.EventHandler(this.cmdLoadBackground_Click);
            // 
            // resizeToolStripMenuItem
            // 
            this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
            this.resizeToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.resizeToolStripMenuItem.Text = "&Resize";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Instrument Configurations|*.json";
            // 
            // gpBasicInfo
            // 
            this.gpBasicInfo.Controls.Add(this.lblAircraft);
            this.gpBasicInfo.Controls.Add(this.dgAircraft);
            this.gpBasicInfo.Controls.Add(this.txtCreateDate);
            this.gpBasicInfo.Controls.Add(this.lblCreateDate);
            this.gpBasicInfo.Controls.Add(this.txtUpdateMS);
            this.gpBasicInfo.Controls.Add(this.lblUpdateMS);
            this.gpBasicInfo.Controls.Add(this.cmbInstrumentType);
            this.gpBasicInfo.Controls.Add(this.lblInstrumentType);
            this.gpBasicInfo.Controls.Add(this.txtAuthor);
            this.gpBasicInfo.Controls.Add(this.lblAuthor);
            this.gpBasicInfo.Controls.Add(this.txtInstrumentName);
            this.gpBasicInfo.Controls.Add(this.lblInstrumentName);
            this.gpBasicInfo.Location = new System.Drawing.Point(12, 28);
            this.gpBasicInfo.Name = "gpBasicInfo";
            this.gpBasicInfo.Size = new System.Drawing.Size(366, 396);
            this.gpBasicInfo.TabIndex = 6;
            this.gpBasicInfo.TabStop = false;
            this.gpBasicInfo.Text = "Instrument Details";
            // 
            // lblAircraft
            // 
            this.lblAircraft.AutoSize = true;
            this.lblAircraft.Location = new System.Drawing.Point(49, 165);
            this.lblAircraft.Name = "lblAircraft";
            this.lblAircraft.Size = new System.Drawing.Size(43, 13);
            this.lblAircraft.TabIndex = 17;
            this.lblAircraft.Text = "Aircraft:";
            // 
            // dgAircraft
            // 
            this.dgAircraft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAircraft.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Aircraft});
            this.dgAircraft.Location = new System.Drawing.Point(108, 160);
            this.dgAircraft.Name = "dgAircraft";
            this.dgAircraft.Size = new System.Drawing.Size(252, 230);
            this.dgAircraft.TabIndex = 16;
            // 
            // Aircraft
            // 
            this.Aircraft.HeaderText = "Aircraft";
            this.Aircraft.Name = "Aircraft";
            this.Aircraft.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Aircraft.Width = 200;
            // 
            // txtCreateDate
            // 
            this.txtCreateDate.AutoSize = true;
            this.txtCreateDate.Location = new System.Drawing.Point(103, 138);
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Size = new System.Drawing.Size(0, 13);
            this.txtCreateDate.TabIndex = 15;
            // 
            // lblCreateDate
            // 
            this.lblCreateDate.AutoSize = true;
            this.lblCreateDate.Location = new System.Drawing.Point(49, 138);
            this.lblCreateDate.Name = "lblCreateDate";
            this.lblCreateDate.Size = new System.Drawing.Size(47, 13);
            this.lblCreateDate.TabIndex = 14;
            this.lblCreateDate.Text = "Created:";
            // 
            // txtUpdateMS
            // 
            this.txtUpdateMS.Location = new System.Drawing.Point(103, 105);
            this.txtUpdateMS.Name = "txtUpdateMS";
            this.txtUpdateMS.Size = new System.Drawing.Size(100, 20);
            this.txtUpdateMS.TabIndex = 13;
            this.txtUpdateMS.TextChanged += new System.EventHandler(this.UpdateMS_Changed);
            // 
            // lblUpdateMS
            // 
            this.lblUpdateMS.AutoSize = true;
            this.lblUpdateMS.Location = new System.Drawing.Point(32, 109);
            this.lblUpdateMS.Name = "lblUpdateMS";
            this.lblUpdateMS.Size = new System.Drawing.Size(64, 13);
            this.lblUpdateMS.TabIndex = 12;
            this.lblUpdateMS.Text = "Update MS:";
            // 
            // cmbInstrumentType
            // 
            this.cmbInstrumentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInstrumentType.FormattingEnabled = true;
            this.cmbInstrumentType.Location = new System.Drawing.Point(103, 76);
            this.cmbInstrumentType.Name = "cmbInstrumentType";
            this.cmbInstrumentType.Size = new System.Drawing.Size(143, 21);
            this.cmbInstrumentType.TabIndex = 11;
            this.cmbInstrumentType.SelectedIndexChanged += new System.EventHandler(this.InstrumentType_Changed);
            // 
            // lblInstrumentType
            // 
            this.lblInstrumentType.AutoSize = true;
            this.lblInstrumentType.Location = new System.Drawing.Point(62, 80);
            this.lblInstrumentType.Name = "lblInstrumentType";
            this.lblInstrumentType.Size = new System.Drawing.Size(34, 13);
            this.lblInstrumentType.TabIndex = 10;
            this.lblInstrumentType.Text = "Type:";
            // 
            // txtAuthor
            // 
            this.txtAuthor.Location = new System.Drawing.Point(103, 47);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new System.Drawing.Size(252, 20);
            this.txtAuthor.TabIndex = 9;
            this.txtAuthor.TextChanged += new System.EventHandler(this.InstrumentAuthor_Changed);
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(55, 51);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(41, 13);
            this.lblAuthor.TabIndex = 8;
            this.lblAuthor.Text = "Author:";
            // 
            // txtInstrumentName
            // 
            this.txtInstrumentName.Location = new System.Drawing.Point(103, 18);
            this.txtInstrumentName.Name = "txtInstrumentName";
            this.txtInstrumentName.Size = new System.Drawing.Size(252, 20);
            this.txtInstrumentName.TabIndex = 7;
            this.txtInstrumentName.TextChanged += new System.EventHandler(this.InstrumentName_Changed);
            // 
            // lblInstrumentName
            // 
            this.lblInstrumentName.AutoSize = true;
            this.lblInstrumentName.Location = new System.Drawing.Point(6, 22);
            this.lblInstrumentName.Name = "lblInstrumentName";
            this.lblInstrumentName.Size = new System.Drawing.Size(90, 13);
            this.lblInstrumentName.TabIndex = 6;
            this.lblInstrumentName.Text = "Instrument Name:";
            // 
            // gpBackground
            // 
            this.gpBackground.Controls.Add(this.cmdLoadBackground);
            this.gpBackground.Controls.Add(this.cmdClearBackground);
            this.gpBackground.Controls.Add(this.txtBackgroundPath);
            this.gpBackground.Controls.Add(this.lblBackgroundPath);
            this.gpBackground.Controls.Add(this.pbBackgroundImage);
            this.gpBackground.Location = new System.Drawing.Point(395, 28);
            this.gpBackground.Name = "gpBackground";
            this.gpBackground.Size = new System.Drawing.Size(297, 396);
            this.gpBackground.TabIndex = 7;
            this.gpBackground.TabStop = false;
            this.gpBackground.Text = "Background";
            // 
            // cmdLoadBackground
            // 
            this.cmdLoadBackground.Location = new System.Drawing.Point(9, 81);
            this.cmdLoadBackground.Name = "cmdLoadBackground";
            this.cmdLoadBackground.Size = new System.Drawing.Size(75, 23);
            this.cmdLoadBackground.TabIndex = 9;
            this.cmdLoadBackground.Text = "Load...";
            this.cmdLoadBackground.UseVisualStyleBackColor = true;
            this.cmdLoadBackground.Click += new System.EventHandler(this.cmdLoadBackground_Click);
            // 
            // cmdClearBackground
            // 
            this.cmdClearBackground.Location = new System.Drawing.Point(9, 51);
            this.cmdClearBackground.Name = "cmdClearBackground";
            this.cmdClearBackground.Size = new System.Drawing.Size(75, 23);
            this.cmdClearBackground.TabIndex = 8;
            this.cmdClearBackground.Text = "Clear...";
            this.cmdClearBackground.UseVisualStyleBackColor = true;
            this.cmdClearBackground.Click += new System.EventHandler(this.cmdClearBackground_Click);
            // 
            // txtBackgroundPath
            // 
            this.txtBackgroundPath.Enabled = false;
            this.txtBackgroundPath.Location = new System.Drawing.Point(86, 18);
            this.txtBackgroundPath.Name = "txtBackgroundPath";
            this.txtBackgroundPath.Size = new System.Drawing.Size(205, 20);
            this.txtBackgroundPath.TabIndex = 7;
            // 
            // lblBackgroundPath
            // 
            this.lblBackgroundPath.AutoSize = true;
            this.lblBackgroundPath.Location = new System.Drawing.Point(6, 22);
            this.lblBackgroundPath.Name = "lblBackgroundPath";
            this.lblBackgroundPath.Size = new System.Drawing.Size(74, 13);
            this.lblBackgroundPath.TabIndex = 6;
            this.lblBackgroundPath.Text = "Relative Path:";
            // 
            // pbBackgroundImage
            // 
            this.pbBackgroundImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbBackgroundImage.Location = new System.Drawing.Point(6, 118);
            this.pbBackgroundImage.Name = "pbBackgroundImage";
            this.pbBackgroundImage.Size = new System.Drawing.Size(285, 272);
            this.pbBackgroundImage.TabIndex = 5;
            this.pbBackgroundImage.TabStop = false;
            // 
            // frmInstrumentDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 444);
            this.Controls.Add(this.gpBackground);
            this.Controls.Add(this.gpBasicInfo);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmInstrumentDesigner";
            this.Text = "Instrument Designer";
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.gpBasicInfo.ResumeLayout(false);
            this.gpBasicInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAircraft)).EndInit();
            this.gpBackground.ResumeLayout(false);
            this.gpBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackgroundImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator fileToolStripMenuItemSep;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuBackground;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importImageToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox gpBasicInfo;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.TextBox txtInstrumentName;
        private System.Windows.Forms.Label lblInstrumentName;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.ComboBox cmbInstrumentType;
        private System.Windows.Forms.Label lblInstrumentType;
        private System.Windows.Forms.Label lblUpdateMS;
        private System.Windows.Forms.Label txtCreateDate;
        private System.Windows.Forms.Label lblCreateDate;
        private System.Windows.Forms.TextBox txtUpdateMS;
        private System.Windows.Forms.GroupBox gpBackground;
        private System.Windows.Forms.PictureBox pbBackgroundImage;
        private System.Windows.Forms.TextBox txtBackgroundPath;
        private System.Windows.Forms.Label lblBackgroundPath;
        private System.Windows.Forms.Button cmdLoadBackground;
        private System.Windows.Forms.Button cmdClearBackground;
        private System.Windows.Forms.DataGridView dgAircraft;
        private System.Windows.Forms.DataGridViewTextBoxColumn Aircraft;
        private System.Windows.Forms.Label lblAircraft;
    }
}

