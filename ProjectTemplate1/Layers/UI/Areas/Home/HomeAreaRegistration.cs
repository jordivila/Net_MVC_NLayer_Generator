using System.Web.Mvc;

namespace $safeprojectname$.Areas.Home
{
    public class HomeAreaRegistration : AreaRegistration
    {
        public static string HomeAreaName = "Home";

        public override string AreaName
        {
            get
            {
                return HomeAreaRegistration.HomeAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Maps the very root site--> http://www.domainname.com/
            context.MapRoute(
                string.Format("{0}_default", HomeAreaRegistration.HomeAreaName),
                string.Empty,
                new { controller = HomeAreaRegistration.HomeAreaName, action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                string.Format("{0}_defaultII", HomeAreaRegistration.HomeAreaName),
                string.Format("{0}/{{action}}/{{id}}", HomeAreaRegistration.HomeAreaName),
                new { controller = HomeAreaRegistration.HomeAreaName, action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
