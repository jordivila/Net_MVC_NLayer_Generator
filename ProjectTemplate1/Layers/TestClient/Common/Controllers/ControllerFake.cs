using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.UI.Web;
using $customNamespace$.UI.Web.Areas.Error;
using $customNamespace$.UI.Web.Areas.Home;
using $customNamespace$.UI.Web.Areas.UserAccount;
using $customNamespace$.UI.Web.Areas.UserAdministration;
using $customNamespace$.UI.Web.Areas.UserProfile;

namespace $customNamespace$.Tests.Client.Common.Controllers
{
    /// <summary>
    /// <![CDATA[
    ///     Usage sample:
    ///     
    ///     CantAccessYourAccountViewModel model = new CantAccessYourAccountViewModel() { EmailAddress = string.Empty };
    ///     ModelStateDictionary modelState = new ControllerFake <UserAccountController, CantAccessYourAccountViewModel> ().ValidateModel(model).ModelState;
    /// ]]>
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    /// <typeparam name="TActionModel"></typeparam>

    public class ControllerFake<TController, TActionModel> : Controller // Keep in mind -> We inherit Controller to use DataAnnotations validation unit tests
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
        public ControllerFake<TController, TActionModel> ValidateModel(TActionModel model)
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

        public UrlHelper UrlHelper
        {
            get
            {
                return ((Controller)this._controller).Url;
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
                    //this._HttpRequestBaseMock = new Mock<HttpRequestBase>(MockBehavior.Strict);
                    this._HttpRequestBaseMock = new Mock<HttpRequestBase>(MockBehavior.Default);
                    this._HttpRequestBaseMock.SetupGet(x => x.ApplicationPath).Returns("/");
                    //request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/", UriKind.Absolute));
                    this._HttpRequestBaseMock.SetupGet(x => x.ServerVariables).Returns(new NameValueCollection());
                    this._HttpRequestBaseMock.SetupGet(x => x.Cookies).Returns(new HttpCookieCollection());
                    this._HttpRequestBaseMock.SetupGet(x => x.Headers).Returns(new NameValueCollection());
                    this._HttpRequestBaseMock.SetupGet(x => x.Params).Returns(new NameValueCollection());
                    this._HttpRequestBaseMock.SetupGet(x => x.Form).Returns(new NameValueCollection());
                    this._HttpRequestBaseMock.SetupGet(x => x.QueryString).Returns(new NameValueCollection());
                    this._HttpRequestBaseMock.SetupGet(x => x.HttpMethod).Returns("POST");
                    this._HttpRequestBaseMock.SetupGet(x => x.RequestType).Returns(HttpVerbs.Post.ToString().ToUpper());
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
                    this._HttpResponseBaseMock = new Mock<HttpResponseBase>(MockBehavior.Default);
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
                    this._HttpContextBaseMock = new Mock<HttpContextBase>(MockBehavior.Default);
                    this._HttpContextBaseMock.SetupGet(x => x.Request).Returns(this.HttpRequestBaseMock.Object);
                    this._HttpContextBaseMock.SetupGet(x => x.Response).Returns(this.HttpResponseBaseMock.Object);
                    this._HttpContextBaseMock.SetupGet(x => x.Session).Returns(this.HttpSessionStateMock.Object);


                    //var context = new Mock<HttpContextBase>();
                    //var request = new Mock<HttpRequestBase>();
                    //var response = new Mock<HttpResponseBase>();
                    //var session = new Mock<HttpSessionStateBase>();
                    var server = new Mock<HttpServerUtilityBase>();
                    //var user = new Mock<IPrincipal>();
                    //var identity = new Mock<IIdentity>();
                    //this._HttpContextBaseMock.Setup(ctx => ctx.Request).Returns(request.Object);
                    //this._HttpContextBaseMock.Setup(ctx => ctx.Response).Returns(response.Object);
                    //this._HttpContextBaseMock.Setup(ctx => ctx.Session).Returns(session.Object);
                    this._HttpContextBaseMock.Setup(ctx => ctx.Server).Returns(server.Object);
                    //context.Setup(ctx => ctx.User).Returns(user.Object);
                    //user.Setup(ctx => ctx.Identity).Returns(identity.Object);
                    //identity.Setup(id => id.IsAuthenticated).Returns(true);
                    //identity.Setup(id => id.Name).Returns(username);
                    //context.Setup(ctx => ctx.Response.Cache).Returns(CreateCachePolicy());
                    //return context.Object;


                }
                return this._HttpContextBaseMock;


                //var context = new Mock<HttpContextBase>();
                //var request = new Mock<HttpRequestBase>();
                //var response = new Mock<HttpResponseBase>();
                //var session = new Mock<HttpSessionStateBase>();
                //var server = new Mock<HttpServerUtilityBase>();
                //var user = new Mock<IPrincipal>();
                //var identity = new Mock<IIdentity>();


                ////controllerContext.RequestContext.HttpContext.Request.RequestType.ToUpper()

                //request.Setup(x => x.RequestType).Returns("POST");

                //context.Setup(ctx => ctx.Request).Returns(request.Object);
                //context.Setup(ctx => ctx.Response).Returns(response.Object);
                //context.Setup(ctx => ctx.Session).Returns(session.Object);
                //context.Setup(ctx => ctx.Server).Returns(server.Object);
                //context.Setup(ctx => ctx.User).Returns(user.Object);
                //user.Setup(ctx => ctx.Identity).Returns(identity.Object);
                //identity.Setup(id => id.IsAuthenticated).Returns(true);
                //identity.Setup(id => id.Name).Returns("xxx@gmail.com");
                ////context.Setup(ctx => ctx.Response.Cache).Returns(CreateCachePolicy());



                //return context;

            }
            set
            {
                this._HttpContextBaseMock = value;
            }
        }

        private Mock<HttpSessionStateBase> _HttpSessionStateMock = null;
        public Mock<HttpSessionStateBase> HttpSessionStateMock
        {
            get
            {
                if (this._HttpSessionStateMock == null)
                {
                    // Buid Context Base
                    this._HttpSessionStateMock = new Mock<HttpSessionStateBase>(MockBehavior.Loose);
                    //this._HttpSessionStateMock.SetupGet(x => x.Request).Returns(this.HttpRequestBaseMock.Object);
                    //this._HttpSessionStateMock.SetupGet(x => x.Response).Returns(this.HttpResponseBaseMock.Object);
                }
                return this._HttpSessionStateMock;
            }
            set
            {
                this._HttpSessionStateMock = value;
            }
        }


        private RouteCollection Routes
        {
            get
            {
                var routes = new RouteCollection();
                MvcApplication.RegisterRoutes(routes);

                new UserProfileAreaRegistration().RegisterArea(new AreaRegistrationContext(UserProfileAreaRegistration.UserProfileAreaName, routes));
                new UserAccountAreaRegistration().RegisterArea(new AreaRegistrationContext(UserAccountAreaRegistration.UserAccountAdminAreaName, routes));
                new HomeAreaRegistration().RegisterArea(new AreaRegistrationContext(HomeAreaRegistration.HomeAreaName, routes));
                new UserAdministrationAreaRegistration().RegisterArea(new AreaRegistrationContext(UserAdministrationAreaRegistration.UserAdminAreaName, routes));
                new ErrorAreaRegistration().RegisterArea(new AreaRegistrationContext(ErrorAreaRegistration.ErrorAreaName, routes));

                return routes;
            }
        }


        #endregion
    }

    public class ControllerActionInvokerForTesting : ControllerActionInvoker
    {
        public ActionResult ActionResultExpected { get; set; }
        public Func<ActionResult, ActionResult, bool> ActionResultComparer { get; set; }

        protected override void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
        {
            if (this.ActionResultComparer == null)
            {
                Assert.IsTrue(actionResult == this.ActionResultExpected);
            }
            else
            {
                Assert.IsTrue(ActionResultComparer(actionResult, this.ActionResultExpected));
            }
        }
    }
}
