using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteCockpit
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        //public override void Install(IDictionary stateSaver)
        //{
        //    base.Install(stateSaver);
        //    if(MessageBox.Show("Do you want to install FS Remote Cockpit Server as a Windows Service?", "Install Windows Service?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //    {
        //        // Install project as a Windows Service

        //    }
        //}
    }
}
