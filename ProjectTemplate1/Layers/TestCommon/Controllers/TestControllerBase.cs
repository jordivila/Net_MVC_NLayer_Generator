using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Unity;
using $customNamespace$.UI.Web.Unity;


namespace $safeprojectname$.Controllers
{
    [TestClass]
    public abstract class TestControllerBase<Tarea> : TestBase where Tarea : AreaRegistration, new()
    {
        //public string currentCultureName = TestCommon.CultureDefault;
        //public static string currentCultureNameStatic = TestCommon.CultureDefault;

        public virtual void MyTestInitialize()
        {
            TestControllerBase<Tarea>.Application_InitEnterpriseLibrary();
            TestControllerBase<Tarea>.SetHttpContext();
        }

        private Tarea Area
        {
            get
            {
                return new Tarea();
            }
        }

        //public static void SetHttpContext()
        //{
        //    SimpleWorkerRequest simpleWorkerRequest = new SimpleWorkerRequest(string.Empty, string.Empty, string.Empty, null, new StringWriter());
        //    HttpContext.Current = new HttpContext(simpleWorkerRequest);
        //    HttpContext.Current.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, currentCultureNameStatic));
        //    Thread.CurrentThread.CurrentCulture = GlobalizationHelper.CultureInfoGetOrDefault(currentCultureNameStatic);
        //}

        //public static void Application_InitEnterpriseLibrary()
        //{
        //    DependencyFactory.SetUnityContainerProviderFactory(UnityContainerProvider.GetContainer(DependencyFactory.UnityContainerDefault));
        //}
    }


}