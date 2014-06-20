using System.Web.Mvc;

namespace $customNamespace$.UI.Web.Areas.Test
{
    public static class TestsUrlHelper
    {
        public static string Index(this UrlHelper helper)
        {
            return helper.Action("Index", "Test", new { Area = TestAreaRegistration.TestAreaName });
        }
    }
}