using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using $customNamespace$.Models.Logging;
using $safeprojectname$.Models;


namespace $safeprojectname$.Areas.LogViewer.Models
{
    public class LogViewerModel : baseViewModel
    {
        public NamedElementCollection<TraceSourceData> LogTraceSources { get; set; }
        public string LogTraceSourceSelected { get; set; }
        public NamedElementCollection<TraceListenerReferenceData> LogTraceListeners { get; set; }
        public string LogTraceListenerSelected { get; set; }
        public DataResultLogMessageList LogMessages { get; set; }
        public DataFilterLogger Filter { get; set; }
    }
}