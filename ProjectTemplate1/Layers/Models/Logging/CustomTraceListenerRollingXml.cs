using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using $customNamespace$.Models.Common;

namespace $customNamespace$.Models.Logging
{
    [ConfigurationElementType(typeof(RollingFlatFileTraceListenerData))]
    public class RollingXmlTraceListener : RollingFlatFileTraceListener, ICustomTraceListener
    {
        public RollingXmlTraceListener()
            : base("RollingXmlTraceListener.xml")
        {

        }

        public RollingXmlTraceListener(string fileName, string header, string footer, ILogFormatter formatter, int rollSizeKB, string timeStampPattern, RollFileExistsBehavior rollFileExistsBehavior, RollInterval rollInterval)
            : base(fileName, header, footer, formatter, rollSizeKB, timeStampPattern, rollFileExistsBehavior, rollInterval)
        {

        }

        public RollingXmlTraceListener(string fileName, string header, string footer, ILogFormatter formatter, int rollSizeKB, string timeStampPattern, RollFileExistsBehavior rollFileExistsBehavior, RollInterval rollInterval, int maxArchivedFiles)
            : base(fileName, header, footer, formatter, rollSizeKB, timeStampPattern, rollFileExistsBehavior, rollInterval, maxArchivedFiles)
        {

        }

        public DataResultLogMessageList SearchLogMessages(string listenerName, string categorySourceName, string LogginConfigurationSectionName, DataFilterLogger dataFilter)
        {
            List<LogMessageModel> logMessageModelList = new List<LogMessageModel>();

            using (MemoryStream ms = new MemoryStream(this.GetAllDataMemoryStream(listenerName, LogginConfigurationSectionName)))
            {
                logMessageModelList = this.GetAllDataDeserialized(ms);
                logMessageModelList = logMessageModelList.Where(r => r.Category == categorySourceName).ToList();
            }

            int rowStartIndex = dataFilter.Page.Value * dataFilter.PageSize;
            int rowEndIndex = (int)(dataFilter.Page.Value * dataFilter.PageSize) + dataFilter.PageSize;

            return new DataResultLogMessageList()
            {
                Page = dataFilter.Page,
                PageSize = dataFilter.PageSize,
                Data = logMessageModelList.Skip(rowStartIndex).Take(rowEndIndex).ToList(),
                TotalRows = logMessageModelList.Count,
            };
        }

        private string RollingXmlFileListenerFilePath(string listenerName, string LogginConfigurationSectionName)
        {
            string filePath = string.Empty;
            LoggingSettings log = ConfigurationManager.GetSection(LogginConfigurationSectionName) as LoggingSettings;
            foreach (TraceListenerData listener in log.TraceListeners)
            {
                if ((listener.Name == listenerName) && listener is RollingFlatFileTraceListenerData)
                {
                    RollingFlatFileTraceListenerData data = listener as RollingFlatFileTraceListenerData;
                    filePath = data.FileName;
                    break;
                }
            }
            return CheckEnvironmentVarInLogFilePath(filePath);
        }

        private string CheckEnvironmentVarInLogFilePath(string filePath)
        {
            // Environment Variables are enclosed "%" characters.
            // This routine checks if exists some environment variable in log file path
            if (filePath.Contains("%"))
            {
                string environmentVarKey = filePath.Substring(filePath.IndexOf('%', 0), filePath.IndexOf('%', filePath.IndexOf('%', 0) + 1) + 1);
                string environmentVarValue = Environment.GetEnvironmentVariable(environmentVarKey.Replace("%", string.Empty));

                if (!string.IsNullOrEmpty(environmentVarValue))
                {
                    filePath = filePath.Replace(environmentVarKey, environmentVarValue);
                }
            }
            return filePath;
        }

        private string[] GetAllFilesWrittenByListener(string listenerName, string LogginConfigurationSectionName)
        {
            string filePath = this.RollingXmlFileListenerFilePath(listenerName, LogginConfigurationSectionName);
            string directoryName = Path.GetDirectoryName(filePath);
            string pattern = string.Format("*{0}*", Path.GetFileNameWithoutExtension(filePath));
            return Directory.GetFiles(directoryName, pattern);
        }

        private byte[] GetAllDataMemoryStream(string listenerName, string LogginConfigurationSectionName)
        {
            List<byte[]> buffers = new List<byte[]>();
            string[] logFiles = this.GetAllFilesWrittenByListener(listenerName, LogginConfigurationSectionName);

            foreach (string file in logFiles)
            {
                try
                {
                    buffers.Add(File.ReadAllBytes(file));
                }
                catch (IOException)
                {

                }
            }

            return buffers.SelectMany(b => b).ToArray();
        }

        private List<LogMessageModel> GetAllDataDeserialized(MemoryStream ms)
        {
            List<LogMessageModel> result = new List<LogMessageModel>();
            XmlReaderSettings set = new XmlReaderSettings();
            set.ConformanceLevel = ConformanceLevel.Fragment;
            XPathDocument doc = new XPathDocument(XmlReader.Create(ms, set));
            XPathNavigator nav = doc.CreateNavigator();

            XPathExpression selectExpression = nav.Compile("/LogMessageModel");
            selectExpression.AddSort(nav.Compile("Timestamp"), new LogMessageModelDatetimeSortDescending());
            XPathNodeIterator nodeIterator = nav.Select(selectExpression);
            while (nodeIterator.MoveNext())
            {
                result.Add(new LogMessageModel(nodeIterator.Current));
            }

            return result;
        }
    }

    [ConfigurationElementType(typeof(CustomFormatterData))]
    public class RollingXmlTraceListenerFormatter : TextFormatter, ILogFormatter
    {
        private const string RollingXmlTraceListener = @"<LogMessageModel><Timestamp><![CDATA[{timestamp(yyyy/MM/dd HH:mm:ss.fffffff)}]]></Timestamp><Message><![CDATA[{message}]]></Message><Category><![CDATA[{category}]]></Category><Priority><![CDATA[{priority}]]></Priority><EventId><![CDATA[{eventid}]]></EventId><Severity><![CDATA[{severity}]]></Severity><Title><![CDATA[{title}]]></Title><Machine><![CDATA[{localMachine}]]></Machine><App_Domain><![CDATA[{localAppDomain}]]></App_Domain><ProcessId><![CDATA[{localProcessId}]]></ProcessId><Process_Name><![CDATA[{localProcessName}]]></Process_Name><Thread_Name><![CDATA[{threadName}]]></Thread_Name><Win32_ThreadId><![CDATA[{win32ThreadId}]]></Win32_ThreadId><FormattedMessage>{dictionary(<KeyValuePair><Name><![CDATA[{key}]]></Name><Value><![CDATA[{value}]]></Value></KeyValuePair>)}</FormattedMessage></LogMessageModel>";

        public RollingXmlTraceListenerFormatter(NameValueCollection attributes)
            : base(attributes.AllKeys.Contains("template") ? attributes["template"] : RollingXmlTraceListener)
        {

        }
    }
}