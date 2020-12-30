using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RemoteCockpitClasses;
using Serilog;

namespace RemoteCockpit
{
    class Program
    {
        static void Main(string[] args)
        {
            var logConfig = new LoggerConfiguration();
            logConfig.WriteTo.Console()
                    .WriteTo.EventLog("FS Remote Cockpit", "Application");
            if (args.Contains("logtofile"))
            {
                logConfig.WriteTo.File(Path.Combine(Path.GetTempPath(), string.Format("FSCockpitServer_{0:yyMMdd}", DateTime.Now))).ToString();
            }
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FSCockpitServer(logConfig.CreateLogger())
            };
            ServiceBase.Run(ServicesToRun);
        }

        static void WriteLog(object sender, LogMessage message)
        {
            var strType = message.Type.ToString().Substring(0, 3).ToUpper();
            Console.WriteLine("({1}) [{0}] {2}", strType, sender.GetType().Name, message.Message);
        }
    }
}
