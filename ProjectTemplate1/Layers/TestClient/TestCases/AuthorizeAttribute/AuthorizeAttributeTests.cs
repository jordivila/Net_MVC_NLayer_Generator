using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models;
using $customNamespace$.Models.Roles;
using $customNamespace$.Models.Unity;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Tests.Common.Controllers;
using $customNamespace$.Tests.Common.MembershipServices;
using $customNamespace$.UI.Web.Areas.Error;

namespace $safeprojectname$.TestCases.AuthorizeAttribute
{
    [TestClass]
    public class AuthorizeAttributeControllerTest : TestControllerBase<AdminOnlyControllerAreaRegistration>
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
         
            //IUnityContainer unityContainerMock = new UnityContainer();
            //unityContainerMock.RegisterType(typeof(ISmtpClient), typeof(SmtpClientMock), new InjectionMember[0]);
            //unityContainerMock.RegisterType(typeof(IMembershipDAL), typeof(MembershipDALMock), new InjectionMember[0]);
            //unityContainerMock.RegisterType(typeof(IRoleAdminDAL), typeof(RoleAdminDALMock), new InjectionMember[0]);
            //unityContainerMock.RegisterType(typeof(IProfileDAL), typeof(ProfileDALMock), new InjectionMember[0]);
            //unityContainerMock.RegisterType(typeof(ILoggingDAL), typeof(LoggingDALMock), new InjectionMember[0]);
            //unityContainerMock.RegisterType(typeof(ISyndicationDAL), typeof(SyndicationDAL), new InjectionMember[0]);
            //$customNamespace$.Models.Unity.DependencyFactory.ConfigurationSet(unityContainerMock);
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }

        [TestInitialize()]
        public override void MyTestInitialize()
        {
            base.MyTestInitialize();
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            
        }

        private void User_Check_Access(ControllerFake<AdminOnlyControllerController> controller, 
                                        Func<ActionResult, ActionResult, bool> resultComparer,
                                        ActionResult actionResultExpected)
        {
            MemberInfo action = baseModel.GetInfoMethod(() => controller.Controller.Index());
            ControllerActionInvokerForTesting invoker = new ControllerActionInvokerForTesting();


            invoker.ActionResultExpected = actionResultExpected == null ?
                                                new RedirectResult(ErrorUrlHelper.UnAuthorized(controller.Controller.Url), false)
                                                : actionResultExpected;

            invoker.ActionResultComparer = resultComparer == null ? delegate(ActionResult result1, ActionResult result2)
                                                                    {
                                                                        return ((RedirectResult)result1).Url == ((RedirectResult)result2).Url;
                                                                    }
                                                                    : resultComparer;

            Assert.IsTrue(invoker.InvokeAction(controller.Controller.ControllerContext, action.Name));
        }

        [TestMethod]
        public void User_Unauthenticated_GetsRedirect()
        {
            this.User_Check_Access(new ControllerFake<AdminOnlyControllerController>(), null, null);
        }

        [TestMethod]
        public void User_Authenticated_WithNOAccess_GetsRedirected()
        {
            UserForTesting userTesting = new UserForTesting(Guid.NewGuid().ToString(), "123456");
            ControllerFake<AdminOnlyControllerController> controller = new ControllerFake<AdminOnlyControllerController>();
            controller.Controller.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, userTesting.AuthenticationToken));
            this.User_Check_Access(controller, null, null);
        }

        [TestMethod]
        public void User_Authenticated_WithAccess_GetsController()
        {
            UserForTesting userTesting = new UserForTesting(Guid.NewGuid().ToString(), "123456");
            ControllerFake<AdminOnlyControllerController> controller = new ControllerFake<AdminOnlyControllerController>();
            controller.Controller.Request.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, userTesting.AuthenticationToken));
            Func<ActionResult, ActionResult, bool> resultComparer = delegate(ActionResult result1, ActionResult result2)
            {
                return (result1.GetType() == result2.GetType()) && (result2.GetType() == typeof(JsonResult));
            };

            using (DependencyFactory dependencyFactory = new DependencyFactory())
            {
                IProviderRoleManager providerRoles = dependencyFactory.Unity.Resolve<IProviderRoleManager>();
                providerRoles.AddToRoles(userTesting.Email, new string[1] { $customNamespace$.Models.Enumerations.SiteRoles.Administrator.ToString() });
                providerRoles.Dispose();
            }

            this.User_Check_Access(controller, resultComparer, AdminOnlyControllerController.IndexResult);
        }
    }
}



