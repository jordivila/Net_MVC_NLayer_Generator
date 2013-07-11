using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using $customNamespace$.Models.UserRequestModel;
using $safeprojectname$.Controllers;
using $customNamespace$.UI.Web.Areas.UserAccount.Controllers;
using $customNamespace$.UI.Web.Areas.UserAccount.Models;

namespace $safeprojectname$.MembershipServices
{
    public class CommonTests
    {
        #region Methods to test
        public static void Register_Succeed(ControllerFake<UserAccountController> controller, string UserEmailValid, string UserPassword, ref Guid UserNameValidActivationToken)
        {
            Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            model.Object.Email = UserEmailValid;
            model.Object.Password = UserPassword;
            ActionResult resultValid = controller.Controller.Register(model.Object);
            bool validFound = ((RegisterViewModel)((ViewResult)resultValid).Model).Result.CreateStatus == MembershipCreateStatus.Success;
            Assert.AreEqual(true, validFound);
            UserNameValidActivationToken = ((RegisterViewModel)((ViewResult)resultValid).Model).Result.ActivateUserToken;
        }
        public static void Register_ActivateAccount(ControllerFake<UserAccountController> controller, Guid UserNameValidActivationToken)
        {
            ActionResult resultValid = controller.Controller.Activate(UserNameValidActivationToken.ToString());
            Assert.AreEqual(true, resultValid.GetType() == typeof(RedirectResult));
            Assert.AreEqual(true, (((RedirectResult)resultValid).Url == controller.Controller.RedirectResultOnLogIn().Url));
        }
        public static void CantAccessMyAccount_Succeed(ControllerFake<UserAccountController> controller, string UserEmailValid, ref Guid CantAccessMyAccountToken)
        {

            ActionResult resultInvalid = controller.Controller.CantAccessYourAccount(new CantAccessYourAccountViewModel()
            {
                EmailAddress = UserEmailValid
            });
            Assert.AreEqual(true, ((CantAccessYourAccountViewModel)((ViewResult)resultInvalid).Model).Result.IsValid == true);
            Assert.AreEqual(true, ((CantAccessYourAccountViewModel)((ViewResult)resultInvalid).Model).Result.Data.User.Email == UserEmailValid);

            CantAccessMyAccountToken = ((CantAccessYourAccountViewModel)((ViewResult)resultInvalid).Model).Result.Data.ChangePasswordToken;
        }
        public static void ResetPassword_Succeed(ControllerFake<UserAccountController> controller, string UserEmailValid, Guid CantAccessMyAccountToken, string NewPassword)
        {
            ResetPasswordClientModel model = new ResetPasswordClientModel() { NewPassword = NewPassword, ConfirmPassword = NewPassword };
            ActionResult actionResult = controller.Controller.ResetPassword(CantAccessMyAccountToken.ToString(), model);
            Assert.AreEqual(true, actionResult.GetType() == typeof(RedirectResult));
            Assert.AreEqual(true, (((RedirectResult)actionResult).Url == controller.Controller.RedirectResultOnLogIn().Url));
        }
        public static void Login_Succeed(ControllerFake<UserAccountController> controller, string UserEmailValid, string UserPassword)
        {
            LogOnViewModel logOnModel = new LogOnViewModel();
            logOnModel.Email = UserEmailValid;
            logOnModel.Password = UserPassword;
            ActionResult resultPost = controller.Controller.LogOn(logOnModel);
            Assert.AreEqual(true, HttpContext.Current.Response.Cookies[UserRequestModel_Keys.WcfFormsAuthenticationCookieName].Value != string.Empty);
            Assert.AreEqual(true, resultPost.GetType() == typeof(RedirectResult));
            Assert.AreEqual(true, (((RedirectResult)resultPost).Url == controller.Controller.RedirectResultOnLogIn().Url));
        }
        #endregion
    }

    public class UserForTesting
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AuthenticationToken { get; set; }

        public UserForTesting() { }

        public UserForTesting(string UserName, string Password)
        {
            // Begin Create new User -> by default is only Guest Role Member
            this.UserName = UserName;
            this.Email = string.Format("{0}@valid.com", this.UserName);
            this.Password = Password;
            Guid UserNameValidActivationToken = Guid.Empty;

            ControllerFake<UserAccountController> controllerUserCreation = new ControllerFake<UserAccountController>();
            CommonTests.Register_Succeed(controllerUserCreation, this.Email, this.Password, ref UserNameValidActivationToken);
            CommonTests.Register_ActivateAccount(controllerUserCreation, UserNameValidActivationToken);
            CommonTests.Login_Succeed(controllerUserCreation, this.Email, this.Password);
            this.AuthenticationToken = HttpContext.Current.Response.Cookies[UserRequestModel_Keys.WcfFormsAuthenticationCookieName].Value;
            controllerUserCreation.Dispose();
        }
    }
}
