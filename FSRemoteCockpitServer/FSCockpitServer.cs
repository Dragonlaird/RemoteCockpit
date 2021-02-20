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
using System.Configuration;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace RemoteCockpitServer
{
    /// <summary>
    /// Wrapper for RemoteCockpit to install as a Windows Service
    /// </summary>
    public partial class FSCockpitServer : ServiceBase
    {
        public EventHandler<LogMessage> LogReceived;
        private readonly RemoteServer server;
        private readonly Logger log;
        public FSCockpitServer()
        {
            try
            {
                var logConfig = new LoggerConfiguration();
                logConfig
                    .WriteTo.EventLog("FS Remote Cockpit", "Application");
                log = logConfig.CreateLogger();
                server = new RemoteServer();
                server.LogReceived += WriteLog;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                server.Start();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                server.Stop();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        public bool IsRunning
        {
            get
            {
                return server?.IsRunning ?? false;
            }
        }

        private void WriteLog(Exception ex)
        {
            var msg = ex.Message;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                msg += "\r" + ex.Message;
            }
            WriteLog(msg, LogEventLevel.Error);
        }

        private void WriteLog(string message, LogEventLevel type = LogEventLevel.Information)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (LogReceived != null)
                    WriteLog(this, new LogMessage { Message = message, Type = type });
                if (log != null)
                {
                    try
                    {
                        var msg = string.Format("{0:HH:mm:dd} [{1}] {2}", DateTime.Now, (type + "   ").ToString().Substring(0, 4), message);
                        log.Write(type, msg);
                    }
                    catch { }
                }
            }
        }

        private void WriteLog(object sender, LogMessage e)
        {
            try
            {
                LogReceived.DynamicInvoke(this, e);
            }
            catch { } // Parent class logger has an error - nothing we can do about it here
        }
    }
}
