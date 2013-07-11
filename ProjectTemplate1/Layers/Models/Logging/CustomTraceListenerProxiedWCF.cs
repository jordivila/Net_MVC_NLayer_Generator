using System;
using System.Diagnostics;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace $safeprojectname$.Logging
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class ProxiedWcfTraceListener : CustomTraceListener
    {
        private static ProviderProxyLogging proxyLogging = new ProviderProxyLogging();

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data is LogEntry && this.Formatter != null)
            {
                // Check Channel State otherwise can generate loop errors
                if (((IClientChannel)ProxiedWcfTraceListener.proxyLogging.proxy).State == CommunicationState.Opened)
                {
                    Guid guid = ProxiedWcfTraceListener.proxyLogging.proxy.LoggingExceptionSet(data as LogEntry);
                }
            }
            else
            {
                //this.WriteLine(data.ToString());
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
