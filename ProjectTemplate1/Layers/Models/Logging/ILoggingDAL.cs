using Microsoft.Practices.EnterpriseLibrary.Logging;
using $customNamespace$.Models.Logging;
using System;

namespace $customNamespace$.Models.Logging
{
    public interface ILoggingDAL: IDisposable
    {
        DataResultLogMessageList LoggingExceptionGetById(Guid guid);

        Guid LoggingExceptionSet(LogEntry logMessage);

        DataResultLogMessageList LoggingExceptionGetAll(DataFilterLogger filter);
    }
}