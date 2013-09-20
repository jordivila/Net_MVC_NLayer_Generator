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
                DirectoryInfo currentDirectory = new DirectoryInfo(replacementsDictionary[IWizardImplementation.GlobalData.TemplateConstants.DestinationDirectory]);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.CustomNamespaceKey, IWizardImplementation.GlobalData.CustomNamespace);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.DatabaseServerNameKey, IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.ServerName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.MembershipDBNameKey, IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.MembershipDBName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.LoggingDBNameKey, IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.LoggingDBName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.WebSiteAdminEmailAddressKey, IWizardImplementation.GlobalData.WebSiteConfig.WebSiteData.WebSiteAdminEmailAddress);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.WebSiteAdminPasswordKey, IWizardImplementation.GlobalData.WebSiteConfig.WebSiteData.WebSiteAdminPassword);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.WebSiteApplictionNameKey, IWizardImplementation.GlobalData.CustomNamespace);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.CustomPackagesRelativeLevelPath, IWizardImplementation.GlobalData.DirectoryGetPackagesLevel(currentDirectory));

                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingIsNetTcp, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.IsNetTcpBinding ? IWizardImplementation.GlobalData.TemplateConstants.BindingConfigurationValue : IWizardImplementation.GlobalData.TemplateConstants.BindingDeactivatedName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingIsBasicHttp, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.IsBasicHttpBinding ? IWizardImplementation.GlobalData.TemplateConstants.BindingConfigurationValue : IWizardImplementation.GlobalData.TemplateConstants.BindingDeactivatedName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingProtocol, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.Protocol);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingTypeNameKey, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.BindingTypeName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingConfigurationKey, IWizardImplementation.GlobalData.TemplateConstants.BindingConfigurationValue);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingDeactivated, IWizardImplementation.GlobalData.TemplateConstants.BindingDeactivatedName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingUserRequestModelAtServer, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.BindingUserRequestModelSelected);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingUserRequestModelHttpAliasName, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.BindingUserRequestModelHttpAliasName);
                replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingUserRequestModelNetTcpAliasName, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.BindingUserRequestModelNetTcpAliasName);
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