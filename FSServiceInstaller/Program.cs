using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSServiceInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            var processName = "";
            try
            {
                var currentFolder = Assembly.GetExecutingAssembly().Location;
                var remoteCockpitPath = Path.GetFullPath(Path.Combine(currentFolder, "..\\RemoteCockpit.exe"));
                if (File.Exists(remoteCockpitPath))
                {
                    if (args?.Contains("install") == true || args?.Contains("-install") == true)
                    {
                        processName = "Installing";
                        System.Diagnostics.Process.Start(remoteCockpitPath, "-install");
                    }
                    if (args?.Contains("uninstall") == true || args?.Contains("-uninstall") == true)
                    {
                        processName = "Uninstalling";
                        System.Diagnostics.Process.Start(remoteCockpitPath, "-uninstall");
                    }
                    if (args?.Contains("start") == true || args?.Contains("-start") == true)
                    {
                        processName = "Starting";
                        using (ServiceController service = new ServiceController("FS Cockpit Server"))
                        {
                            service.Start();
                        }
                    }
                    if (args?.Contains("stop") == true || args?.Contains("-stop") == true)
                    {
                        processName = "Stopping";
                        using (ServiceController service = new ServiceController("FS Cockpit Server"))
                        {
                            service.Stop();
                        }
                    }
                }
                else
                    MessageBox.Show(string.Format("RemoteCockpit.exe not found:\rPath: {0}", remoteCockpitPath), "Service Installer Error", MessageBoxButtons.OK);

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error {0} FS Cockpit Service:\r{1}", processName, ex.Message), "FS Remote Cockpit Service Error", MessageBoxButtons.OK);
            }
        }
    }
}
