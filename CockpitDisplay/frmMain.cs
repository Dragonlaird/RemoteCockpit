﻿using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace CockpitDisplay
{
    public partial class frmMain : Form
    {
        private List<ClientRequestResult> requestResults;
        private RemoteConnector connector;
        private delegate void SafeCallDelegate(object obj, string propertyName, object value);


        private ClientRequestResult altitude = new ClientRequestResult { Request = new ClientRequest { Name = "INDICATED ALTITUDE", Unit = "feet" }, Result = 1250 };

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

            var myTimer = new System.Timers.Timer();
            // Tell the timer what to do when it elapses
            myTimer.Elapsed += new ElapsedEventHandler(updateAlitmeter);
            // Set it to go off every five seconds
            myTimer.Interval = 500;
            // And start it        
            myTimer.Enabled = true;

        }

        private void updateAlitmeter(object source, ElapsedEventArgs e)
        {
            if (cockpit != null)
            {
                var rnd = new Random();
                var changeAmount = rnd.Next(20);
                if (rnd.NextDouble() > 0.5)
                    changeAmount = -1 * changeAmount;
                altitude.Result = double.Parse(altitude.Result?.ToString()) + changeAmount;
                cockpit.ResultUpdate(altitude);
            }
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
            requestResults = new List<RemoteCockpitClasses.ClientRequestResult>();
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var ipPort = 5555;
            connector = new RemoteConnector(new System.Net.IPEndPoint(ipAddress, ipPort));
            connector.ReceiveData += ReceiveResultFromServer;
            connector.Connect();
            cbConnected.Checked = connector.Connected;
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
                        UpdateObject(connectionStateLabel, "Checked", true);
                    else
                        UpdateObject(connectionStateLabel, "Checked", false);
                }
            }
            if(cockpit != null)
            {
                cockpit.ResultUpdate(requestResult);
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
                cmbCockpitLayout.Text = requestResults.SingleOrDefault(x => x.Request.Name == "TITLE")?.Result?.ToString();
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
                text = "Cessna 152 ASOBO";
            }
            if (cockpit == null)
            {
                cockpit = new frmCockpit();
                cockpit.RequestValue += RequestVariable;
            }
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
            foreach (var requestResult in requestResults)
            {
                cockpit.ResultUpdate(requestResult);
            }
            //cockpit.Update();
            cockpit.Focus();
            this.Focus();
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
