using InstrumentPlugins;
using RemoteCockpitClasses;
using RemoteCockpitClasses.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstrumentDesigner
{
    public partial class frmInstrumentTest : Form
    {
        private readonly Configuration _config;
        private Generic_Instrument instrument;
        public frmInstrumentTest(Configuration config)
        {
            _config = config;
            InitializeComponent();
            LoadInstrument();
        }

        public void LoadInstrument()
        {
            instrument = new Generic_Instrument(300,300,_config);
            this.Controls.Add(instrument.Control);
            dgSimVarValues.Rows.Clear();
            foreach(var simVar in instrument.RequiredValues.OrderBy(x=> x.Name))
            {
                dgSimVarValues.Rows.Add(simVar.Name, simVar.Unit);
            }
        }

        private void SimVarValue_Changed(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgSimVarValues.Rows[e.RowIndex];
            if (row.Cells["VariableValue"].Value == null)
                row.Cells["VariableValue"].Value = 0;
            var newValue = new ClientRequestResult
            {
                Request = new ClientRequest
                {
                    Name = row.Cells["VariableName"].Value?.ToString(),
                    Unit = row.Cells["VariableUnit"].Value?.ToString()
                },
                Result = double.Parse(row.Cells["VariableValue"].Value.ToString())
            };
            instrument.ValueUpdate(newValue);
        }
    }
}
