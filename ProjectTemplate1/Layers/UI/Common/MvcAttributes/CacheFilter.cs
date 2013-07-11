    using System;
    using System.Web;
    using System.Web.Mvc;

namespace $safeprojectname$.Common.Mvc.Attributes
{
    
    public class CacheFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the cache duration in seconds. The default is 10 seconds
        /// </summary>
        /// <value>The cache duration in seconds.</value>
        public int Duration
        {
            get;
            set;
        }

        public CacheFilterAttribute()
        {
            Duration = 900;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if ($customNamespace$.Models.Configuration.ApplicationConfiguration.IsDebugMode)
            {
                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                TimeSpan cacheDuration = TimeSpan.FromSeconds(0);

                cache.SetCacheability(HttpCacheability.Public);
                cache.SetExpires(DateTime.Now.Add(cacheDuration));
                cache.SetMaxAge(cacheDuration);
                cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            }
            else
            {
                if (Duration <= 0) return;

                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

                cache.SetCacheability(HttpCacheability.Public);
                cache.SetExpires(DateTime.Now.Add(cacheDuration));
                cache.SetMaxAge(cacheDuration);
                //  cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            }
        }
    }
}