using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSServiceInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args?.Contains("install") == true || args?.Contains("-install") == true)
            {
                System.Diagnostics.Process.Start(".\\RemoteCockpit.exe", "-install");
            }
            if (args?.Contains("uninstall") == true || args?.Contains("-uninstall") == true)
            {
                System.Diagnostics.Process.Start(".\\RemoteCockpit.exe", "-uninstall");
            }
        }
    }
}
