using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE80;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace VSIX_MVC_Layered_Wizard
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
                replacementsDictionary = GlobalData.ReplacementDictionaryGet(replacementsDictionary);
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