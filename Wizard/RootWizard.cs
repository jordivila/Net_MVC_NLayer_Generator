using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using Wizard;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE80;
using System.Diagnostics;

namespace CustomWizard
{
    public class IWizardImplementation : IWizard
    {
        public static GlobalData GlobalData { get; set; }
        

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            this.FormInfo_CreateExecute();
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

        }

        public void RunFinished()
        {
            this.SolutionEvents_SetStartupProjects();

            IWizardImplementation.GlobalData.LogWriter.Dispose();
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                // Initilize GlobalData to be used by child template projects
                IWizardImplementation.GlobalData = new GlobalData(automationObject, replacementsDictionary, customParams);
                IWizardImplementation.GlobalData.dte = (DTE2)automationObject;
                IWizardImplementation.GlobalData.DirectoryCopy(IWizardImplementation.GlobalData.PackagesDirectoryRepository.FullName, IWizardImplementation.GlobalData.PackagesDirectory.FullName, true);
                IWizardImplementation.GlobalData.dte.Events.SolutionEvents.ProjectAdded += new _dispSolutionEvents_ProjectAddedEventHandler(SolutionEvents_ProjectAdded);


                // Append Custom Dictionary Entries
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.CustomNamespaceKey, IWizardImplementation.GlobalData.CustomNamespace);

                this.FormInfo_FormShow();
            }
            catch (Exception ex)
            {
                IWizardImplementation.GlobalData.LogWriter.Write(new LogMessageModel(ex));
                MessageBox.Show(FormsWizardGeneralResources.UnHandledErrorMoreInfo, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void SolutionEvents_ProjectAdded(Project Project)
        {
            IWizardImplementation.GlobalData.LogWriter.Write(new LogMessageModel(string.Format(FormsWizardGeneralResources.AddingProject, Project.Name),
                                                                                FormsWizardGeneralResources.General,
                                                                                1,
                                                                                1,
                                                                                TraceEventType.Information,
                                                                                FormsWizardGeneralResources.General,
                                                                                null));

            IWizardImplementation.GlobalData.dte.StatusBar.Text = string.Format(FormsWizardGeneralResources.AddingProject, Project.Name);
        }
        protected void SolutionEvents_SetStartupProjects()
        {
            IWizardImplementation.GlobalData.dte.StatusBar.Text = FormsWizardGeneralResources.SettingStartupProjects;
            IWizardImplementation.GlobalData.LogWriter.Write(new LogMessageModel(FormsWizardGeneralResources.SettingStartupProjects,
                                                                                FormsWizardGeneralResources.General,
                                                                                1,
                                                                                1,
                                                                                TraceEventType.Information,
                                                                                FormsWizardGeneralResources.General,
                                                                                null));

            try
            {
                // Set Multiple Startup Projects
                object startupProjectsArray = Array.CreateInstance(typeof(object), 2);
                ((Array)startupProjectsArray).SetValue(string.Format(@"{0}\{0}.UI.Web\{0}.UI.Web.csproj", IWizardImplementation.GlobalData.CustomNamespace), 0);
                ((Array)startupProjectsArray).SetValue(string.Format(@"{0}\{0}.WCF.ServicesHost\{0}.WCF.ServicesHost.csproj", IWizardImplementation.GlobalData.CustomNamespace), 1);

                SolutionBuild currentSolutionBuild = IWizardImplementation.GlobalData.dte.Solution.SolutionBuild;
                currentSolutionBuild.StartupProjects = startupProjectsArray;
            }
            catch (Exception ex)
            {
                IWizardImplementation.GlobalData.LogWriter.Write(new LogMessageModel(ex));
            }
        }


        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }


        #region Web Site Configuration customize

        private void FormInfo_FormShow()
        {
            FormInfo formInfo = new FormInfo();
            formInfo.onCompleted += new FormInfo.CompletedEventHandler(FormInfo_onCompleted);
            formInfo.onCancelled += new FormInfo.CompletedEventHandler(FormInfo_onCancelled);
            formInfo.ShowDialog();
        }

        private void FormInfo_FormRelease(object sender, EventArgs e)
        {
            ((FormInfo)sender).Close();
            ((FormInfo)sender).Dispose();
        }

        protected void FormInfo_onCancelled(object sender, FormInfoEventArgs e)
        {
            this.FormInfo_FormRelease(sender, e);
            IWizardImplementation.GlobalData.WebSiteConfig = e.WebSiteConfig;
        }

        protected void FormInfo_onCompleted(object sender, FormInfoEventArgs e)
        {
            this.FormInfo_FormRelease(sender, e);
            IWizardImplementation.GlobalData.WebSiteConfig = e.WebSiteConfig;
        }

        private void FormInfo_CreateExecute()
        {
            try
            {
                if (IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.CreateDatabaseAccepted)
                {
                    string command = string.Format(@"{0}\{1}.DAL\Scripts\InstallDatabase.bat", IWizardImplementation.GlobalData.SolutionDirectory, IWizardImplementation.GlobalData.CustomNamespace);
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

                    startInfo.RedirectStandardOutput = true;
                    startInfo.UseShellExecute = false;
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
                    startInfo.FileName = command;
                    startInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" ",
                                                IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.ServerName,
                                                IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.MembershipDBName,
                                                IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.LoggingDBName,
                                                IWizardImplementation.GlobalData.WebSiteConfig.WebSiteData.WebSiteAdminEmailAddress,
                                                IWizardImplementation.GlobalData.WebSiteConfig.WebSiteData.WebSiteAdminPassword,
                                                IWizardImplementation.GlobalData.CustomNamespace,
                                                string.Format(@"{0}\{1}.DAL\Scripts\", IWizardImplementation.GlobalData.SolutionDirectory, IWizardImplementation.GlobalData.CustomNamespace));
                    process.StartInfo = startInfo;
                    process.Start();

                    string result = process.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                IWizardImplementation.GlobalData.LogWriter.Write(new LogMessageModel(ex));
            }
        }

        #endregion


    }
}