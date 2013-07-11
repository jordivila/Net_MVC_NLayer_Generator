using System.Web.Mvc;

namespace $safeprojectname$.Areas.UserAccount
{
    public class UserAccountAreaRegistration : AreaRegistration
    {
        public static string UserAccountAdminAreaName = "UserAccount";

        public override string AreaName
        {
            get
            {
                return UserAccountAreaRegistration.UserAccountAdminAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                string.Format("{0}_default", UserAccountAreaRegistration.UserAccountAdminAreaName),
                string.Format("{0}/{{action}}/{{id}}", UserAccountAreaRegistration.UserAccountAdminAreaName),
                new { controller = UserAccountAreaRegistration.UserAccountAdminAreaName, action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
