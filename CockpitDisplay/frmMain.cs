using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using AutoUpdaterDotNET;

namespace CockpitDisplay
{
    public partial class frmMain : Form
    {
        private List<ClientRequestResult> requestResults;
        private RemoteConnector connector;
        private delegate void SafeCallDelegate(object obj, string propertyName, object value);
        private delegate void SafeUpdateDelegate(object sender, string e);
        private string appDataFolder;

        //private ClientRequestResult altitude = new ClientRequestResult { Request = new ClientRequest { Name = "INDICATED ALTITUDE", Unit = "feet" }, Result = 14250 };

        private frmCockpit cockpit;

        public frmMain()
        {
            AutoUpdater.Start("https://dragonlaird.visualstudio.com/_git/RemoteCockpit?path=%2FFSRemoteCockpit.xml");
            InitializeComponent();
            Initialize();
        }

        private void RequestVariable(ClientRequest request)
        {
            if(!requestResults.Any(x=> x.Request.Name == request.Name && x.Request.Unit == request.Unit))
            {
                requestResults.Add(new ClientRequestResult { Request = request, Result = null });
            }
            // Needs to be thread-safe
            UpdateObject(cbConnected, "Checked", connector?.Connected ?? false);
            //cbConnected.Checked = connector?.Connected ?? false;
            if (connector?.Connected == true)
            {
                connector.RequestVariable(request);
            }
        }

        private void RequestAllVariables()
        {
            // Assume FS Connection is not established - the re-request the Connection State
            // If FS Connection returns true, it will automatially resubmit all variable requests
            cbFSRunning.Checked = false;
            RequestVariable(new ClientRequest { Name = "FS CONNECTION", Unit = "bool" });
        }

        private void Initialize()
        {
            try
            {
                appDataFolder = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".\\FS Remote Cockpit"));
                requestResults = new List<RemoteCockpitClasses.ClientRequestResult>();
                var ipAddress = IPAddress.Parse(txtServerAddress.Text);
                var ipPort = (int)txtServerPort.Value;
                connector = new RemoteConnector(new System.Net.IPEndPoint(ipAddress, ipPort));
                connector.LogMessage += DebugMessage;
                connector.ReceiveData += ReceiveResultFromServer;
                connector.Connect();

                var layoutsDefinitionsText = File.ReadAllText(Path.Combine(appDataFolder, @".\Layouts\Layouts.json"));
                var layouts = (JObject)JsonConvert.DeserializeObject(layoutsDefinitionsText);
                cmbCockpitLayout.Items.Clear();
                foreach (var layoutJson in layouts["Layouts"])
                {
                    var layout = layoutJson.ToObject<LayoutDefinition>();
                    cmbCockpitLayout.Items.Add(layout.Name);
                }
                cmbCockpitLayout.SelectedIndex = 0;
                // Always ask to be notified when Connection to FlightSim is made or dropped
                RequestVariable(new ClientRequest
                {
                    Name = "FS CONNECTION",
                    Unit = "bool"
                });
                // Also, ask to be notified whenever user starts flying a different aircraft
                RequestVariable(new ClientRequest
                {
                    Name = "TITLE",
                    Unit = "string"
                });

                RequestVariable(new ClientRequest
                {
                    Name = "UPDATE FREQUENCY",
                    Unit = "millisecond"
                });
                cbConnected.Checked = connector.Connected;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Cockpit: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateValuesGrid(ClientRequestResult requestResult)
        {
            if (dgValues.InvokeRequired)
            {
                MethodInvoker del = delegate { UpdateValuesGrid(requestResult); };
                dgValues.Invoke(del);
                return;
            }
            try
            {
                var row = dgValues.Rows.Cast<DataGridViewRow>()
                    .Where(x => x.Cells["SimVar"] != null && x.Cells["SimVar"].Value?.ToString() == requestResult.Request.Name).FirstOrDefault();

                if (row == null)
                {
                    // Add new row for this SimVar
                    try
                    {
                        row = dgValues.Rows[dgValues.Rows.Add()];
                        row.Cells["SimVar"].Value = requestResult.Request.Name;
                    }
                    catch //(Exception ex)
                    {
                    }
                }
                if (row.Cells["Value"] != null)
                {
                    row.Cells["Value"].Value = requestResult.Result?.ToString();
                }
                if (row.Cells["Updated"] != null)
                {
                    row.Cells["Updated"].Value = requestResult.LastUpdated.TimeOfDay.ToString();
                }
                dgValues.Update();
            }
            catch { }
        }

        private void ReceiveResultFromServer(object sender, ClientRequestResult requestResult)
        {
            string message = string.Format("Value Update: {0}({1}) = {2}", requestResult.Request.Name, requestResult.Request.Unit, requestResult.Result);
            DebugMessage(this, message);

            if (requestResults.Any(x => x.Request.Name == requestResult.Request.Name && x.Request.Unit == requestResult.Request.Unit))
                lock (requestResults)
                {
                    requestResults.First(x => x.Request.Name == requestResult.Request.Name && x.Request.Unit == requestResult.Request.Unit).Result = requestResult.Result;
                }
            else
                requestResults.Add(requestResult);

            UpdateValuesGrid(requestResult);
            // Received a new value for a request - identify which plugins need this variable and send it
            if (requestResult.Request.Name == "FS CONNECTION")
            {
                // Just informing us the current connection state to the Flight Simulator = display it on screen
                var existingConnectionState = this.Controls.Find("cbFSRunning", true);
                foreach (Control connectionStateLabel in existingConnectionState)
                {
                    if ((bool)requestResult.Result)
                    {
                        if (!((CheckBox)existingConnectionState.First()).Checked)
                        {
                            // Re-request all previous variables after reconnect
                            foreach (var variableRequest in requestResults.Where(x => x.Request.Name != "FS CONNECTION"))
                            {
                                RequestVariable(variableRequest.Request);
                            }
                        }
                        UpdateObject(connectionStateLabel, "Checked", true);
                    }
                    else
                        UpdateObject(connectionStateLabel, "Checked", false);
                }
            }
            if (requestResult.Request.Name == "TITLE")
            {
                // New aircraft selected
                if (requestResult.Result != null && cmbCockpitLayout.Items.Contains(requestResult.Result?.ToString()) && cbAutoCockpitLayout.Checked)
                {
                    // User wants to auto-select the aircraft
                    UpdateObject(cmbCockpitLayout, "SelectedIndex", cmbCockpitLayout.Items.IndexOf(requestResult.Result?.ToString()));
                    //cmbCockpitLayout.SelectedIndex = cmbCockpitLayout.Items.IndexOf(requestResult.Result?.ToString());
                    if (cockpit != null)
                        // Cockpit already display - reload with the new layout
                        ReloadCockpit(requestResult.Result.ToString());
                }
            }
            else
            {
                if (cockpit != null)
                {
                    cockpit.ResultUpdate(requestResult);
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
                var layout = requestResults.SingleOrDefault(x => x.Request.Name == "TITLE")?.Result?.ToString() ?? "Generic";
                cmbCockpitLayout.Text = layout;
                if (cockpit != null)
                    ReloadCockpit(cmbCockpitLayout.Text);
            }
            else
            {
                cmbCockpitLayout.Enabled = true;
            }
        }

        private void ReloadCockpit(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = "Generic";
            }
            if(cockpit != null)
            {
                //cockpit.Close();
                cockpit.Dispose();
            }
            cockpit = new frmCockpit();
            cockpit.LogMessage += DebugMessage;
            cockpit.RequestValue += RequestVariable;
            cockpit.FormClosed += Cockpit_Closed;
            if (cbFullScreen.Checked)
            {
                cockpit.WindowState = FormWindowState.Maximized;
                cockpit.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                cockpit.WindowState = FormWindowState.Normal;
                cockpit.Height = (int)txtCockpitHeight.Value;
                cockpit.Width = (int)txtCockpitWidth.Value;
                cockpit.FormBorderStyle = FormBorderStyle.FixedDialog;
                cbCockpitCentre_CheckedChanged(null, null);
            }
            cockpit.Text = string.Format("Cockpit{0}", string.IsNullOrEmpty(text) ? "" : (" - " + text));
            cockpit.Show();
            cockpit.LoadLayout(text); // This should force all viible controls to be removed and re-added with new dimensions
            try
            {
                foreach (var requestResult in requestResults)
                {
                    try
                    {
                        cockpit.ResultUpdate(requestResult);
                    }
                    catch
                    {
                        // If one plugin generates an error, it should not prevent others from being updated
                    }
                }
            }
            catch { }
            RequestAllVariables(); // Resubmit all previous requests, in case server disconnected from FS
            cockpit.Focus();
        }

        private void DebugMessage(object sender, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (txtDebugMessages.InvokeRequired)
                {
                    var d = new SafeUpdateDelegate(DebugMessage);
                    txtDebugMessages.Invoke(d, new[] { sender, message });
                    return;
                }
                txtDebugMessages.Text += string.Format("{0:HH:mm:ss} ({1}) {2}\r\n", DateTime.Now, sender?.GetType().Name ?? "Unknown", message);
                while(txtDebugMessages.Text.Count(x=> x=='\n') > 200)
                {
                    txtDebugMessages.Text = txtDebugMessages.Text.Substring(0, txtDebugMessages.Text.IndexOf('\n') + 1);
                }
                txtDebugMessages.SelectionStart = txtDebugMessages.Text.Length;
                txtDebugMessages.ScrollToCaret();
            }
        }

        private void RequestVariable(object sender, ClientRequest request)
        {
            string message = string.Format("Request Value: {0} ({1})", request.Name, request.Unit);
            DebugMessage(this, message);
            // Has the variable already been requested and received?
            if (requestResults.Any(x => x.Request.Name == request.Name && x.Request.Unit == request.Unit))
            {
                // Yes - just send the latest value, it will be updated automatically
                cockpit.ResultUpdate(requestResults.First(x => x.Request.Name == request.Name && x.Request.Unit == request.Unit));
            }
            else
            {
                // No - submit a new request for this variable
                if (connector != null)
                {
                    connector.RequestVariable(request);
                }
            }
        }

        private void Cockpit_Closed(object sender, FormClosedEventArgs e)
        {
            pbShowCockpit_Click(pbShowCockpit, null);
        }

        private void pbShowCockpit_Click(object sender, EventArgs e)
        {
            var cmdButton = (Button)sender;
            if (cockpit == null)
            {
                cmdButton.Text = "Hide Cockpit";
                ReloadCockpit(cmbCockpitLayout.Text);
                this.Focus();
            }
            else
            {
                cmdButton.Text = "Show Cockpit";
                if (cockpit.Visible)
                {
                    cockpit.Visible = false;
                    cockpit?.Close();
                }
                cockpit?.Dispose();
                cockpit = null;
            }
        }

        private void cbFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox)
                if (((CheckBox)sender).Checked)
                {
                    txtCockpitHeight.Enabled = false;
                    txtCockpitWidth.Enabled = false;
                    txtCockpitHeight.ReadOnly = true;
                    txtCockpitWidth.ReadOnly = true;
                    cbCockpitCentre.Enabled = false;
                }
                else
                {
                    txtCockpitHeight.Enabled = true;
                    txtCockpitWidth.Enabled = true;
                    txtCockpitHeight.ReadOnly = false;
                    txtCockpitWidth.ReadOnly = false;
                    cbCockpitCentre.Enabled = true;
                }
            cbCockpitCentre_CheckedChanged(null, null);
            if (cockpit != null)
                ReloadCockpit(cmbCockpitLayout.Text);
        }

        private void cbCockpitCentre_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCockpitCentre.Checked)
            {
                txtCockpitLeft.Enabled = false;
                txtCockpitTop.Enabled = false;
                txtCockpitLeft.ReadOnly = true;
                txtCockpitTop.ReadOnly = true;
                if (cockpit != null)
                {
                    var screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
                    var screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                    var cockpitLeft = (screenWidth - txtCockpitWidth.Value) / 2;
                    var cockpitTop = (screenHeight - txtCockpitHeight.Value) / 2;
                    cockpitLeft += Screen.PrimaryScreen.WorkingArea.Left;
                    cockpitTop += Screen.PrimaryScreen.WorkingArea.Top;
                    cockpit.Top = (int)cockpitTop;
                    cockpit.Left = (int)cockpitLeft;
                }
            }
            else
            {
                txtCockpitLeft.Enabled = true;
                txtCockpitTop.Enabled = true;
                txtCockpitLeft.ReadOnly = false;
                txtCockpitTop.ReadOnly = false;
                if (cockpit != null)
                {
                    cockpit.Top = (int)txtCockpitTop.Value;
                    cockpit.Left = (int)txtCockpitLeft.Value;
                }
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cockpit != null)
                cockpit.Close();
        }
    }
}
