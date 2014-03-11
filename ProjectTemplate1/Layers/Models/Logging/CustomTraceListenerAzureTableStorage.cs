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
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Configuration;
using System.Globalization;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Table.Queryable;


namespace $customNamespace$.Models.Logging
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class AzureTableStorageListener : TraceListener, ICustomTraceListener
    {
        public AzureTableStorageListener()
            : base()
        {

        }

        private const string _azureStorageCnnAttributeName = "azureStorageConnectionStringName";
        private CloudStorageAccount _storageAccount = null;
        private CloudTableClient _tableClient = null;
        private Dictionary<string, List<TableOperation>> _tableBatchOperationList = new Dictionary<string, List<TableOperation>>();
        private int _tableBatchOperationListLimit = 90;

        private CloudTableClient TableClient()
        {
            if (this._storageAccount == null)
            {



                this._storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(this.Attributes[_azureStorageCnnAttributeName]));
                this._tableClient = _storageAccount.CreateCloudTableClient();
            }

            return this._tableClient;
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (this.TableClient() != null)
            {
                //if (data is LogEntry && this.Formatter != null)
                if (data is LogEntry)
                {
                    LogMessageModel logMessage = new LogMessageModel(data as LogEntry);
                    AzureTableStorageListenerEntity azureLogEntity = new AzureTableStorageListenerEntity(logMessage);

                    if (ApplicationConfiguration.IsDebugMode)
                    {
                        this.TraceDataSingleOperation(logMessage.Category, azureLogEntity);
                    }
                    else
                    {
                        this.TraceDataBatchOperation(logMessage.Category, azureLogEntity);
                    }
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

        /// <summary>
        /// Batching is about more than just committing a bunch of rows at one time, it also has an impact on cost. 
        /// Remember how Azure Table Storage charges you $0.0000001 per “transaction”? 
        /// 
        /// By the time I write these lines:
        /// 
        /// There are no limits to the number of tables you can create in Windows Azure. 
        /// 
        /// 1.- single operation: the size of the entity must be a maximum of 64KB 
        /// 2.- batch operation : max of 100 entities or 4MB (per batch)
        /// 3.- batch operation: all items in a batch must have the same partition key
        /// 
        /// </summary>
        private void TraceDataBatchOperation(string categorySource, AzureTableStorageListenerEntity azureEntityLog)
        {
            if (!this._tableBatchOperationList.ContainsKey(categorySource))
            {
                this._tableBatchOperationList.Add(categorySource, new List<TableOperation>());
            }

            List<TableOperation> operationList = this._tableBatchOperationList[categorySource];

            operationList.Add(TableOperation.Insert(azureEntityLog));

            if (operationList.Count == this._tableBatchOperationListLimit)
            {
                CloudTable table = this.TableClient().GetTableReference(categorySource);
                table.CreateIfNotExists();

                var batch = new TableBatchOperation();
                for (int i = 0; i < operationList.Count; i++)
                {
                    batch.Insert(i, operationList[i]);
                }
                table.ExecuteBatch(batch);

                operationList.Clear();
            }
        }

        private void TraceDataSingleOperation(string categorySource, AzureTableStorageListenerEntity azureEntityLog)
        {
            if (this.TableClient() != null)
            {
                CloudTable table = this.TableClient().GetTableReference(categorySource);
                table.CreateIfNotExists();
                table.Execute(TableOperation.Insert(azureEntityLog));
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


        public DataResultLogMessageList SearchLogMessages(string LogginConfigurationSectionName, DataFilterLogger dataFilter)
        {
            // Pagination Sample over Azute Table Storage Entities
            // Remember that method "Count" is not supported. Thus, I can't know the total number of pages

            //using Microsoft.WindowsAzure.Storage.Table.Queryable;

            //var table = this.TableClient().GetTableReference(dataFilter.LogTraceSourceSelected);

            //var query = (from log in table.CreateQuery<AzureTableStorageListenerEntity>()
            //             where log.PartitionKey == dataFilter.CreationDateFrom.ToString("yyyyMMdd")
            //             select log).Take(dataFilter.PageSize)
            //        .AsTableQuery<AzureTableStorageListenerEntity>();

            //var result = new List<LogMessageModel>();

            //var queryResult = query.ExecuteSegmented(dataFilter.NextContinuationToken);

            //return new DataResultLogMessageList()
            //{
            //    Page = dataFilter.Page,
            //    PageSize = dataFilter.PageSize,
            //    Data = queryResult.Results.Select(p => baseModel.DeserializeFromJson<LogMessageModel>(p.LogMessageJSON)).ToList(),
            //    TotalRows = 1000, //-> just a fake
            //    NextContinuationToken = queryResult.ContinuationToken,
            //    PreviousContinuationToken = dataFilter.NextContinuationToken
            //};

            
            
            
            CloudTable table = this.TableClient().GetTableReference(dataFilter.LogTraceSourceSelected);

            TableQuery<AzureTableStorageListenerEntity> rangeQuery = new TableQuery<AzureTableStorageListenerEntity>().Where(

                    TableQuery.GenerateFilterCondition("PartitionKey",
                                                        QueryComparisons.GreaterThanOrEqual,
                                                        dataFilter.CreationDate.ToString("yyyyMMdd"))
                    );

            List<AzureTableStorageListenerEntity> resultsList = table.ExecuteQuery<AzureTableStorageListenerEntity>(rangeQuery).ToList();


            int rowStartIndex = dataFilter.Page.Value * dataFilter.PageSize;
            int rowEndIndex = (int)(dataFilter.Page.Value * dataFilter.PageSize) + dataFilter.PageSize;

            return new DataResultLogMessageList()
            {
                Page = dataFilter.Page,
                PageSize = dataFilter.PageSize,
                Data = resultsList.Skip(rowStartIndex).Take(dataFilter.PageSize).Select(p => baseModel.DeserializeFromJson<LogMessageModel>(p.LogMessageJSON)).ToList(),
                TotalRows = resultsList.Count,
            };

        }
    }


    public class AzureTableStorageListenerEntity : TableEntity
    {
        public AzureTableStorageListenerEntity()
        {

        }

        public AzureTableStorageListenerEntity(LogMessageModel logMessage)
        {
            this.PartitionKey = logMessage.Timestamp.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            this.RowKey = logMessage.Timestamp.Ticks.ToString();
            this.LogMessageJSON = logMessage.SerializeToJson();
        }

        public string LogMessageJSON { get; set; }
    }
}