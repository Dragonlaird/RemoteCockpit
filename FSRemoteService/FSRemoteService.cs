using RemoteCockpit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using RemoteCockpitClasses;

namespace FSRemoteService
{
    public partial class fsRemoteService : ServiceBase
    {
        private readonly EventLog eventLog = null;
        private FSCockpitServer server;
        public fsRemoteService()
        {
            InitializeComponent();
            try
            {
                eventLog = new System.Diagnostics.EventLog();
                // Turn off autologging
                this.AutoLog = false;
                // create an event source, specifying the name of a log that
                // does not currently exist to create a new, custom log
                try
                {
                    System.Diagnostics.EventLog.CreateEventSource("FS Remote Server", "Application");
                }
                catch
                {
                    // Can fail when attempting to search the Security Log
                    // This would require manual creation of the Event Log Source
                    // Will also fail if Source already exists, no further action is required for this scenario
                }
                // configure the event log instance to use this source name
                eventLog.Source = "FS Remote Server";
                eventLog.Log = "Application";
            }
            catch { }
        }

        protected override void OnStart(string[] args)
        {
            if(server == null)
            {
                try
                {
                    server = new FSCockpitServer();
                    server.LogReceived += WriteLog;
                    server.Start();
                }
                catch { }
            }
        }

        protected override void OnStop()
        {
            try
            {
                server?.Stop();
            }
            catch { }
        }

        private void WriteLog(object source, LogMessage message)
        {
            try
            {
                if (message?.Message != null)
                    eventLog.WriteEntry(message.Message, message.Type);
            }
            catch { }
        }
    }
}
