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
using Microsoft.Practices.EnterpriseLibrary.Logging.Database;
using $customNamespace$.Models.Configuration.ConnectionProviders;
using $customNamespace$.Models.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using $customNamespace$.Models.Unity;

namespace $customNamespace$.Models.Logging
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class DatabaseTraceListener : CustomTraceListener, ICustomTraceListener
    {
        private const string _databaseInstanceNameAttribute = "databaseInstanceName";
        private string _databaseInstanceName = string.Empty;

        private const string _writeLogStoredProcNameAttribute = "writeLogStoredProcName";
        private string _writeLogStoredProcName = "WriteLog";

        private const string _addCategoryStoredProcNameAttribute = "addCategoryStoredProcName";
        private string _addCategoryStoredProcName = "AddCategory";

        private FormattedDatabaseTraceListener _dormattedDatabaseTraceListener = null;

        public DatabaseTraceListener()
        {

        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[] { _databaseInstanceNameAttribute, _writeLogStoredProcNameAttribute, _addCategoryStoredProcNameAttribute };
        }

        private FormattedDatabaseTraceListener RollingFlatFileTraceListenerGet()
        {
            if (this._dormattedDatabaseTraceListener == null)
            {
                this.AttributesSetDefaultValues();

                this._dormattedDatabaseTraceListener = new FormattedDatabaseTraceListener(
                        DatabaseFactory.CreateDatabase(this.Attributes[_databaseInstanceNameAttribute]),
                        this.Attributes[_writeLogStoredProcNameAttribute],
                        this.Attributes[_addCategoryStoredProcNameAttribute],
                        new RollingXmlTraceListenerFormatter(this.AttributesConvertToNameValueCollection()));
            }

            return this._dormattedDatabaseTraceListener;
        }

        private void AttributesSetDefaultValues()
        {
            if (!this.Attributes.ContainsKey(_databaseInstanceNameAttribute))
            {
                this.Attributes.Add(_databaseInstanceNameAttribute, this._databaseInstanceName.ToString());
            }

            if (!this.Attributes.ContainsKey(_writeLogStoredProcNameAttribute))
            {
                this.Attributes.Add(_writeLogStoredProcNameAttribute, this._writeLogStoredProcName.ToString());
            }

            if (!this.Attributes.ContainsKey(_addCategoryStoredProcNameAttribute))
            {
                this.Attributes.Add(_addCategoryStoredProcNameAttribute, this._addCategoryStoredProcName.ToString());
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
            this.RollingFlatFileTraceListenerGet().TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            this.RollingFlatFileTraceListenerGet().TraceData(eventCache, source, eventType, id, data);
        }

        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }

        public DataResultLogMessageList SearchLogMessages(string LogginConfigurationSectionName, DataFilterLogger dataFilter)
        {
            return DependencyFactory.Resolve<ILoggingDAL>().LoggingExceptionGetAll(dataFilter);
        }
    }
}