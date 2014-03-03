using $customNamespace$.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $customNamespace$.Models.Logging
{
    public interface ICustomTraceListener
    {
        DataResultLogMessageList SearchLogMessages(string listenerName, string categorySourceName, string LogginConfigurationSectionName, DataFilterLogger dataFilter);
    }
}