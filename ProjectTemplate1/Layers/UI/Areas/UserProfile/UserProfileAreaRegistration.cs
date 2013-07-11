using System.Web.Mvc;

namespace $safeprojectname$.Areas.UserProfile
{
    public class UserProfileAreaRegistration : AreaRegistration
    {
        public static string UserProfileAreaName = "UserProfile";

        public override string AreaName
        {
            get
            {
                return UserProfileAreaRegistration.UserProfileAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                string.Format("{0}_default", UserProfileAreaRegistration.UserProfileAreaName),
                string.Format("{0}/{{action}}/{{id}}", UserProfileAreaRegistration.UserProfileAreaName),
                new { controller = UserProfileAreaRegistration.UserProfileAreaName, action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
