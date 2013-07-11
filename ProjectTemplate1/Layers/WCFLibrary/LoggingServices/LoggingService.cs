using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using $customNamespace$.BL.LoggingServices;
using $customNamespace$.Models.Logging;

namespace $safeprojectname$.LoggingServices
{
    public class LoggingService : IProviderLogging, IDisposable
    {
        ILoggingBL _bl = null;

        public LoggingService()
        {
            this._bl = new LoggingBL();
        }
        public void Dispose()
        {
            if (this._bl != null)
            {
                this._bl.Dispose();
            }
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
