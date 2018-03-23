namespace BNE.Services.AsyncExecutor
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
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServiceAsyncExecutorInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;
            this.serviceProcessInstaller.Committed += new System.Configuration.Install.InstallEventHandler(this.ServiceProcessInstaller_Committed);
            this.serviceProcessInstaller.BeforeInstall += new System.Configuration.Install.InstallEventHandler(this.ServiceProcessInstaller_BeforeInstall);
            // 
            // ServiceAsyncExecutorInstaller
            // 
            this.ServiceAsyncExecutorInstaller.DisplayName = "BNE.Services.AsyncExecutor";
            this.ServiceAsyncExecutorInstaller.ServiceName = "BNE.Services.AsyncExecutor";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.ServiceAsyncExecutorInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ServiceAsyncExecutorInstaller;
    }
}