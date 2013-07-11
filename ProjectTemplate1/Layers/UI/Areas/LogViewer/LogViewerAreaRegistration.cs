using System.Web.Mvc;
using System.Web.Routing;

namespace $safeprojectname$.Areas.LogViewer
{
    public class LogViewerAreaRegistration : AreaRegistration
    {
        public static string LogViewerAreaName = "LogViewer";

        public override string AreaName
        {
            get
            {
                return LogViewerAreaRegistration.LogViewerAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            Route r = context.MapRoute(
                string.Format("{0}_default", LogViewerAreaRegistration.LogViewerAreaName),
                string.Format("{0}/{{action}}/{{id}}", LogViewerAreaRegistration.LogViewerAreaName),
                new { controller = LogViewerAreaRegistration.LogViewerAreaName, action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
