using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio;
using System.Diagnostics;
using System.Xml;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;
using EnvDTE100;

namespace VSIX_MVC_Layered_Wizard
{
    public class IWizardImplementation : IWizard
    {
        public static GlobalData GlobalData { get; set; }
        

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            this.BackEndHostsAddAsALinkSharedAppConfig();
            this.DatabaseProjects_ReplaceParameters();
        }


        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

        }

        public void RunFinished()
        {
            this.SolutionEvents_GitHelperFiles_Add();
            this.SolutionEvents_SetStartupProjects();
            this.SolutionEvents_CloseAllDocuments();

            IWizardImplementation.GlobalData.LogWriter.Dispose();
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                // Initilize GlobalData to be used by child template projects
                IWizardImplementation.GlobalData = new GlobalData(automationObject, replacementsDictionary, customParams);
                IWizardImplementation.GlobalData.dte = (DTE2)automationObject;
                IWizardImplementation.GlobalData.dte.Events.SolutionEvents.ProjectAdded += new _dispSolutionEvents_ProjectAddedEventHandler(SolutionEvents_ProjectAdded);
                


                // Append Custom Dictionary Entries
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.CustomNamespaceKey, IWizardImplementation.GlobalData.CustomNamespace);
                //IWizardImplementation.GlobalData.DirectoryCopy(IWizardImplementation.GlobalData.PackagesDirectory.FullName, IWizardImplementation.GlobalData.PackagesDirectory.FullName, true);
                this.FormInfo_FormShow();
            }
            catch (Exception ex)
            {
                IWizardImplementation.GlobalData.LogWriter.Write(new LogMessageModel(ex));
                MessageBox.Show(FormsWizardGeneralResources.UnHandledErrorMoreInfo, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SolutionEvents_ProjectAdded(Project Project)
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
        private void SolutionEvents_SetStartupProjects()
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
        private void SolutionEvents_ReplaceParameters(string filePath)
        {
            Dictionary<string, string> replacementsDictionary = GlobalData.ReplacementDictionaryGet(new Dictionary<string, string>());


            string fileContent = string.Empty;

            if (File.Exists(filePath))
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    fileContent = sr.ReadToEnd();
                }


                foreach (var item in replacementsDictionary.Keys)
                {
                    fileContent = fileContent.Replace(item, replacementsDictionary[item]);
                }

                File.WriteAllText(filePath, fileContent);
            }
        }
        private void SolutionEvents_ReplaceRecursive(ProjectItem pItem)
        {
            bool isFolder = pItem.ProjectItems.Count > 0;

            if (isFolder)
            {
                IEnumerator enumerator = pItem.ProjectItems.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    this.SolutionEvents_ReplaceRecursive(((ProjectItem)enumerator.Current));
                }
            }
            else
            {
                this.SolutionEvents_ReplaceParameters(this.DatabaseProjects_GetFullPathFromProperties(pItem));
            }
        }
        private void SolutionEvents_CloseAllDocuments()
        {
            GlobalData.dte.ExecuteCommand("Window.CloseAllDocuments");
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

        #endregion

        private void BackEndHostsAddAsALinkSharedAppConfig()
        {

            IEnumerator enumerator = IWizardImplementation.GlobalData.dte.Solution.Projects.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Project item = (Project)enumerator.Current;

                bool isBackendHostProject = (item.Name == string.Format("{0}.WCF.ServicesHost", IWizardImplementation.GlobalData.CustomNamespace))
                                            ||
                                            (item.Name == string.Format("{0}.WCF.ServicesHostWorkerRole", IWizardImplementation.GlobalData.CustomNamespace));

                if (isBackendHostProject)
                {
                    string pathToSharedAppConfig = string.Format(@"{0}\{1}.WCF.ServicesHostCommon\app.config", IWizardImplementation.GlobalData.SolutionDirectory, IWizardImplementation.GlobalData.CustomNamespace);

                    ProjectItem appConfigLink = item.ProjectItems.AddFromFile(pathToSharedAppConfig);


                    //project.ProjectItems.AddFromFile()
                }
            }
        }



        #region Database Projects
        /// <summary>
        /// Por el motivo que sea los proyectos de tipo *.sqlproj no pasan por el proceso normal y los parametros no llegan a reemplazarse
        /// Asi que tengo que hacer un tratamiento especial para reemplazar los parametros programaticamente
        /// </summary>
        private void DatabaseProjects_ReplaceParameters()
        {
            IEnumerator enumerator = IWizardImplementation.GlobalData.dte.Solution.Projects.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Project item = (Project)enumerator.Current;

                bool isDatabaseProject = (item.Name.Contains(string.Format("{0}.Database.", IWizardImplementation.GlobalData.CustomNamespace)));

                if (isDatabaseProject)
                {
                    //this.DatabaseProject_AddUserFile(item);

                    IEnumerator enumeratorFiles = item.ProjectItems.GetEnumerator();

                    while (enumeratorFiles.MoveNext())
                    {
                        this.SolutionEvents_ReplaceRecursive(((ProjectItem)enumeratorFiles.Current));
                    }
                }
            }
        }
        private string DatabaseProjects_GetFullPathFromProperties(ProjectItem project)
        { 
            string result = string.Empty;

            if (project.Properties != null)
            {
                IEnumerator enumerator = project.Properties.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    Property p = (Property)enumerator.Current;
                    if (p.Name == "FullPath")
                    {
                        return p.Value;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Git Helper Files

        private void SolutionEvents_GitHelperFiles_Add()
        {
            //As far as -> adding files directly to the solution is not posible using *.vstemplate file
            //             I need 
            //               1.- to create "GitFiles" folder under "ui.web.standalone" and set needed files inside
            //               2.- then add those files using ui.web.standalone.vstemplate 
            //               3.- copy to the solution folder using this routine

            Solution4 solution = ((Solution4)GlobalData.dte.Solution);
            string templatePath = solution.GetProjectTemplate("ProjectTemplate1.zip", "CSharp");
            FileInfo fi = new FileInfo(templatePath);
            DirectoryInfo di = fi.Directory.GetDirectories("Layers").First()
                                            .GetDirectories("UI").First()
                                            .GetDirectories("GitFiles").First();

            List<string> filePaths = new List<string>();
            filePaths.Add(di.GetFiles(".deployment").First().FullName);
            filePaths.Add(di.GetFiles(".gitattributes").First().FullName);
            filePaths.Add(di.GetFiles(".gitignore").First().FullName);
            filePaths.Add(di.GetFiles(".gitHowToDeploy").First().FullName);

            Project p = solution.AddSolutionFolder("git_files");
            foreach (var item in filePaths)
            {
                ProjectItem pItem = p.ProjectItems.AddFromFileCopy(item);
                this.SolutionEvents_ReplaceParameters(Path.Combine(GlobalData.SolutionDirectory.Parent.FullName, pItem.Name));
                pItem.Save();
            }
        }

        #endregion
    }
}