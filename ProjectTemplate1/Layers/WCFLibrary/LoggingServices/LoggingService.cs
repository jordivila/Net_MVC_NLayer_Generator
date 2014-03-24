using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using $customNamespace$.BL.LoggingServices;
using $customNamespace$.Models.Logging;

namespace $customNamespace$.WCF.ServicesLibrary.LoggingServices
{
    public class LoggingService : BaseService, ILoggingProxy
    {
        ILoggingProxy _bl = null;

        public LoggingService()
        {
            this._bl = new LoggingBL();
        }
        public override void Dispose()
        {
            if (this._bl != null)
            {
                this._bl.Dispose();
            }

            base.Dispose();
        }
        public DataResultLogMessageList LoggingExceptionGetById(Guid guid)
        {
            return this._bl.LoggingExceptionGetById(guid);
        }
        public Guid LoggingExceptionSet(LogEntry logMessage)
        {
            return this._bl.LoggingExceptionSet(logMessage);
        }
        public DataResultLogMessageList LoggingExceptionGetAll(DataFilterLogger filter)
        {
            return this._bl.LoggingExceptionGetAll(filter);
        }
    }
}