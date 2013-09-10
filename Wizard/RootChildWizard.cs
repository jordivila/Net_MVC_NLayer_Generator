using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE80;
using System.IO;
using System.Linq;
using Wizard;
using System.Diagnostics;

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
            try
            {
                DirectoryInfo currentDirectory = new DirectoryInfo(replacementsDictionary["$destinationdirectory$"]);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.CustomNamespaceKey, IWizardImplementation.GlobalData.CustomNamespace);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.DatabaseServerNameKey, IWizardImplementation.GlobalData.DBInfo.ServerName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.MembershipDBNameKey, IWizardImplementation.GlobalData.DBInfo.MembershipDBName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.LoggingDBNameKey, IWizardImplementation.GlobalData.DBInfo.LoggingDBName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.WebSiteAdminEmailAddressKey, IWizardImplementation.GlobalData.DBInfo.WebSiteAdminEmailAddress);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.WebSiteAdminPasswordKey, IWizardImplementation.GlobalData.DBInfo.WebSiteAdminPassword);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.WebSiteApplictionNameKey, IWizardImplementation.GlobalData.CustomNamespace);

                replacementsDictionary.Add("$customPackagesRelativeLevelPath$", IWizardImplementation.GlobalData.DirectoryGetPackagesLevel(currentDirectory));
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
    }
}