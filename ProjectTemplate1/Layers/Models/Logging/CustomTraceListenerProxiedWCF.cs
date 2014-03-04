using System;
using System.Diagnostics;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using $customNamespace$.Models.Unity;
using $customNamespace$.Models.Common;

namespace $customNamespace$.Models.Logging
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class ProxiedWcfTraceListener : CustomTraceListener, ICustomTraceListener
    {
        private ProviderProxyLogging proxyLogging = null;

        public ProxiedWcfTraceListener()
        {
            proxyLogging = new ProviderProxyLogging();
        }

        protected override void Dispose(bool disposing)
        {
            this.proxyLogging.proxy.Dispose();

            base.Dispose(disposing);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            //if (data is LogEntry && this.Formatter != null)
            if (data is LogEntry)
            {
                // Check Channel State otherwise can generate loop errors
                if (((IClientChannel)this.proxyLogging.proxy).State == CommunicationState.Opened)
                {
                    Guid guid = this.proxyLogging.proxy.LoggingExceptionSet(data as LogEntry);
                }
            }
            else
            {
                //this.WriteLine(data.ToString());
            }
        }

        public DataResultLogMessageList SearchLogMessages(string listenerName, string categorySourceName, string LogginConfigurationSectionName, DataFilterLogger dataFilter)
        {
            using (IProviderLogging provider = DependencyFactory.Resolve<IProviderLogging>())
            {
                DataResultLogMessageList resultSearch = provider.LoggingExceptionGetAll((DataFilterLogger)dataFilter);
                return new DataResultLogMessageList()
                {
                    Data = resultSearch.Data,
                    Page = resultSearch.Page,
                    PageSize = resultSearch.PageSize,
                    SortAscending = resultSearch.SortAscending,
                    SortBy = resultSearch.SortBy,
                    TotalPages = resultSearch.TotalPages,
                    TotalRows = resultSearch.TotalRows
                };
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
}