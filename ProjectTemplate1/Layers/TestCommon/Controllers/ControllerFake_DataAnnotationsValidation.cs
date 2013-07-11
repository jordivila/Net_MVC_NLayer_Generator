using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using $customNamespace$.UI.Web;
using $customNamespace$.UI.Web.Areas.Error;
using $customNamespace$.UI.Web.Areas.Home;
using $customNamespace$.UI.Web.Areas.UserAccount;
using $customNamespace$.UI.Web.Areas.UserAdministration;

namespace $safeprojectname$.Controllers
{

    /// <summary>
    /// <![CDATA[
    ///     Usage sample:
    ///     
    ///     CantAccessYourAccountViewModel model = new CantAccessYourAccountViewModel() { EmailAddress = string.Empty };
    ///     ModelStateDictionary modelState = new ControllerFake_WithModelValidation <UserAccountController, CantAccessYourAccountViewModel> ().ValidateModel(model).ModelState;
    /// ]]>
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    /// <typeparam name="TActionModel"></typeparam>

    public class ControllerFake_WithModelValidation<TController, TActionModel> : Controller // Keep in mind -> We inherit Controller to use DataAnnotations validation unit tests
            where TController : Controller, new()
    {
        protected override void Dispose(bool disposing)
        {
            if (this._controller != null)
            {
                this._controller.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Validation Methods Purposes Unit Tests
        public ControllerFake_WithModelValidation<TController, TActionModel> ValidateModel(TActionModel model)
        {
            this.ControllerContext = new ControllerContext(this.HttpContextBaseMock.Object, new RouteData(), this.Controller);
            this.Url = new UrlHelper(new RequestContext(this.HttpContextBaseMock.Object, new RouteData()), this.Routes);
            this.TryValidateModel(model);
            return this;
        }
        #endregion

        #region Controller Methods Purposes Unit Tests
        private TController _controller = null;
        public TController Controller
        {
            get
            {
                if (this._controller == null)
                {
                    this._controller = new TController();
                    ((Controller)this._controller).ControllerContext = new ControllerContext(this.HttpContextBaseMock.Object, new RouteData(), this.Controller);
                    ((Controller)this._controller).Url = new UrlHelper(new RequestContext(this.HttpContextBaseMock.Object, new RouteData()), this.Routes);
                }
                return this._controller;
            }
        }

        private Mock<HttpRequestBase> _HttpRequestBaseMock = null;
        public Mock<HttpRequestBase> HttpRequestBaseMock
        {
            get
            {
                if (this._HttpRequestBaseMock == null)
                {
                    // Build Common Request Values
                    this._HttpRequestBaseMock = new Mock<HttpRequestBase>(MockBehavior.Strict);
                    this._HttpRequestBaseMock.SetupGet(x => x.ApplicationPath).Returns("/");
                    //request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/", UriKind.Absolute));
                    this._HttpRequestBaseMock.SetupGet(x => x.ServerVariables).Returns(new NameValueCollection());
                }
                return this._HttpRequestBaseMock;
            }
            set
            {
                this._HttpRequestBaseMock = value;
            }
        }

        private Mock<HttpResponseBase> _HttpResponseBaseMock = null;
        public Mock<HttpResponseBase> HttpResponseBaseMock
        {
            get
            {
                if (this._HttpResponseBaseMock == null)
                {
                    // Build Common Response Values
                    this._HttpResponseBaseMock = new Mock<HttpResponseBase>(MockBehavior.Strict);
                    this._HttpResponseBaseMock.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns((string url) => url);
                }
                return this._HttpResponseBaseMock;
            }
            set
            {
                this._HttpResponseBaseMock = value;
            }
        }

        private Mock<HttpContextBase> _HttpContextBaseMock = null;
        public Mock<HttpContextBase> HttpContextBaseMock
        {
            get
            {
                if (this._HttpContextBaseMock == null)
                {
                    // Buid Context Base
                    this._HttpContextBaseMock = new Mock<HttpContextBase>(MockBehavior.Strict);
                    this._HttpContextBaseMock.SetupGet(x => x.Request).Returns(this.HttpRequestBaseMock.Object);
                    this._HttpContextBaseMock.SetupGet(x => x.Response).Returns(this.HttpResponseBaseMock.Object);
                }
                return this._HttpContextBaseMock;
            }
            set
            {
                this._HttpContextBaseMock = value;
            }
        }

        private RouteCollection Routes
        {
            get
            {
                var routes = new RouteCollection();
                MvcApplication.RegisterRoutes(routes);

                new UserAccountAreaRegistration().RegisterArea(new AreaRegistrationContext(UserAccountAreaRegistration.UserAccountAdminAreaName, routes));
                new HomeAreaRegistration().RegisterArea(new AreaRegistrationContext(HomeAreaRegistration.HomeAreaName, routes));
                new UserAdministrationAreaRegistration().RegisterArea(new AreaRegistrationContext(UserAdministrationAreaRegistration.UserAdminAreaName, routes));
                new ErrorAreaRegistration().RegisterArea(new AreaRegistrationContext(ErrorAreaRegistration.ErrorAreaName, routes));

                return routes;
            }
        }

        //public ControllerFake_WithModelValidation<TController, TActionModel> BuildContext()
        //{
        //    // Set Controller Common Values
        //    ((Controller)this.Controller).ControllerContext = new ControllerContext(this.HttpContextBaseMock.Object, new RouteData(), this.Controller);
        //    ((Controller)this.Controller).Url = new UrlHelper(new RequestContext(this.HttpContextBaseMock.Object, new RouteData()), this.Routes);
        //    return this;
        //}
        #endregion
    }
}
