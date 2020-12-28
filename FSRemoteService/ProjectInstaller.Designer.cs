
namespace FSRemoteService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fsRemoteServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.fsRemoteServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // fsRemoteServiceProcessInstaller
            // 
            this.fsRemoteServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.fsRemoteServiceProcessInstaller.Password = null;
            this.fsRemoteServiceProcessInstaller.Username = null;
            // 
            // fsRemoteServiceInstaller
            // 
            this.fsRemoteServiceInstaller.ServiceName = "FS Remote Cockpit Server";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.fsRemoteServiceProcessInstaller,
            this.fsRemoteServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller fsRemoteServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller fsRemoteServiceInstaller;
    }
}