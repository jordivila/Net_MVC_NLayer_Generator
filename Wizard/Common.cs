using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EnvDTE80;
using System.Diagnostics;
using System.Xml;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Wizard
{
    public class GlobalData
    {
        public GlobalData(object automationObjet, Dictionary<string, string> replacementsDictionary, object[] customParams)
        {
            this.CustomNamespace = replacementsDictionary["$projectname$"].Replace(" ", "_");
            this.SolutionDirectory = new DirectoryInfo((string)replacementsDictionary["$destinationdirectory$"]);
            if (this.SolutionDirectory.Parent.GetDirectories(this.NuGetPackagesFolderName).Count() == 0)
            {
                this.PackagesDirectory = this.SolutionDirectory.Parent.CreateSubdirectory(this.NuGetPackagesFolderName);
            }
            else
            {
                this.PackagesDirectory = this.SolutionDirectory.Parent.GetDirectories(this.NuGetPackagesFolderName).First();
            }

            this.PackagesDirectoryRepository = this.DirectoryGetPackagesRepository(customParams);
            this.LogWriter = new LogWriter(Path.Combine(this.SolutionDirectory.FullName, "SolutionGeneration.log"));
        }

        public string NuGetPackagesFolderName = "packages";
        public string CustomNamespace { get; set; }
        public readonly string CustomNamespaceKey = "$customNamespace$";
        public DirectoryInfo PackagesDirectory { get; private set; }
        public DirectoryInfo SolutionDirectory { get; private set; }
        public DirectoryInfo PackagesDirectoryRepository { get; private set; }
        public DTE2 dte { get; set; }


        private bool DirectoryHasPackages(DirectoryInfo current)
        {
            return (current.GetDirectories(this.NuGetPackagesFolderName).Count() == 1);
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
                    result = current.Parent.GetDirectories(this.NuGetPackagesFolderName).First();
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

        public DatabaseInstallInfo DBInfo { get; set; }
        public readonly string DatabaseServerNameKey = "$DatabaseServerName$";
        public readonly string MembershipDBNameKey = "$MembershipDBName$";
        public readonly string LoggingDBNameKey = "$LoggingDBName$";
        public readonly string WebSiteAdminEmailAddressKey = "$WebSiteAdminEmailAddress$";
        public readonly string WebSiteAdminPasswordKey = "$WebSiteAdminPassword$";
        public readonly string WebSiteApplictionNameKey = "$WebSiteApplicationName$";

        public LogWriter LogWriter { get; set; }
    }

    public class DatabaseInstallInfo
    {
        public bool CreateDatabaseAccepted { get; set; }

        public string ServerName { get; set; }
        public string MembershipDBName { get; set; }
        public string LoggingDBName { get; set; }
        public string WebSiteAdminEmailAddress { get; set; }
        public string WebSiteAdminPassword { get; set; }
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
