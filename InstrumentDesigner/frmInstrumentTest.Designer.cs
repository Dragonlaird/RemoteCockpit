
namespace InstrumentDesigner
{
    partial class frmInstrumentTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInstrumentTest));
            this.tabInstrument = new System.Windows.Forms.TabControl();
            this.tabSimVar = new System.Windows.Forms.TabPage();
            this.dgSimVarValues = new System.Windows.Forms.DataGridView();
            this.VariableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VariableUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VariableValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.tabInstrument.SuspendLayout();
            this.tabSimVar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSimVarValues)).BeginInit();
            this.tabDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabInstrument
            // 
            this.tabInstrument.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabInstrument.Controls.Add(this.tabSimVar);
            this.tabInstrument.Controls.Add(this.tabDebug);
            this.tabInstrument.Location = new System.Drawing.Point(291, 2);
            this.tabInstrument.Margin = new System.Windows.Forms.Padding(2);
            this.tabInstrument.Name = "tabInstrument";
            this.tabInstrument.SelectedIndex = 0;
            this.tabInstrument.Size = new System.Drawing.Size(346, 315);
            this.tabInstrument.TabIndex = 1;
            // 
            // tabSimVar
            // 
            this.tabSimVar.Controls.Add(this.dgSimVarValues);
            this.tabSimVar.Location = new System.Drawing.Point(4, 22);
            this.tabSimVar.Margin = new System.Windows.Forms.Padding(2);
            this.tabSimVar.Name = "tabSimVar";
            this.tabSimVar.Padding = new System.Windows.Forms.Padding(2);
            this.tabSimVar.Size = new System.Drawing.Size(338, 289);
            this.tabSimVar.TabIndex = 0;
            this.tabSimVar.Text = "Variables";
            this.tabSimVar.UseVisualStyleBackColor = true;
            // 
            // dgSimVarValues
            // 
            this.dgSimVarValues.AllowUserToAddRows = false;
            this.dgSimVarValues.AllowUserToDeleteRows = false;
            this.dgSimVarValues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgSimVarValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSimVarValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VariableName,
            this.VariableUnit,
            this.VariableValue});
            this.dgSimVarValues.Location = new System.Drawing.Point(5, 2);
            this.dgSimVarValues.Margin = new System.Windows.Forms.Padding(2);
            this.dgSimVarValues.Name = "dgSimVarValues";
            this.dgSimVarValues.RowHeadersWidth = 51;
            this.dgSimVarValues.RowTemplate.Height = 24;
            this.dgSimVarValues.Size = new System.Drawing.Size(333, 284);
            this.dgSimVarValues.TabIndex = 1;
            this.dgSimVarValues.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.SimVarValue_Changed);
            // 
            // VariableName
            // 
            this.VariableName.HeaderText = "Name";
            this.VariableName.MinimumWidth = 6;
            this.VariableName.Name = "VariableName";
            this.VariableName.ReadOnly = true;
            this.VariableName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.VariableName.Width = 125;
            // 
            // VariableUnit
            // 
            this.VariableUnit.HeaderText = "Units";
            this.VariableUnit.MinimumWidth = 6;
            this.VariableUnit.Name = "VariableUnit";
            this.VariableUnit.ReadOnly = true;
            // 
            // VariableValue
            // 
            this.VariableValue.HeaderText = "Value";
            this.VariableValue.MinimumWidth = 6;
            this.VariableValue.Name = "VariableValue";
            this.VariableValue.Width = 60;
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.txtDebug);
            this.tabDebug.Location = new System.Drawing.Point(4, 22);
            this.tabDebug.Margin = new System.Windows.Forms.Padding(2);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(2);
            this.tabDebug.Size = new System.Drawing.Size(338, 289);
            this.tabDebug.TabIndex = 1;
            this.tabDebug.Text = "Debug";
            this.tabDebug.UseVisualStyleBackColor = true;
            // 
            // txtDebug
            // 
            this.txtDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDebug.Location = new System.Drawing.Point(5, 5);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.ReadOnly = true;
            this.txtDebug.Size = new System.Drawing.Size(328, 279);
            this.txtDebug.TabIndex = 0;
            // 
            // frmInstrumentTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 327);
            this.Controls.Add(this.tabInstrument);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmInstrumentTest";
            this.Text = "Test Instrument Design";
            this.tabInstrument.ResumeLayout(false);
            this.tabSimVar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSimVarValues)).EndInit();
            this.tabDebug.ResumeLayout(false);
            this.tabDebug.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabInstrument;
        private System.Windows.Forms.TabPage tabSimVar;
        private System.Windows.Forms.TabPage tabDebug;
        private System.Windows.Forms.DataGridView dgSimVarValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn VariableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn VariableUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn VariableValue;
        private System.Windows.Forms.TextBox txtDebug;
    }
}