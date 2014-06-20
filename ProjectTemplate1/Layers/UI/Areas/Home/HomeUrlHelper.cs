using System.Web.Mvc;

namespace $customNamespace$.UI.Web.Areas.Home
{
    public static class HomeUrlHelper
    {
        public static string Home_Index(this UrlHelper helper)
        {
            return helper.Action("Index", "Home", new { Area = HomeAreaRegistration.HomeAreaName });
        }
        public static string Home_About(this UrlHelper helper)
        {
            return helper.Action("About", "Home", new { Area = HomeAreaRegistration.HomeAreaName });
        }
    }
}