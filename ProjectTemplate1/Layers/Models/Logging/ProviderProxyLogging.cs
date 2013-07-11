using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using $safeprojectname$.ProxyProviders;

namespace $safeprojectname$.Logging
{
    public class ProviderProxyLogging : ProviderBaseChannel<IProviderLogging>, IProviderLogging
    {
        public DataResultLogMessageList LoggingExceptionGetById(System.Guid guid)
        {
            return this.proxy.LoggingExceptionGetById(guid);
        }
        public Guid LoggingExceptionSet(LogEntry logMessage)
        {
            return this.proxy.LoggingExceptionSet(logMessage);
        }
        public DataResultLogMessageList LoggingExceptionGetAll(DataFilterLogger filter)
        {
            return this.proxy.LoggingExceptionGetAll(filter);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}