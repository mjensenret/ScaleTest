namespace ScaleIntegrationService
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
            this.scaleServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.scaleServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // scaleServiceProcessInstaller
            // 
            this.scaleServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.scaleServiceProcessInstaller.Password = null;
            this.scaleServiceProcessInstaller.Username = null;
            // 
            // scaleServiceInstaller
            // 
            this.scaleServiceInstaller.Description = "Service used to integrate with the scale kiosk";
            this.scaleServiceInstaller.DisplayName = "Scale Integration Service";
            this.scaleServiceInstaller.ServiceName = "ScaleIntegrationService";
            this.scaleServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.scaleServiceProcessInstaller,
            this.scaleServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller scaleServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller scaleServiceInstaller;
    }
}