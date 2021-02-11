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
        private delegate void SafeUpdateDelegate(object sender, string e);

        public frmInstrumentTest(Configuration config)
        {
            _config = config;
            InitializeComponent();
            LoadInstrument();
        }

        public void LoadInstrument()
        {
            instrument = new Generic_Instrument(300,300,_config);
            instrument.UpdateFrequency = 3;
            instrument.LogMessage += DebugMessage;
            this.Controls.Add(instrument.Control);
            dgSimVarValues.Rows.Clear();
            foreach(var simVar in instrument.RequiredValues.OrderBy(x=> x.Name))
            {
                dgSimVarValues.Rows.Add(simVar.Name, simVar.Unit);
            }
        }

        private void DebugMessage(object sender, string e)
        {
            if (!string.IsNullOrEmpty(e))
            {
                if (txtDebug.InvokeRequired)
                {
                    var d = new SafeUpdateDelegate(DebugMessage);
                    ((Control)txtDebug).Invoke(d, new object[] { sender, e });
                    return;
                }
                txtDebug.Text += string.Format("{0:HH:mm:ss} ({1}) {2}\r\n", DateTime.Now, sender?.GetType().Name ?? "Unknown", e);
                while (txtDebug.Text.Count(x => x == '\n') > 200)
                {
                    txtDebug.Text = txtDebug.Text.Substring(txtDebug.Text.IndexOf('\n') + 1);
                }
                txtDebug.SelectionStart = txtDebug.Text.Length;
                txtDebug.ScrollToCaret();
            }
        }

        private void SimVarValue_Changed(object sender, DataGridViewCellEventArgs e)
        {
            if (e != null && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                var row = dgSimVarValues.Rows[e.RowIndex];
                var val = row.Cells["VariableValue"].Value;
                val = val ?? 0;
                double dblVal;
                ClientRequestResult newValue;
                if (double.TryParse(val.ToString(), out dblVal))
                {
                    newValue = new ClientRequestResult
                    {
                        Request = new ClientRequest
                        {
                            Name = row.Cells["VariableName"].Value?.ToString(),
                            Unit = row.Cells["VariableUnit"].Value?.ToString()
                        },
                        Result = dblVal
                    };
                }
                else
                {
                    DebugMessage(this, string.Format("Invalid value supplied for SimVar: {0} ({1})", row.Cells["VariableName"].Value, val));
                    newValue = new ClientRequestResult
                    {
                        Request = new ClientRequest
                        {
                            Name = row.Cells["VariableName"].Value?.ToString(),
                            Unit = row.Cells["VariableUnit"].Value?.ToString()
                        },
                        Result = 0
                    };
                }
                instrument.ValueUpdate(newValue);
            }
        }
    }
}
