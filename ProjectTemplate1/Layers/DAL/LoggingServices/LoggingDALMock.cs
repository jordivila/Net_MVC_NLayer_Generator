
using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using $customNamespace$.Models.Logging;

namespace $safeprojectname$.LoggingServices
{
    public class LoggingDALMock : BaseDAL, ILoggingDAL
    {
        public DataResultLogMessageList LoggingExceptionGetById(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Guid LoggingExceptionSet(LogEntry logMessage)
        {
            throw new NotImplementedException();
        }

        public DataResultLogMessageList LoggingExceptionGetAll(DataFilterLogger filter)
        {
            throw new NotImplementedException();
        }
    }
}
