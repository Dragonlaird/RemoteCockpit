using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CockpitDisplay
{
    public partial class frmMain : Form
    {
        private List<RemoteCockpitClasses.SimVarRequestResult> requestResults;
        private RemoteConnector connector;
        private delegate void SafeCallDelegate(object obj, string propertyName, object value);


        private frmCockpit cockpit;
        public frmMain()
        {
            InitializeComponent();
            Initialize();

            // Only variable needed for this is "TITLE", to be notified whenever the aircraft type changes
            Thread.Sleep(3000);
            // Always ask to be notified when Connection to FlightSim is made or dropped
            RequestVariable(new ClientRequest
            {
                Name = "FS CONNECTION",
                Unit = "bool"
            });
            // Also, ask to be notified whenever user starts flying a different aircraft
            RequestVariable(new ClientRequest
            {
                Name = "TITLE"
            });

            // Requesting more variables to test
            RequestVariable(new ClientRequest
            {
                Name = Name = "AMBIENT WIND VELOCITY",
            });
            RequestVariable(new ClientRequest
            {
                Name = Name = "AMBIENT WIND DIRECTION",
            });
        }

        private void RequestVariable(ClientRequest request)
        {
            cbConnected.Checked = connector?.Connected ?? false;
            if (connector?.Connected == true)
            {
                connector.RequestVariable(request);
            }
        }

        private void Initialize()
        {
            requestResults = new List<RemoteCockpitClasses.SimVarRequestResult>();
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var ipPort = 5555;
            connector = new RemoteConnector(new System.Net.IPEndPoint(ipAddress, ipPort));
            connector.ReceiveData += ReceiveResultFromServer;
            connector.Connect();
            cbConnected.Checked = connector.Connected;
        }

        private void ReceiveResultFromServer(object sender, ClientRequestResult requestResult)
        {
            // Received a new value for a request - identify which plugins need this variable and send it
            if (requestResult.Request.Name == "FS CONNECTION")
            {
                // Just informing us the current connection state to the Flight Simulator = display it on screen
                var existingConnectionState = this.Controls.Find("cbFSRunning", true);
                foreach (Control connectionStateLabel in existingConnectionState)
                {
                    if ((bool)requestResult.Result)
                        UpdateObject(connectionStateLabel, "Checked", true);
                    else
                        UpdateObject(connectionStateLabel, "Checked", false);
                }
            }
        }

        private void UpdateObject(object obj, string propertyName, object value)
        {
            if (obj != null && !string.IsNullOrEmpty(propertyName) && obj.GetType().GetProperty(propertyName) != null)
            {
                if (((Control)obj).InvokeRequired)
                {
                    var d = new SafeCallDelegate(UpdateObject);
                    ((Control)obj).Invoke(d, new object[] { obj, propertyName, value });
                    return;
                }
                if (obj.GetType().GetProperty(propertyName) != null)
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, value);
                    ((Control)obj).Update();
                }
            }
        }

        private void cbAutoCockpitLayout_CheckedChanged(object sender, EventArgs e)
        {
            var cb = (CheckBox)sender;
            if (cb.Checked)
            {
                cmbCockpitLayout.Enabled = false;
                cmbCockpitLayout.Text = requestResults.SingleOrDefault(x => x.Request.Name == "TITLE")?.Value?.ToString();
                ReloadCockpit(cmbCockpitLayout.Text);
            }
            else
            {
                cmbCockpitLayout.Enabled = true;
            }
        }

        private void ReloadCockpit(string text)
        {
            if (cockpit == null)
            {
                cockpit = new frmCockpit();
                cockpit.Show();
            }
            cockpit.LoadLayout(text);
            cockpit.Update();
        }

        private void pbShowCockpit_Click(object sender, EventArgs e)
        {
            var cmdButton = (Button)sender;
            if (cockpit == null)
            {
                cmdButton.Text = "Hide Cockpit";
                ReloadCockpit(cmbCockpitLayout.Text);
            }
            else
            {
                cmdButton.Text = "Show Cockpit";
                cockpit?.Close();
                cockpit?.Dispose();
                cockpit = null;
            }
        }
    }
}
