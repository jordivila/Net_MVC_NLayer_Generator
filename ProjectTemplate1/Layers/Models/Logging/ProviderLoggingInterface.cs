using System;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace $safeprojectname$.Logging
{

    [ServiceContract]
    public interface IProviderLogging : IDisposable
    {
        [OperationContract]
        DataResultLogMessageList LoggingExceptionGetById(Guid guid);

        [OperationContract]
        Guid LoggingExceptionSet(LogEntry logMessage);

        [OperationContract]
        DataResultLogMessageList LoggingExceptionGetAll(DataFilterLogger filter);

    }
}
