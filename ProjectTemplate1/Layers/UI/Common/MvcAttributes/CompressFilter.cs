using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Common.Mvc.Attributes
{
    //http://weblogs.asp.net/rashid/archive/2008/03/28/asp-net-mvc-action-filter-caching-and-compression.aspx
    //http://msdn.microsoft.com/en-us/magazine/gg232768.aspx
    public class CompressFilterActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;

            string acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding))
            {
                return;
            }
            else
            {
                acceptEncoding = acceptEncoding.ToUpperInvariant();

                HttpResponseBase response = filterContext.HttpContext.Response;

                if (acceptEncoding.Contains("GZIP"))
                {
                    response.AppendHeader("Content-encoding", "gzip");
                    response.Filter = CompressFilterActionAttribute.Gzip(response.Filter);
                }
                else if (acceptEncoding.Contains("DEFLATE"))
                {
                    response.AppendHeader("Content-encoding", "deflate");
                    response.Filter = CompressFilterActionAttribute.Deflate(response.Filter);
                }
            }
        }

        public static GZipStream Gzip(Stream stream)
        {
            return new GZipStream(stream, CompressionMode.Compress);
        }

        public static DeflateStream Deflate(Stream stream)
        {
            return new DeflateStream(stream, CompressionMode.Compress);
        }
    }
}