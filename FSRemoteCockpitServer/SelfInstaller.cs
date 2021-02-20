using System.Reflection;
using System.Configuration.Install;
using System;

namespace RemoteCockpitServer
{
    /// <summary>
    /// Self-Install for Windows Service
    /// </summary>
    public static class SelfInstaller
    {
        private static readonly string _exePath =
            Assembly.GetExecutingAssembly().Location;
        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(
                    new string[] { _exePath });
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(
                    new string[] { "/u", _exePath });
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}