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
        private LoggingProxy proxyLogging = null;

        public ProxiedWcfTraceListener()
        {
            proxyLogging = new LoggingProxy();
        }

        protected override void Dispose(bool disposing)
        {
            this.proxyLogging.Dispose();

            base.Dispose(disposing);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data is LogEntry)
            {
                // Check Channel State otherwise can generate loop errors
                if (this.proxyLogging.State == CommunicationState.Opened)
                {
                    Guid guid = this.proxyLogging.LoggingExceptionSet(data as LogEntry);
                }
            }
            else
            {
                //this.WriteLine(data.ToString());
            }
        }

        public DataResultLogMessageList SearchLogMessages(string LogginConfigurationSectionName, DataFilterLogger dataFilter)
        {
            using (ILoggingProxy provider = DependencyFactory.Resolve<ILoggingProxy>())
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