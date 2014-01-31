﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Tests.Common.Controllers;
using $customNamespace$.UI.Web.Areas.UserAccount.Controllers;
using $customNamespace$.UI.Web.Areas.UserAccount.Models;
using $customNamespace$.UI.Web.Areas.UserProfile;
using $customNamespace$.UI.Web.Areas.UserProfile.Controllers;
using $customNamespace$.UI.Web.Areas.UserProfile.Models;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Tests.Client.Common;

namespace $safeprojectname$.TestCases.UserProfile
{
    [TestClass]
    public class UserProfileControllerTest : TestControllerBase<UserProfileAreaRegistration>
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {

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

        [TestMethod]
        public void UserProfileControllerTest_Security()
        {
            bool hassAuthorizeAttribute = typeof(UserProfileController).GetCustomAttributes(typeof($customNamespace$.UI.Web.Common.Mvc.Attributes.AuthorizeAttribute), true).Count() == 1;
            Assert.AreEqual(true, hassAuthorizeAttribute);
        }

        [TestMethod]
        public void UserProfileControllerTest_Update()
        {
            string UserNameValid = Guid.NewGuid().ToString();
            string UserEmailValid = string.Format("{0}@valid.com", UserNameValid);
            string UserPassword = "123456";
            Guid UserNameValidActivationToken = Guid.Empty;

            UserNameValid = Guid.NewGuid().ToString();
            UserEmailValid = string.Format("{0}@valid.com", UserNameValid);
            CommonTests.Register_Succeed(UserEmailValid, UserPassword, ref UserNameValidActivationToken);
            CommonTests.Register_ActivateAccount(UserNameValidActivationToken);
            LogOnViewModel logOnModel = new LogOnViewModel();
            logOnModel.Email = UserEmailValid;
            logOnModel.Password = UserPassword;

            ControllerFake_WithModelValidation<UserAccountController, LogOnViewModel> controllerLogin = new ControllerFake_WithModelValidation<UserAccountController, LogOnViewModel>();
            ActionResult resultPost = controllerLogin.Controller.LogOn(logOnModel);
            Assert.AreEqual(true, string.IsNullOrEmpty(HttpContext.Current.Response.Cookies[UserRequestModel_Keys.WcfFormsAuthenticationCookieName].Value) != true);
            Assert.AreEqual(true, resultPost.GetType() == typeof(RedirectResult));
            Assert.AreEqual(true, (((RedirectResult)resultPost).Url == controllerLogin.Controller.RedirectResultOnLogIn().Url));

            Mock<UserProfileIndexModel> model = new Mock<UserProfileIndexModel>();
            model.Object.UserProfileResult = new DataResultUserProfile()
            {
                Data = new UserProfileModel()
                {
                    Gender = Gender.Female,
                    FirstName = "Jordi",
                    BirthDate = DateTime.Now,
                    Culture = GlobalizationHelper.CultureInfoGetOrDefault(this.currentCultureName),
                    Theme = ThemesAvailable.Flick,
                    LastName = "Vila"
                }
            };

            ControllerFake_WithModelValidation<UserProfileController, UserProfileIndexModel> controller = new ControllerFake_WithModelValidation<UserProfileController, UserProfileIndexModel>();
            model.Object.UserProfileResult.Data.Gender = Gender.Female;
            ActionResult resultInvalid = controller.Controller.Edit(model.Object);
            bool invalidFound = ((UserProfileIndexModel)((ViewResult)resultInvalid).Model).UserProfileResultUpdated.IsValid == true;
            Assert.AreEqual(true, invalidFound);
        }
    }
}
