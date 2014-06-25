using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using $customNamespace$.Models.Enumerations;

namespace $customNamespace$.Models.Logging
{
    public class LoggingHelper
    {
        private static void LogMessageAsync(LogEntry log)
        {
            using (TransactionScope transScope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                Logger.Write(log);
            }
        }

        public static void Write(LogEntry logEntry)
        {
            if (Logger.IsLoggingEnabled())
            {
                if (Logger.ShouldLog(logEntry))
                {
                    // Warning: starting new tasks could create threading deadlocks
                    //Task.Factory.StartNew(() => LoggingHelper.LogMessageAsync(logEntry));

                    LoggingHelper.LogMessageAsync(logEntry);
                }
            }
        }

        public static void Write(Exception exception)
        {
            LoggingHelper.Write(new LogEntry(exception, LoggerCategories.WCFGeneral, 1, 1, TraceEventType.Error, string.Format("{0} (DetailException)", baseModel.GetInvokingMethod().DeclaringType.FullName), null));
        }

    }
}