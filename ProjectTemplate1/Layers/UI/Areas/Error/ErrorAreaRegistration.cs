using System.Web.Mvc;
using System.Web.Routing;

namespace $safeprojectname$.Areas.Error
{
    public class ErrorAreaRegistration : AreaRegistration
    {

        public static string ErrorAreaName = "Error";

        public override string AreaName
        {
            get
            {
                return ErrorAreaRegistration.ErrorAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            Route r = context.MapRoute(
                string.Format("{0}_default", ErrorAreaRegistration.ErrorAreaName),
                string.Format("{0}/{{action}}/{{id}}", ErrorAreaRegistration.ErrorAreaName),
                new { controller = ErrorAreaRegistration.ErrorAreaName, action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
