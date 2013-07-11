using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Models.UserRequestModel;

namespace $safeprojectname$.Proxies
{
    [TestClass]
    public abstract class TestProxyBase
    {
        //private CulturesAvailable currentCulture = $customNamespace$.Models.Enumerations.CulturesAvailable.es;
        //private string currentCulture = "es-ES";
        public static string currentCultureName = TestCommon.CultureDefault;

        public virtual void MyTestInitialize()
        {
            TestProxyBase.SetHttpContext();
        }

        public static void SetHttpContext()
        {
            SimpleWorkerRequest simpleWorkerRequest = new SimpleWorkerRequest(string.Empty, string.Empty, string.Empty, null, new StringWriter());
            HttpContext.Current = new HttpContext(simpleWorkerRequest);
            HttpContext.Current.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, currentCultureName));
            //HttpContext.Current.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, "sdsdsd"));
            Thread.CurrentThread.CurrentCulture = GlobalizationHelper.CultureInfoGetOrDefault(currentCultureName);
        }

    }
}