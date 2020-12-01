using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace CockpitDisplay
{
    public partial class frmMain : Form
    {
        private List<ClientRequestResult> requestResults;
        private RemoteConnector connector;
        private delegate void SafeCallDelegate(object obj, string propertyName, object value);
        private delegate void SafeUpdateDelegate(object sender, string e);

        //private ClientRequestResult altitude = new ClientRequestResult { Request = new ClientRequest { Name = "INDICATED ALTITUDE", Unit = "feet" }, Result = 14250 };

        private frmCockpit cockpit;

        private System.Timers.Timer testTimer;

        public frmMain()
        {
            InitializeComponent();
            Initialize();

            // Only variable needed for this is "TITLE", to be notified whenever the aircraft type changes
            Thread.Sleep(3000);

            TestInstruments();
        }

        private void TestInstruments()
        {
            if(testTimer != null)
            {
                testTimer.Stop();
                testTimer.Dispose();
                testTimer = null;
            }
            // Set it to go off every ten seconds
            testTimer = new System.Timers.Timer(2500);
            // Tell the timer what to do when it elapses
            testTimer.Elapsed += new ElapsedEventHandler(updateInstrumentsForTest);
            // And start it        
            testTimer.Start();

        }

        private void updateInstrumentsForTest(object source, ElapsedEventArgs e)
        {
            if (cockpit != null)
            {
                try
                {
                    var rnd = new Random();
                    var changeAmount = rnd.Next(50);
                    if (rnd.NextDouble() > 0.5)
                        changeAmount = -changeAmount;
                    var testResult = requestResults.FirstOrDefault(x => x.Request.Name == "INDICATED ALTITUDE" && x.Request.Unit == "feet");
                    if (testResult == null)
                    {
                        testResult = new ClientRequestResult { Request = new ClientRequest { Name = "INDICATED ALTITUDE", Unit = "feet" }, Result = (double)-1 };
                        requestResults.Add(testResult);
                        testResult.Result = (double)3000;
                    }
                    else
                    {
                        testResult.Result = (double)testResult.Result + changeAmount;
                    }
                    ReceiveResultFromServer(null, testResult);

                    testResult = requestResults.FirstOrDefault(x => x.Request.Name == "AIRSPEED INDICATED" && x.Request.Unit == "knots");
                    if (testResult == null)
                    {
                        testResult = new ClientRequestResult { Request = new ClientRequest { Name = "AIRSPEED INDICATED", Unit = "knots" }, Result = (double)-1 };
                        requestResults.Add(testResult);
                        testResult.Result = (double)100;
                    }
                    else
                    {
                        changeAmount = rnd.Next(40);
                        if (rnd.NextDouble() > 0.5)
                        {
                            changeAmount = -changeAmount;
                        }
                        testResult.Result = (double)testResult.Result + changeAmount;
                    }
                    ReceiveResultFromServer(null, testResult);

                    testResult = requestResults.FirstOrDefault(x => x.Request.Name == "ATTITUDE INDICATOR BANK DEGREES" && x.Request.Unit == "radians");
                    if (testResult == null)
                    {
                        testResult = new ClientRequestResult { Request = new ClientRequest { Name = "ATTITUDE INDICATOR BANK DEGREES", Unit = "radians" }, Result = (double)-1 };
                        requestResults.Add(testResult);
                        testResult.Result = (double)0;
                    }
                    else
                    {
                        changeAmount = ((int)(double)40);
                        if (rnd.NextDouble() > 0.5)
                        {
                            changeAmount = -changeAmount;
                        }
                        testResult.Result = (double)testResult.Result + changeAmount;
                    }
                    ReceiveResultFromServer(null, testResult);
                }
                catch (Exception ex) { }
            }
        }

        private void RequestVariable(ClientRequest request)
        {
            if(!requestResults.Any(x=> x.Request.Name == request.Name && x.Request.Unit == request.Unit))
            {
                requestResults.Add(new ClientRequestResult { Request = request, Result = null });
            }
            cbConnected.Checked = connector?.Connected ?? false;
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
            RequestVariable(new ClientRequest { Name = "FS CONNECTION", Unit = "second" });
        }

        private void Initialize()
        {
            requestResults = new List<RemoteCockpitClasses.ClientRequestResult>();
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var ipPort = 5555;
            connector = new RemoteConnector(new System.Net.IPEndPoint(ipAddress, ipPort));
            connector.ReceiveData += ReceiveResultFromServer;
            connector.Connect();
            cbConnected.Checked = connector.Connected;

            var layoutsDefinitionsText = File.ReadAllText(@".\Layouts\Layouts.json");
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
                Unit = "second"
            });
        }

        private void ReceiveResultFromServer(object sender, ClientRequestResult requestResult)
        {
            if (requestResults.Any(x => x.Request.Name == requestResult.Request.Name && x.Request.Unit == requestResult.Request.Unit))
                lock (requestResults)
                {
                    requestResults.First(x => x.Request.Name == requestResult.Request.Name && x.Request.Unit == requestResult.Request.Unit).Result = requestResult.Result;
                }
            else
                requestResults.Add(requestResult);

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
            cockpit.RequestValue += RequestVariable;
            cockpit.Messages += DebugMessage;
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
            //cockpit.Update();
            cockpit.Focus();
            this.Focus();
        }

        private void DebugMessage(object sender, string e)
        {
            if (!string.IsNullOrEmpty(e))
            {
                if (txtDebugMessages.InvokeRequired)
                {
                    var d = new SafeUpdateDelegate(DebugMessage);
                    ((Control)txtDebugMessages).Invoke(d, new object[] { sender, e });
                    return;
                }
                txtDebugMessages.Text += string.Format("{0:HH:mm:ss} {1}\r", DateTime.Now, e);
                txtDebugMessages.SelectionStart = txtDebugMessages.Text.Length;
                txtDebugMessages.ScrollToCaret();
            }
        }

        private void RequestVariable(object sender, ClientRequest request)
        {
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
                cockpit?.Close();
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
