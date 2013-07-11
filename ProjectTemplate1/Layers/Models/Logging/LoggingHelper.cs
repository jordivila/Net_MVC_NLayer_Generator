using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace $safeprojectname$.Logging
{
    public class LoggingHelper
    {
        private static void LogMessageAsync(LogEntry log)
        {
            Logger.Write(log);
        }

        public static void Write(LogEntry logEntry)
        {
            if (Logger.IsLoggingEnabled())
            {
                if (Logger.ShouldLog(logEntry))
                {
                    Task.Factory.StartNew(() => LoggingHelper.LogMessageAsync(logEntry));
                }
            }
        }
    }

}
