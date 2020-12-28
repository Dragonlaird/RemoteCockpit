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
        FSCockpitServer server;
        public fsRemoteService()
        {
            InitializeComponent();
            this.eventLogger = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLogger)).BeginInit();
        }

        protected override void OnStart(string[] args)
        {
            if(server == null)
            {
                server = new FSCockpitServer();
                server.LogReceived += WriteLog;
                server.Start();
            }
        }

        protected override void OnStop()
        {
            server?.Stop();
        }

        private void WriteLog(object source, LogMessage message)
        {
            eventLogger.WriteEntry(message.Message, message.Type);
        }
    }
}
