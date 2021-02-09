using System.Reflection;
using System.Configuration.Install;
using System;

namespace RemoteCockpitServer
{
    public static class SelfInstaller
    {
        private static readonly string _exePath =
            Assembly.GetExecutingAssembly().Location;
        public static bool InstallMe()//Serilog.Core.Logger log = null)
        {
            try
            {
                //log?.Write(Serilog.Events.LogEventLevel.Information, "Installing RemoteCockpitServer Service, vias SelfInstaller");
                ManagedInstallerClass.InstallHelper(
                    new string[] { _exePath });
            }
            catch(Exception ex)
            {
                //log?.Write(Serilog.Events.LogEventLevel.Error, ex, "Install Failed\rError: {0}");

                return false;
            }
            return true;
        }

        public static bool UninstallMe()//Serilog.Core.Logger log = null)
        {
            try
            {
                //log?.Write(Serilog.Events.LogEventLevel.Information, "Installing RemoteCockpitServer Service, vias SelfInstaller");
                ManagedInstallerClass.InstallHelper(
                    new string[] { "/u", _exePath });
            }
            catch(Exception ex)
            {
                //log?.Write(Serilog.Events.LogEventLevel.Error, ex, "Uninstall Failed\rError: {0}");
                return false;
            }
            return true;
        }
    }
}