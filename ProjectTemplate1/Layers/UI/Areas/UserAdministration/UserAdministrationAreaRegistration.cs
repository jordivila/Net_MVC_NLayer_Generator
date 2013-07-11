using System.Web.Mvc;

namespace $safeprojectname$.Areas.UserAdministration
{
    public class UserAdministrationAreaRegistration : AreaRegistration
    {
        public static string UserAdminAreaName = "UserAdministration";

        public override string AreaName
        {
            get
            {
                return UserAdministrationAreaRegistration.UserAdminAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                string.Format("{0}_default", UserAdministrationAreaRegistration.UserAdminAreaName),
                string.Format("{0}/{{action}}/{{id}}", UserAdministrationAreaRegistration.UserAdminAreaName),
                new { controller = UserAdministrationAreaRegistration.UserAdminAreaName, action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                string.Format("{0}_defaultII", UserAdministrationAreaRegistration.UserAdminAreaName),
                string.Format("{0}/{{action}}/{{id}}/{{model}}/{{formAction}}", UserAdministrationAreaRegistration.UserAdminAreaName),
                new
                {
                    controller = UserAdministrationAreaRegistration.UserAdminAreaName,
                    action = "Details",
                    id = UrlParameter.Optional,
                    model = UrlParameter.Optional,
                    formAction = UrlParameter.Optional
                }
            );

        }

    }
}
