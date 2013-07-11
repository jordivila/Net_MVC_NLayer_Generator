using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE80;
using System.IO;
using System.Linq;

namespace CustomWizard
{
    public class IWizardImplementationChild : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            EnvDTE.DTE dte = (DTE)automationObject;
            Solution solution = (Solution)dte.Solution;
            //List<string> projects = solution.SolutionBuild.StartupProjects;
            //this.dte.ActiveSolutionProjects



            try
            {
                DirectoryInfo currentDirectory = new DirectoryInfo(replacementsDictionary["$destinationdirectory$"]);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.CustomNamespaceKey, IWizardImplementation.GlobalData.CustomNamespace);
                replacementsDictionary.Add("$customPackagesRelativeLevelPath$", IWizardImplementation.GlobalData.DirectoryGetPackagesLevel(currentDirectory));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}