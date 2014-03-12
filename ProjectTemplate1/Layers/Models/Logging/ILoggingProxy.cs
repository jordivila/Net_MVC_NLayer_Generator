using System;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace $customNamespace$.Models.Logging
{

    [ServiceContract]
    public interface ILoggingProxy : IDisposable
    {
        [OperationContract]
        DataResultLogMessageList LoggingExceptionGetById(Guid guid);

        [OperationContract]
        Guid LoggingExceptionSet(LogEntry logMessage);

        [OperationContract]
        DataResultLogMessageList LoggingExceptionGetAll(DataFilterLogger filter);
    }
}
