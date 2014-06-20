using System.Web.Mvc;

namespace $customNamespace$.UI.Web.Areas.Blog
{
    public class BlogAreaRegistration : AreaRegistration
    {
        public static string BlogAreaName = "Blog";

        public override string AreaName
        {
            get
            {
                return BlogAreaRegistration.BlogAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                string.Format("{0}_default", BlogAreaRegistration.BlogAreaName),
                string.Format("{0}", BlogAreaRegistration.BlogAreaName),
                new { controller = BlogAreaRegistration.BlogAreaName, action = "Index", title = UrlParameter.Optional }
            );

            context.MapRoute(
                string.Format("{0}_defaultII", BlogAreaRegistration.BlogAreaName),
                string.Format("{0}/{{title}}.html", BlogAreaRegistration.BlogAreaName),
                new { controller = BlogAreaRegistration.BlogAreaName, action = "Index", title = UrlParameter.Optional }
            );

            context.MapRoute(
                string.Format("{0}_defaultIII", BlogAreaRegistration.BlogAreaName),
                string.Format("{0}/Category/{{categoryName}}", BlogAreaRegistration.BlogAreaName),
                new { controller = BlogAreaRegistration.BlogAreaName, action = "Category", categoryName = UrlParameter.Optional }
            );


        }
    }
}
