using System;
using System.Web.Mvc;

namespace $customNamespace$.UI.Web.Areas.UserAdministration
{
    public static class UrlHelperUserAdmin
    {
        public static string UserAdminIndex(this UrlHelper helper)
        {
            return helper.Action("Index", "UserAdministration", new { Area = UserAdministrationAreaRegistration.UserAdminAreaName });
        }

        public static string UserAdminDetails(this UrlHelper helper)
        {
            return helper.Action("Details", "UserAdministration", new { Area = UserAdministrationAreaRegistration.UserAdminAreaName });
        }

        public static string UserAdminDetails(this UrlHelper helper, Guid providerKey)
        {
            return helper.Action(string.Format("Details/{0}", providerKey), "UserAdministration", new { Area = UserAdministrationAreaRegistration.UserAdminAreaName });
        }

        public static string UserAdminRoleManager(this UrlHelper helper)
        {
            return helper.Action("RolesManager", "UserAdministration", new { Area = UserAdministrationAreaRegistration.UserAdminAreaName });
        }


    }
}