using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Models.UserRequestModel;

namespace $safeprojectname$.Controllers
{
    [TestClass]
    public abstract class TestControllerBase<Tarea> where Tarea : AreaRegistration, new()
    {
        //private CulturesAvailable currentCulture = $customNamespace$.Models.Enumerations.CulturesAvailable.es;
        //private string currentCulture = "es-ES";
        public string currentCultureName = TestCommon.CultureDefault;




        public virtual void MyTestInitialize()
        {
            SimpleWorkerRequest simpleWorkerRequest = new SimpleWorkerRequest(string.Empty, string.Empty, string.Empty, null, new StringWriter());
            HttpContext.Current = new HttpContext(simpleWorkerRequest);
            HttpContext.Current.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, currentCultureName));
            //HttpContext.Current.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, "sdsdsd"));
            Thread.CurrentThread.CurrentCulture = GlobalizationHelper.CultureInfoGetOrDefault(currentCultureName);
        }

        private Tarea Area
        {
            get
            {
                return new Tarea();
            }
        }

        
        
        //[TestMethod]
        //public virtual void AreaRegistrationRoutesTests()
        //{
        //    // Get an AreaRegistrationContext for my class. Give it an empty RouteCollection
        //    var Routes = new RouteCollection();
        //    var areaRegistrationContext = new AreaRegistrationContext(this.Area.AreaName, Routes);
        //    this.Area.RegisterArea(areaRegistrationContext);

        //    var areaErrorsRegistrationContext = new AreaRegistrationContext($customNamespace$.UI.Web.Areas.Error.ErrorAreaRegistration.ErrorAreaName, Routes);
        //    this.Area.RegisterArea(areaErrorsRegistrationContext);

        //    // Mock up an HttpContext object with my test path (using Moq)
        //    var context = new Mock<HttpContextBase>();
        //    context.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(string.Format("~/{0}", this.Area.AreaName));

        //    // Get the RouteData based on the HttpContext
        //    var routeData = Routes.GetRouteData(context.Object);

        //    Assert.IsNotNull(routeData, "Should have found the route");
        //    Assert.AreEqual(this.Area.AreaName, routeData.DataTokens["area"]);
        //    Assert.AreEqual(this.Area.AreaName, routeData.Values["controller"]);
        //    Assert.AreEqual("Index", routeData.Values["action"]);
        //    //Assert.AreEqual("", ((UrlParameter)routeData.Values["id"]));        
        //}

    }


}