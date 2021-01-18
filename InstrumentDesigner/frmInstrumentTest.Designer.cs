
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
            this.dgSimVarValues = new System.Windows.Forms.DataGridView();
            this.VariableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VariableUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VariableValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgSimVarValues)).BeginInit();
            this.SuspendLayout();
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
            this.dgSimVarValues.Location = new System.Drawing.Point(389, 12);
            this.dgSimVarValues.Name = "dgSimVarValues";
            this.dgSimVarValues.RowHeadersWidth = 51;
            this.dgSimVarValues.RowTemplate.Height = 24;
            this.dgSimVarValues.Size = new System.Drawing.Size(431, 378);
            this.dgSimVarValues.TabIndex = 0;
            this.dgSimVarValues.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.SimVarValue_Changed);
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
            this.VariableValue.Width = 125;
            // 
            // frmInstrumentTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 403);
            this.Controls.Add(this.dgSimVarValues);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmInstrumentTest";
            this.Text = "Test Instrument Design";
            ((System.ComponentModel.ISupportInitialize)(this.dgSimVarValues)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgSimVarValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn VariableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn VariableUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn VariableValue;
    }
}