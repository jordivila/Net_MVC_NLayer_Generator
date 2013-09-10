using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.XPath;
using $safeprojectname$.Common;
using $safeprojectname$.Globalization;


namespace $safeprojectname$.Logging
{
    [DataContract]
    public class DataResultLogMessageList : baseDataPagedResult<LogMessageModel>, IDataResultPaginatedModel<LogMessageModel>
    {

    }

    [DataContract]
    public class DataResultLogMessageModel : baseDataResult<LogMessageModel>, IDataResultModel<LogMessageModel>
    {

    }

    [DataContract]
    public class LogMessageModel : baseModel
    {
        public static string LogMessageModel_DatetimeFormat
        {
            get
            {
                return "yyyy/MM/dd HH:mm:ss.fffffff";
            }
        }

        public LogMessageModel() { }
        public LogMessageModel(XPathNavigator node)
        {
            this.timestampField = DateTime.ParseExact(((XPathItem)node.SelectSingleNode("Timestamp")).Value, LogMessageModel.LogMessageModel_DatetimeFormat, GlobalizationHelper.CultureInfoGetOrDefault(string.Empty));
            this.messageField = ((XPathItem)node.SelectSingleNode("Message")).Value;
            this.categoryField = ((XPathItem)node.SelectSingleNode("Category")).Value;
            this.priorityField = Convert.ToInt32(((XPathItem)node.SelectSingleNode("Priority")).Value);
            this.eventIdField = Convert.ToInt32(((XPathItem)node.SelectSingleNode("EventId")).Value);
            this.severityField = ((XPathItem)node.SelectSingleNode("Severity")).Value;
            this.titleField = ((XPathItem)node.SelectSingleNode("Title")).Value;
            this.machineField = ((XPathItem)node.SelectSingleNode("Machine")).Value;
            this.appDomainField = ((XPathItem)node.SelectSingleNode("App_Domain")).Value;
            this.processIdField = ((XPathItem)node.SelectSingleNode("ProcessId")).Value;
            this.processNameField = ((XPathItem)node.SelectSingleNode("Process_Name")).Value;
            this.threadNameField = ((XPathItem)node.SelectSingleNode("Thread_Name")).Value;
            this.win32ThreadIdField = ((XPathItem)node.SelectSingleNode("Win32_ThreadId")).Value;

            List<LogMessageKeyValuePair> keyValuePairsList = new List<LogMessageKeyValuePair>();
            XPathNodeIterator keyValuePairsNodes = node.Select("FormattedMessage/KeyValuePair");
            foreach (var item in keyValuePairsNodes)
            {
                keyValuePairsList.Add(new LogMessageKeyValuePair()
                {
                    Name = ((XPathNavigator)item).SelectSingleNode("Name").Value,
                    Value = ((XPathNavigator)item).SelectSingleNode("Value").Value
                });
            }
            this.FormattedMessage = keyValuePairsList.ToArray();
        }
        public LogMessageModel(object message, string category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            this.messageField = message.ToString();
            this.categoryField = category;
            this.priorityField = priority;
            this.eventIdField = eventId;
            this.severityField = severity.ToString();
            this.titleField = title;
            this.formattedMessageField = (from p in properties
                                         select new LogMessageKeyValuePair() { 
                                            Name = p.Key,
                                            Value = p.Value.ToString()
                                         }).ToArray(); 
        }


        private DateTime timestampField;
        
        [DataMember]
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
        [DataMember]
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
        [DataMember] 
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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

    [DataContract]
    public class LogMessageKeyValuePair : baseModel
    {

        private string nameField;

        private string valueField;




        [DataMember]
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

        [DataMember]
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

    public class LogMessageModelDatetimeSortDescending : IComparer
    {
        public int Compare(object x, object y)
        {
            DateTime xDate = DateTime.ParseExact((string)x, LogMessageModel.LogMessageModel_DatetimeFormat, System.Globalization.CultureInfo.InvariantCulture);
            DateTime yDate = DateTime.ParseExact((string)y, LogMessageModel.LogMessageModel_DatetimeFormat, System.Globalization.CultureInfo.InvariantCulture);
            return yDate.CompareTo(xDate);
        }
    }
}
