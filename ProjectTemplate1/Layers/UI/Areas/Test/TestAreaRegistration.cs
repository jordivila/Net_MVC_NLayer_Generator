using System.Web.Mvc;
using System.Web.Routing;

namespace $safeprojectname$.Areas.Test
{
    public class TestAreaRegistration : AreaRegistration
    {
        public static string TestAreaName = "Test";

        public override string AreaName
        {
            get
            {
                return TestAreaRegistration.TestAreaName;
            }
        }


        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Test_default",
            //    "Test/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            Route r = context.MapRoute(
                string.Format("{0}_default", TestAreaRegistration.TestAreaName),
                string.Format("{0}/{{action}}/{{id}}", TestAreaRegistration.TestAreaName),
                new { controller = TestAreaRegistration.TestAreaName, action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
