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

namespace $safeprojectname$.Logging
{
    [ConfigurationElementType(typeof(RollingFlatFileTraceListenerData))]
    public class RollingXmlTraceListener : RollingFlatFileTraceListener
    {
        public RollingXmlTraceListener(string fileName, string header, string footer, ILogFormatter formatter, int rollSizeKB, string timeStampPattern, RollFileExistsBehavior rollFileExistsBehavior, RollInterval rollInterval)
            : base(fileName, header, footer, formatter, rollSizeKB, timeStampPattern, rollFileExistsBehavior, rollInterval)
        {

        }

        public RollingXmlTraceListener(string fileName, string header, string footer, ILogFormatter formatter, int rollSizeKB, string timeStampPattern, RollFileExistsBehavior rollFileExistsBehavior, RollInterval rollInterval, int maxArchivedFiles)
            : base(fileName, header, footer, formatter, rollSizeKB, timeStampPattern, rollFileExistsBehavior, rollInterval, maxArchivedFiles)
        {

        }


        public static List<LogMessageModel> RollingXmlFileListenerToList(string listenerName, string LogginConfigurationSectionName)
        {
            List<LogMessageModel> result = new List<LogMessageModel>();
            MemoryStream ms = null;
            BinaryWriter ws = null;
            try
            {
                string filePath = RollingXmlTraceListener.RollingXmlFileListenerFilePath(listenerName, LogginConfigurationSectionName);
                
                string directoryName = Path.GetDirectoryName(filePath);
                string pattern = string.Format("*{0}*", Path.GetFileNameWithoutExtension(filePath));
                string[] logFiles = Directory.GetFiles(directoryName, pattern);
                ms = new MemoryStream();
                ws = new BinaryWriter(ms);
                foreach (string file in logFiles)
                {
                    try
                    {
                        ws.Write(System.IO.File.ReadAllBytes(file));
                    }
                    catch (IOException)
                    {

                    }
                }

                ms.Position = 0;

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
            }
            catch (Exception)
            {
                if (ws != null) { ws.Close(); ws.Dispose(); }
                if (ms != null) { ms.Close(); ms.Dispose(); }

                //LogMessageModel logM = new LogMessageModel();
                //logM.Title = ex.Message;
                //result.Add(logM);
            }
            return result;
        }

        private static string RollingXmlFileListenerFilePath(string listenerName, string LogginConfigurationSectionName)
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

        private static string CheckEnvironmentVarInLogFilePath(string filePath)
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

    }

    [ConfigurationElementType(typeof(CustomFormatterData))]
    public class RollingXmlTraceListenerFormatter : TextFormatter, ILogFormatter
    {
        private static string RollingXmlTraceListener = @"<LogMessageModel><Timestamp><![CDATA[{timestamp(yyyy/MM/dd HH:mm:ss.fffffff)}]]></Timestamp><Message><![CDATA[{message}]]></Message><Category><![CDATA[{category}]]></Category><Priority><![CDATA[{priority}]]></Priority><EventId><![CDATA[{eventid}]]></EventId><Severity><![CDATA[{severity}]]></Severity><Title><![CDATA[{title}]]></Title><Machine><![CDATA[{localMachine}]]></Machine><App_Domain><![CDATA[{localAppDomain}]]></App_Domain><ProcessId><![CDATA[{localProcessId}]]></ProcessId><Process_Name><![CDATA[{localProcessName}]]></Process_Name><Thread_Name><![CDATA[{threadName}]]></Thread_Name><Win32_ThreadId><![CDATA[{win32ThreadId}]]></Win32_ThreadId><FormattedMessage>{dictionary(<KeyValuePair><Name><![CDATA[{key}]]></Name><Value><![CDATA[{value}]]></Value></KeyValuePair>)}</FormattedMessage></LogMessageModel>";

        public RollingXmlTraceListenerFormatter(NameValueCollection attributes)
            : base(attributes.AllKeys.Contains("template") ? attributes["template"] : RollingXmlTraceListener)
        {

        }
    }
}
