using System.Web.Mvc;

namespace $safeprojectname$.Areas.UserAccount
{
    public static class UserAccountUrlHelper
    {
        public static string Account_LogOn(this UrlHelper helper)
        {
            return helper.Action("LogOn", "UserAccount", new { Area = UserAccountAreaRegistration.UserAccountAdminAreaName });
        }
        public static string Account_LogOff(this UrlHelper helper)
        {
            return helper.Action("LogOff", "UserAccount", new { Area = UserAccountAreaRegistration.UserAccountAdminAreaName });
        }
        public static string Account_Register(this UrlHelper helper)
        {
            return helper.Action("Register", "UserAccount", new { Area = UserAccountAreaRegistration.UserAccountAdminAreaName });
        }
        public static string Account_CantAccessYourAccount(this UrlHelper helper)
        {
            return helper.Action("CantAccessYourAccount", "UserAccount", new { Area = UserAccountAreaRegistration.UserAccountAdminAreaName });
        }
        public static string Account_ChangePassword(this UrlHelper helper)
        {
            return helper.Action("ChangePassword", "UserAccount", new { Area = UserAccountAreaRegistration.UserAccountAdminAreaName });
        }
        public static string Account_ResetPassword(this UrlHelper helper)
        {
            return helper.Action("ResetPassword", "UserAccount", new { Area = UserAccountAreaRegistration.UserAccountAdminAreaName });
        }
        public static string Account_Activate(this UrlHelper helper)
        {
            return helper.Action("Activate", "UserAccount", new { Area = UserAccountAreaRegistration.UserAccountAdminAreaName });
        }
        public static string Account_Dashboard(this UrlHelper helper)
        {
            return helper.Action("Dashboard", "UserAccount", new { Area = UserAccountAreaRegistration.UserAccountAdminAreaName });
        }
        public static string Account_UserAccountBar(this UrlHelper helper)
        {
            return "/UserAccountBar/UserAccountBar";
            //return helper.Action("UserAccountBar", "UserAccountBar", new { Area = UserAccountAreaRegistration.UserAccountAdminAreaName });
        }
    }
}