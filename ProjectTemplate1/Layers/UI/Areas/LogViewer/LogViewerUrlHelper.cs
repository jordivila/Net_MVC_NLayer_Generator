using System.Web.Mvc;

namespace $customNamespace$.UI.Web.Areas.LogViewer
{
    public static class LogViewerUrlHelper
    {
        public static string LogViewer(this UrlHelper helper)
        {
            return helper.Action("Index", "LogViewer", new { Area = LogViewerAreaRegistration.LogViewerAreaName });
        }
        public static string LogGetById(this UrlHelper helper, string guid)
        {
            return helper.Action("LogViewerById", "LogViewer", new { Area = LogViewerAreaRegistration.LogViewerAreaName, guid = guid });
        }
        public static string LogViewerBySourceName(this UrlHelper helper, string sourceName)
        {
            return helper.Action("LogViewerBySourceName", "LogViewer", new { Area = LogViewerAreaRegistration.LogViewerAreaName, sourceName = sourceName });
        }
        public static string LogViewerByListenerName(this UrlHelper helper, string sourceName, string listenerName)
        {
            return helper.Action("LogViewerByListenerName", "LogViewer", new { Area = LogViewerAreaRegistration.LogViewerAreaName, sourceName = sourceName, listenerName = listenerName });
        }
        public static string LogViewerByModel(this UrlHelper helper)
        {
            return helper.Action("LogViewerByModel", "LogViewer", new { Area = LogViewerAreaRegistration.LogViewerAreaName });
        }



        //public static string LogViewerByProxiedWcfTraceListener(this UrlHelper helper, string sourceName, string listenerName)
        //{
        //    return helper.Action("LogViewerByProxiedWcfTraceListener", "LogViewer", new { Area = LogViewerAreaRegistration.LogViewerAreaName, sourceName = sourceName, listenerName = listenerName });
        //}
        //public static string LogViewerByRollingXmlFileTraceListener(this UrlHelper helper, string sourceName, string listenerName)
        //{
        //    return helper.Action("LogViewerByRollingXmlFileTraceListener", "LogViewer", new { Area = LogViewerAreaRegistration.LogViewerAreaName, sourceName = sourceName, listenerName = listenerName });
        //}
    }

}