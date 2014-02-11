namespace VSIX_MVC_Layered_Wizard
{
    partial class FormInfo
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInfo));
            this.formDatabaseInstallInfoUserControl1 = new VSIX_MVC_Layered_Wizard.FormDatabaseInstallInfoUserControl();
            this.ButtonDev = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonNext = new System.Windows.Forms.Button();
            this.formWcfInfo1 = new VSIX_MVC_Layered_Wizard.FormWcfInfo();
            this.formsWebSiteInfo1 = new VSIX_MVC_Layered_Wizard.FormsWebSiteInfo();
            this.SuspendLayout();
            // 
            // formDatabaseInstallInfoUserControl1
            // 
            resources.ApplyResources(this.formDatabaseInstallInfoUserControl1, "formDatabaseInstallInfoUserControl1");
            this.formDatabaseInstallInfoUserControl1.Name = "formDatabaseInstallInfoUserControl1";
            // 
            // ButtonDev
            // 
            resources.ApplyResources(this.ButtonDev, "ButtonDev");
            this.ButtonDev.Name = "ButtonDev";
            this.ButtonDev.UseVisualStyleBackColor = true;
            this.ButtonDev.Click += new System.EventHandler(this.ButtonDev_Click);
            // 
            // ButtonCancel
            // 
            resources.ApplyResources(this.ButtonCancel, "ButtonCancel");
            this.ButtonCancel.CausesValidation = false;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonNext
            // 
            resources.ApplyResources(this.ButtonNext, "ButtonNext");
            this.ButtonNext.Name = "ButtonNext";
            this.ButtonNext.UseVisualStyleBackColor = true;
            this.ButtonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // formWcfInfo1
            // 
            resources.ApplyResources(this.formWcfInfo1, "formWcfInfo1");
            this.formWcfInfo1.Name = "formWcfInfo1";
            // 
            // formsWebSiteInfo1
            // 
            resources.ApplyResources(this.formsWebSiteInfo1, "formsWebSiteInfo1");
            this.formsWebSiteInfo1.Name = "formsWebSiteInfo1";
            // 
            // FormInfo
            // 
            this.AcceptButton = this.ButtonNext;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.Controls.Add(this.formsWebSiteInfo1);
            this.Controls.Add(this.formWcfInfo1);
            this.Controls.Add(this.ButtonDev);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonNext);
            this.Controls.Add(this.formDatabaseInstallInfoUserControl1);
            this.Name = "FormInfo";
            this.ResumeLayout(false);

        }

        #endregion

        private FormDatabaseInstallInfoUserControl formDatabaseInstallInfoUserControl1;
        private System.Windows.Forms.Button ButtonDev;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonNext;
        private FormWcfInfo formWcfInfo1;
        private FormsWebSiteInfo formsWebSiteInfo1;
    }
}