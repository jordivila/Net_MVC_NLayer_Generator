using System.Web.Mvc;

namespace $safeprojectname$.Areas.Blog
{
    public static class BlogUrlHelper
    {
        public static string Index(this UrlHelper helper, string title)
        {
            return helper.Action("Index", "Blog", new { Area = BlogAreaRegistration.BlogAreaName, title = title });
        }

        public static string IndexRoot(this UrlHelper helper)
        {
            return helper.Action("Index", "Blog", new { Area = BlogAreaRegistration.BlogAreaName });
        }

        public static string Category(this UrlHelper helper, string categoryName)
        {
            return helper.Action("Category", "Blog", new { Area = BlogAreaRegistration.BlogAreaName, categoryName = categoryName });
        }

    }
}