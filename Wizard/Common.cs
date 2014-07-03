using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml.XPath;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;
using EnvDTE100;

namespace VSIX_MVC_Layered_Wizard
{
    public class GlobalData
    {
        public GlobalData(object automationObjet, Dictionary<string, string> replacementsDictionary, object[] customParams)
        {
            this.TemplateConstants = new TemplateConstants();
            this.CustomNamespace = replacementsDictionary[this.TemplateConstants.ProjectNameKey].Replace(" ", "_");
            this.SolutionDirectory = new DirectoryInfo((string)replacementsDictionary[this.TemplateConstants.DestinationDirectory]);
            if (this.SolutionDirectory.Parent.GetDirectories(this.TemplateConstants.NuGetPackagesFolderName).Count() == 0)
            {
                this.PackagesDirectory = this.SolutionDirectory.Parent.CreateSubdirectory(this.TemplateConstants.NuGetPackagesFolderName);
            }
            else
            {
                this.PackagesDirectory = this.SolutionDirectory.Parent.GetDirectories(this.TemplateConstants.NuGetPackagesFolderName).First();
            }

            //this.PackagesDirectoryRepository = this.DirectoryGetPackagesRepository(customParams);
            this.LogWriter = new LogWriter(Path.Combine(this.SolutionDirectory.FullName, this.TemplateConstants.SolutionGenerationLogFileName));
        }

        public TemplateConstants TemplateConstants { get; set; }
        public string CustomNamespace { get; set; }
        public DirectoryInfo PackagesDirectory { get; private set; }
        public DirectoryInfo SolutionDirectory { get; private set; }
        //public DirectoryInfo PackagesDirectoryRepository { get; private set; }
        public DTE2 dte { get; set; }


        private bool DirectoryHasPackages(DirectoryInfo current)
        {
            return (current.GetDirectories(this.TemplateConstants.NuGetPackagesFolderName).Count() == 1);
        }
        public string DirectoryGetPackagesLevel(DirectoryInfo current)
        {
            string result = string.Empty;
            string packagesDirectoryLevel = @".\";

            if (!this.DirectoryHasPackages(current))
            {
                while (current.Parent != null)
                {
                    packagesDirectoryLevel += @"..\";

                    if (!this.DirectoryHasPackages(current.Parent))
                    {
                        current = current.Parent;
                    }
                    else
                    {
                        result = packagesDirectoryLevel;
                        break;
                    }
                }
            }

            return result;
        }
        private DirectoryInfo DirectoryGetPackagesRepository(object[] customParams)
        {
            DirectoryInfo result = null;
            DirectoryInfo current = new FileInfo((string)customParams[0]).Directory;
            while (current.Parent != null)
            {
                if (!this.DirectoryHasPackages(current.Parent))
                {
                    current = current.Parent;
                }
                else
                {
                    result = current.Parent.GetDirectories(this.TemplateConstants.NuGetPackagesFolderName).First();
                    break;
                }
            }
            return result;
        }
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public WebSiteConfigurationCustomized WebSiteConfig { get; set; }
        public LogWriter LogWriter { get; set; }

        public static Dictionary<string, string> ReplacementDictionaryGet(Dictionary<string, string> replacementsDictionary)
        {
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.CustomNamespaceKey, IWizardImplementation.GlobalData.CustomNamespace);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.DatabaseServerNameKey, IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.ServerName);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.MembershipDBNameKey, IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.MembershipDBName);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.LoggingDBNameKey, IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.LoggingDBName);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.TokenPersistenceDBNameKey, IWizardImplementation.GlobalData.WebSiteConfig.DBInfo.TokenPersistenceDBName);

            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.WebSiteAdminEmailAddressKey, IWizardImplementation.GlobalData.WebSiteConfig.WebSiteData.WebSiteAdminEmailAddress);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.WebSiteAdminPasswordKey, IWizardImplementation.GlobalData.WebSiteConfig.WebSiteData.WebSiteAdminPassword);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.WebSiteApplictionNameKey, IWizardImplementation.GlobalData.CustomNamespace);


            DirectoryInfo currentDirectory = null;

            if (replacementsDictionary.Keys.Contains(IWizardImplementation.GlobalData.TemplateConstants.DestinationDirectory))
            {
                if (Directory.Exists(replacementsDictionary[IWizardImplementation.GlobalData.TemplateConstants.DestinationDirectory]))
                {
                    currentDirectory = new DirectoryInfo(replacementsDictionary[IWizardImplementation.GlobalData.TemplateConstants.DestinationDirectory]);
                    replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.CustomPackagesRelativeLevelPath, IWizardImplementation.GlobalData.DirectoryGetPackagesLevel(currentDirectory));
                }
            }

            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingIsNetTcp, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.IsNetTcpBinding ? IWizardImplementation.GlobalData.TemplateConstants.BindingConfigurationValue : IWizardImplementation.GlobalData.TemplateConstants.BindingDeactivatedName);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingIsBasicHttp, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.IsBasicHttpBinding ? IWizardImplementation.GlobalData.TemplateConstants.BindingConfigurationValue : IWizardImplementation.GlobalData.TemplateConstants.BindingDeactivatedName);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingProtocol, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.Protocol);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingTypeNameKey, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.BindingTypeNameConfigValue);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingConfigurationKey, IWizardImplementation.GlobalData.TemplateConstants.BindingConfigurationValue);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingDeactivated, IWizardImplementation.GlobalData.TemplateConstants.BindingDeactivatedName);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingUserRequestModelAtServer, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.BindingUserRequestModelAtServerClassNameSelected);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingUserRequestModelHttpAliasName, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.BindingUserRequestModelAtServerHttpClassName);
            replacementsDictionary.Add(IWizardImplementation.GlobalData.TemplateConstants.BindingUserRequestModelNetTcpAliasName, IWizardImplementation.GlobalData.WebSiteConfig.WcfInfo.BindingUserRequestModelAtServerNetTcpClassName);
            return replacementsDictionary;
        }

    }

    public class TemplateConstants
    {
        public string NuGetPackagesFolderName = "packages";
        public readonly string SolutionGenerationLogFileName = "SolutionGeneration.log";
        public readonly string CustomNamespaceKey = "$customNamespace$";
        public readonly string ProjectNameKey = "$projectname$";
        public readonly string CustomPackagesRelativeLevelPath = "$customPackagesRelativeLevelPath$";
        public readonly string DestinationDirectory = "$destinationdirectory$";
        public readonly string DatabaseServerNameKey = "$DatabaseServerName$";
        public readonly string MembershipDBNameKey = "$MembershipDBName$";
        public readonly string LoggingDBNameKey = "$LoggingDBName$";
        public readonly string TokenPersistenceDBNameKey = "$TokenPersistenceDBNameKey$";
        

        public readonly string WebSiteAdminEmailAddressKey = "$WebSiteAdminEmailAddress$";
        public readonly string WebSiteAdminPasswordKey = "$WebSiteAdminPassword$";
        public readonly string WebSiteApplictionNameKey = "$WebSiteApplicationName$";

        public readonly string BindingIsNetTcp = "$customBindingIsNetTcp$";
        public readonly string BindingIsBasicHttp = "$customBindingIsBasicHttp$";
        public readonly string BindingProtocol = "$customBindingProtocol$";
        public readonly string BindingTypeNameKey = "$customBindingTypeName$";
        public readonly string BindingConfigurationKey = "$customBindingConfigurationName$";
        public readonly string BindingConfigurationValue = "currentBinding";
        public readonly string BindingDeactivated = "$customBindingDeactivated$";
        public readonly string BindingDeactivatedName = "currentBindingDeactivated";
        public readonly string BindingUserRequestModelAtServer = "$customBindingUserRequestModelAtServer$";
        public readonly string BindingUserRequestModelNetTcpAliasName = "$customBindingUserRequestModelNetTcpAliasName$";
        public readonly string BindingUserRequestModelHttpAliasName = "$customBindingUserRequestModelHttpAliasName$";

    }

    #region Logging

    public class LogWriter : IDisposable
    {
        public string LogFilePath { get; private set; }
        TextWriter writer = null;

        public LogWriter(string logFilePath)
        {
            this.LogFilePath = logFilePath;
            //this.sb = new StringBuilder();
            //this.writer = new StringWriter(sb);

            //this.writer = XmlWriter.Create(this.LogFilePath);
            this.writer = new StreamWriter(this.LogFilePath);
        }

        public void Write(LogMessageModel logEntry)
        {
            this.writer.Write(this.Serialize(logEntry).DocumentElement.OuterXml);
        }

        private XmlDocument Serialize(object obj)
        {
            StringWriter writer = null;
            try
            {
                writer = new StringWriter(CultureInfo.InvariantCulture);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(writer.ToString());
                return xDoc;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        public void Dispose()
        {
            if (this.writer != null)
            {
                this.writer.Close();
                this.writer.Dispose();
            }
        }
    }

    public class LogMessageModel
    {
        public static string LogMessageModel_DatetimeFormat
        {
            get
            {
                return "yyyy/MM/dd HH:mm:ss.fffffff";
            }
        }

        public LogMessageModel() { }
        public LogMessageModel(object message, string category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            this.messageField = message.ToString();
            this.categoryField = category;
            this.priorityField = priority;
            this.eventIdField = eventId;
            this.severityField = severity.ToString();
            this.titleField = title;
            if (properties == null)
            {
                properties = new Dictionary<string, object>();
            }
            this.formattedMessageField = (from p in properties
                                          select new LogMessageKeyValuePair()
                                          {
                                              Name = p.Key,
                                              Value = p.Value.ToString()
                                          }).ToArray();
        }
        public LogMessageModel(Exception ex)
        {
            this.messageField = string.Format("{0}\n\n\n{1}", ex.Message, ex.StackTrace);
            this.categoryField = ex.Source;
            this.priorityField = 1;
            this.severityField = TraceEventType.Error.ToString();
            this.titleField = ex.Message;
            this.formattedMessageField = new LogMessageKeyValuePair[0];
        }


        private DateTime timestampField;


        public DateTime Timestamp
        {
            get
            {
                return this.timestampField;
            }
            set
            {
                this.timestampField = value;
            }
        }

        private string messageField;

        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        private string categoryField;

        public string Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        private int priorityField;

        public int Priority
        {
            get
            {
                return this.priorityField;
            }
            set
            {
                this.priorityField = value;
            }
        }

        private int eventIdField;

        public int EventId
        {
            get
            {
                return this.eventIdField;
            }
            set
            {
                this.eventIdField = value;
            }
        }

        private string severityField;

        public string Severity
        {
            get
            {
                return this.severityField;
            }
            set
            {
                this.severityField = value;
            }
        }

        private string titleField;

        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        private string machineField;

        public string Machine
        {
            get
            {
                return this.machineField;
            }
            set
            {
                this.machineField = value;
            }
        }

        private string appDomainField;

        public string AppDomain
        {
            get
            {
                return this.appDomainField;
            }
            set
            {
                this.appDomainField = value;
            }
        }

        private string processIdField;

        public string ProcessId
        {
            get
            {
                return this.processIdField;
            }
            set
            {
                this.processIdField = value;
            }
        }

        private string processNameField;

        public string ProcessName
        {
            get
            {
                return this.processNameField;
            }
            set
            {
                this.processNameField = value;
            }
        }

        private string threadNameField;

        public string ThreadName
        {
            get
            {
                return this.threadNameField;
            }
            set
            {
                this.threadNameField = value;
            }
        }

        private string win32ThreadIdField;

        public string Win32ThreadId
        {
            get
            {
                return this.win32ThreadIdField;
            }
            set
            {
                this.win32ThreadIdField = value;
            }
        }

        private LogMessageKeyValuePair[] formattedMessageField;

        [System.Xml.Serialization.XmlArrayItemAttribute("KeyValuePair", IsNullable = false)]
        public LogMessageKeyValuePair[] FormattedMessage
        {
            get
            {
                return this.formattedMessageField;
            }
            set
            {
                this.formattedMessageField = value;
            }
        }
    }

    public class LogMessageKeyValuePair
    {

        private string nameField;

        private string valueField;





        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }


        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    #endregion 
}
