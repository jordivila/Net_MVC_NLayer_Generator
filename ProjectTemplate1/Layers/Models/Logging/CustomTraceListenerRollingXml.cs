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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace $customNamespace$.Models.Logging
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class RollingXmlTraceListener : CustomTraceListener, ICustomTraceListener
    {
        private const string _fileNameAttribute = "fileName";
        private string _fileName = string.Empty;

        private const string _maxArchivedFilesAttribute = "maxArchivedFiles";
        private int _maxArchivedFiles = 2;

        private const string _rollSizeKBAttribute = "rollSizeKB";
        private int _rollSizeKB = 1024;

        private const string _timeStampPatternAttribute = "timeStampPattern";
        private string _timeStampPattern = "yyyy-MM-dd";


        private RollingFlatFileTraceListener _rollingFlatFileTraceListener = null;

        public RollingXmlTraceListener()
            : base()
        {
            
        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[] { _fileNameAttribute, _maxArchivedFilesAttribute, _rollSizeKBAttribute, _timeStampPatternAttribute };
        }

        private RollingFlatFileTraceListener RollingFlatFileTraceListenerGet()
        {
            if (this._rollingFlatFileTraceListener == null)
            {
                this.AttributesSetDefaultValues();

                this._rollingFlatFileTraceListener = new RollingFlatFileTraceListener(
                                                            this.Attributes[_fileNameAttribute]
                                                            , string.Empty
                                                            , string.Empty
                                                            , null
                                                            , this._rollSizeKB
                                                            , this._timeStampPattern
                                                            , RollFileExistsBehavior.Overwrite
                                                            , RollInterval.None
                                                            , this._maxArchivedFiles);
            }

            return this._rollingFlatFileTraceListener;
        }

        private void AttributesSetDefaultValues()
        {
            if (!this.Attributes.ContainsKey(_maxArchivedFilesAttribute))
            {
                this.Attributes.Add(_maxArchivedFilesAttribute, this._maxArchivedFiles.ToString());
            }

            if (!this.Attributes.ContainsKey(_rollSizeKBAttribute))
            {
                this.Attributes.Add(_rollSizeKBAttribute, this._rollSizeKB.ToString());
            }

            if (!this.Attributes.ContainsKey(_timeStampPatternAttribute))
            {
                this.Attributes.Add(_timeStampPatternAttribute, this._timeStampPattern.ToString());
            }
        }

        private NameValueCollection AttributesConvertToNameValueCollection()
        {
            NameValueCollection attributes = new NameValueCollection();

            foreach (string item in this.Attributes.Keys)
            {
                attributes.Add(item, this.Attributes[item]);
            }

            return attributes;
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data is LogEntry)
            {
                this.Write(baseModel.Serialize(new LogMessageModel(data as LogEntry)).DocumentElement.OuterXml);
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            this.RollingFlatFileTraceListenerGet().TraceData(eventCache, source, eventType, id, data);
        }

        public override void Write(string message)
        {
            this.RollingFlatFileTraceListenerGet().Write(message);
            this.RollingFlatFileTraceListenerGet().RollingHelper.RollIfNecessary();
        }

        public override void WriteLine(string message)
        {
            this.RollingFlatFileTraceListenerGet().WriteLine(message);
            this.RollingFlatFileTraceListenerGet().RollingHelper.RollIfNecessary();
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
                if ((listener.Name == listenerName) && listener is CustomTraceListenerData)
                {
                    CustomTraceListenerData data = listener as CustomTraceListenerData;
                    filePath = data.Attributes[_fileNameAttribute];
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
