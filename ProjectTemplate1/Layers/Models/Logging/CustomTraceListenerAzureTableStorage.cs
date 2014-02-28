using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace $customNamespace$.Models.Logging
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class AzureTableStorageListener : CustomTraceListener
    {
        private const string _azureStorageCnnAttributeName = "azureStorageConnectionStringName";

        private CloudStorageAccount storageAccount = null;
        private CloudTableClient tableClient = null;

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (this.storageAccount == null)
            {
                this.storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(this.Attributes[_azureStorageCnnAttributeName]));
                this.tableClient = storageAccount.CreateCloudTableClient();
            }

            if (this.storageAccount != null)
            {
                //if (data is LogEntry && this.Formatter != null)
                if (data is LogEntry)
                {
                    LogMessageModel logMsg = new LogMessageModel(data as LogEntry);

                    CloudTable table = tableClient.GetTableReference(logMsg.Category);
                    table.CreateIfNotExists();

                    TableOperation insertOperation = TableOperation.Insert(new AzureTableStorageListenerEntity(logMsg));
                    table.Execute(insertOperation);
                }
                else
                {
                    //this.WriteLine(data.ToString());
                }
            }
            else
            { 
            
            }
        }

        public override void Write(string message)
        {
            //Debug.Write(message);
        }

        public override void WriteLine(string message)
        {
            //Debug.WriteLine(message);
        }
    }

    public class AzureTableStorageListenerEntity : TableEntity
    {
        public AzureTableStorageListenerEntity(LogMessageModel logMessage)
        {
            this.PartitionKey = logMessage.Category;
            this.RowKey = logMessage.Timestamp.Ticks.ToString();
            this.LogMessageJSON = logMessage.SerializeToJson();
        }

        public string LogMessageJSON { get; set; }
    }
}
