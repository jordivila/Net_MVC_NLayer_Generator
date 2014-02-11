using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using $customNamespace$.Models.DataAnnotationsAttributes;
using $customNamespace$.Models.Membership;
using $customNamespace$.UI.Web.Areas.UserAccount;
using $customNamespace$.UI.Web.Areas.UserAccount.Controllers;
using $customNamespace$.UI.Web.Areas.UserAccount.Models;
using $customNamespace$.Resources.Account;
using $customNamespace$.Tests.Integration.Common;
using $customNamespace$.Tests.Integration.Common.Controllers;
using $customNamespace$.Tests.Integration.Common.Actions;

namespace $customNamespace$.Tests.Integration.TestCases.UserAccount
{
    [TestClass]
    public class UserAccountControllerTest : TestControllerBase<UserAccountAreaRegistration>
    {
        //static ControllerFake<UserAccountController> controller;
        static string UserNameValid = Guid.NewGuid().ToString();
        static string UserEmailValid = string.Format("{0}@valid.com", UserNameValid).Replace("-", string.Empty);
        static string UserPassword = "123456";
        static Guid UserNameValidActivationToken;
        static Guid CantAccessMyAccountToken;
        static string UserNameValidUnActivated = Guid.NewGuid().ToString();
        static string UserEmailValidUnActivated = string.Format("{0}@valid.com", UserNameValidUnActivated);

        [TestMethod]
        public void UserAccountControllerTest_Security()
        {
            ControllerFake<UserAccountController, ChangePasswordViewModel> controller = new ControllerFake<UserAccountController, ChangePasswordViewModel>();

            Expression<Func<ChangePasswordViewModel, ActionResult>> methodChangePassword = m => controller.Controller.ChangePassword(new ChangePasswordViewModel());
            Expression<Func<ChangePasswordViewModel, ActionResult>> dashBoard = m => controller.Controller.Dashboard();

            List<string> methodsWithAuthorizeAttributeExpected = new List<string>() { 
                (methodChangePassword.Body as MethodCallExpression).Method.Name 
                , (dashBoard.Body as MethodCallExpression).Method.Name
            };

            List<MethodInfo> methodsWithAuthorizeAttributeResulted = typeof(UserAccountController)
                                                                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                                                .Where(x => methodsWithAuthorizeAttributeExpected.Contains(x.Name) &&
                                                                            x.GetCustomAttributes(typeof(UI.Web.Common.Mvc.Attributes.AuthorizeAttribute), true)
                                                                            .Count() == 1).ToList();

            Assert.AreEqual(true, methodsWithAuthorizeAttributeResulted.Count == methodsWithAuthorizeAttributeExpected.Count);
        }


        [TestMethod]
        public void UserAccountControllerTest_ShouldPass()
        {
            this.Register_ShouldPass();
            this.CantAccessMyACcount_ShouldPass();
            this.ResetPassword_ShouldPass();
            this.Login_ShouldPass();
        }

        public void Register_ShouldPass()
        {
            this.Register_ModelValidation();
            this.Register_InvalidEmail();
            this.Register_InvalidPassword();
            this.Register_Succeed();
            this.Register_CreateUnActivatedAccount();
            this.Register_Duplicated();
            this.Register_ActivateAccount_UnexistingToken();
            this.Register_ActivateAccount();
        }

        #region Register User

        public void Register_ModelValidation()
        {
            RegisterViewModel model = new RegisterViewModel() { Email = "some@mail.com", ConfirmPassword = "123456", Password = "123456" };

            Expression<Func<RegisterViewModel, string>> expEmail = m => model.Email;
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasAttribute(model, expEmail, typeof(DisplayAttribute)));
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasAttribute(model, expEmail, typeof(RequiredAttribute)));
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasAttribute(model, expEmail, typeof(EmailAttribute)));

            Expression<Func<RegisterViewModel, string>> expPassword = m => model.Password;
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasAttribute(model, expPassword, typeof(DisplayAttribute)));
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasAttribute(model, expPassword, typeof(RequiredAttribute)));
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasDataType(model, expPassword, DataType.Password));

            Expression<Func<RegisterViewModel, string>> expConfirm = m => model.ConfirmPassword;
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasAttribute(model, expConfirm, typeof(DisplayAttribute)));
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasAttribute(model, expConfirm, typeof(RequiredAttribute)));
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasDataType(model, expConfirm, DataType.Password));
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasAttribute(model, expConfirm, typeof(System.ComponentModel.DataAnnotations.CompareAttribute)));
        }
        public void Register_InvalidEmail()
        {
            ControllerFake<UserAccountController, RegisterViewModel> controller = new ControllerFake<UserAccountController, RegisterViewModel>();
            Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            model.Object.Email = "jordi.invalidMail.com";
            ActionResult resultInvalid = controller.Controller.Register(model.Object);
            bool invalidFound = ((RegisterViewModel)((ViewResult)resultInvalid).Model).Result.CreateStatus == MembershipCreateStatus.InvalidEmail;
            Assert.AreEqual(true, invalidFound);
        }
        public void Register_InvalidPassword()
        {
            ControllerFake<UserAccountController, RegisterViewModel> controller = new ControllerFake<UserAccountController, RegisterViewModel>();
            Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            model.Object.Email = UserEmailValid;
            model.Object.Password = string.Empty;
            ActionResult resultInvalid = controller.Controller.Register(model.Object);
            bool invalidFound = ((RegisterViewModel)((ViewResult)resultInvalid).Model).Result.CreateStatus == MembershipCreateStatus.InvalidPassword;
            Assert.AreEqual(true, invalidFound);
        }
        public void Register_Succeed()
        {
            CommonTests.Register_Succeed(UserEmailValid, UserPassword, ref UserAccountControllerTest.UserNameValidActivationToken);
        }
        public void Register_CreateUnActivatedAccount()
        {
            ControllerFake<UserAccountController, RegisterViewModel> controller = new ControllerFake<UserAccountController, RegisterViewModel>();
            Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            model.Object.Email = UserEmailValidUnActivated;
            model.Object.Password = UserPassword;
            ActionResult resultValid = controller.Controller.Register(model.Object);
            bool validFound = ((RegisterViewModel)((ViewResult)resultValid).Model).Result.CreateStatus == MembershipCreateStatus.Success;
            Assert.AreEqual(true, validFound);
            //UserNameValidActivationToken = ((RegisterViewModel)((ViewResult)resultValid).Model).Result.ActivateUserToken;    // We will NOT activate this account
        }
        public void Register_Duplicated()
        {
            ControllerFake<UserAccountController, RegisterViewModel> controller = new ControllerFake<UserAccountController, RegisterViewModel>();
            Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            model.Object.Email = UserEmailValid;
            model.Object.Password = UserPassword;
            ActionResult resultInvalid = controller.Controller.Register(model.Object);
            MembershipCreateStatus status = ((RegisterViewModel)((ViewResult)resultInvalid).Model).Result.CreateStatus;
            bool invalidFound = (status == MembershipCreateStatus.DuplicateEmail) || (status == MembershipCreateStatus.DuplicateUserName);
            Assert.AreEqual(true, invalidFound);
        }
        public void Register_ActivateAccount_UnexistingToken()
        {
            ControllerFake<UserAccountController, object> controller = new ControllerFake<UserAccountController, object>();
            ActionResult resultInvalid = controller.Controller.Activate(Guid.NewGuid().ToString());
            MembershipUserWrapper user = ((AccountActivationClientModel)((ViewResult)resultInvalid).Model).Result.Data.User;
            bool invalidFound = user == null;
            Assert.AreEqual(true, invalidFound);
        }
        public void Register_ActivateAccount()
        {
            CommonTests.Register_ActivateAccount(UserAccountControllerTest.UserNameValidActivationToken);
        }

        #endregion

        public void CantAccessMyACcount_ShouldPass()
        {
            this.CantAccessMyAccount_Validation();
            this.CantAccessMyAccount_UnexistingEmail();
            this.CantAccessMyAccount_Succeed();
        }

        #region Cant Access My Account

        public void CantAccessMyAccount_Validation()
        {
            CantAccessYourAccountViewModel model = new CantAccessYourAccountViewModel() { EmailAddress = string.Empty };
            Expression<Func<CantAccessYourAccountViewModel, string>> expEmail = m => model.EmailAddress;
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expEmail)).GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0);
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expEmail)).GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0);
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expEmail)).GetCustomAttributes(typeof(EmailAttribute), false).Count() > 0);
        }
        public void CantAccessMyAccount_UnexistingEmail()
        {
            ControllerFake<UserAccountController, CantAccessYourAccountViewModel> controller = new ControllerFake<UserAccountController, CantAccessYourAccountViewModel>();
            ActionResult resultInvalid = controller.Controller.CantAccessYourAccount(new CantAccessYourAccountViewModel()
            {
                EmailAddress = "someUnexistingEmail@gmail.com"
            });
            Assert.AreEqual(true, ((CantAccessYourAccountViewModel)((ViewResult)resultInvalid).Model).Result.IsValid == false);
        }
        public void CantAccessMyAccount_Succeed()
        {
            CommonTests.CantAccessMyAccount_Succeed(UserEmailValid, ref UserAccountControllerTest.CantAccessMyAccountToken);
        }

        #endregion

        public void ResetPassword_ShouldPass()
        {
            this.ResetPassword_Validation();
            this.ResetPassword_InvalidPassword();
            this.ResetPassword_InvalidConfirmPassword();
            this.ResetPassword_InvalidToken();
            this.ResetPassword_Succeed();
        }

        #region Reset Password

        public void ResetPassword_Validation()
        {
            ResetPasswordClientModel model = new ResetPasswordClientModel() { NewPassword = "123456", ConfirmPassword = "123456" };

            Expression<Func<ResetPasswordClientModel, string>> expPassword = m => model.NewPassword;
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expPassword)).GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0);
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expPassword)).GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0);
            Assert.AreEqual(true, Helper<ResetPasswordClientModel, string>.PropertyHasDataType(model, expPassword, DataType.Password));

            Expression<Func<ResetPasswordClientModel, string>> expConfirmPassword = m => model.ConfirmPassword;
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expConfirmPassword)).GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0);
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expConfirmPassword)).GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0);
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expConfirmPassword)).GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.CompareAttribute), false).Count() > 0);
            Assert.AreEqual(true, Helper<ResetPasswordClientModel, string>.PropertyHasDataType(model, expConfirmPassword, DataType.Password));
        }
        public void ResetPassword_InvalidPassword()
        {
            ControllerFake<UserAccountController, ResetPasswordClientModel> controller = new ControllerFake<UserAccountController, ResetPasswordClientModel>();
            ResetPasswordClientModel model = new ResetPasswordClientModel();
            ActionResult actionResult = controller.Controller.ResetPassword(Guid.NewGuid().ToString(), model);
            Assert.AreEqual(false, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.IsValid);
            Assert.AreEqual(true, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.Message == Resources.Account.AccountResources.InvalidPassword);
        }
        public void ResetPassword_InvalidConfirmPassword()
        {
            ControllerFake<UserAccountController, ResetPasswordClientModel> controller = new ControllerFake<UserAccountController, ResetPasswordClientModel>();
            ResetPasswordClientModel model = new ResetPasswordClientModel() { NewPassword = "123456" };
            ActionResult actionResult = controller.Controller.ResetPassword(Guid.NewGuid().ToString(), model);
            Assert.AreEqual(false, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.IsValid);
            Assert.AreEqual(true, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.Message == Resources.Account.AccountResources.NewPasswordConfirmError);
        }
        public void ResetPassword_InvalidToken()
        {
            ControllerFake<UserAccountController, ResetPasswordClientModel> controller = new ControllerFake<UserAccountController, ResetPasswordClientModel>();
            ResetPasswordClientModel model = new ResetPasswordClientModel() { NewPassword = "123456", ConfirmPassword = "123456" };
            ActionResult actionResult = controller.Controller.ResetPassword(Guid.NewGuid().ToString(), model);
            Assert.AreEqual(false, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.IsValid);
            Assert.AreEqual(true, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.Message == Resources.Account.AccountResources.CantAccessYourAccount_TokenExpired);
        }
        public void ResetPassword_Succeed()
        {
            CommonTests.ResetPassword_Succeed(UserEmailValid, CantAccessMyAccountToken, UserPassword);
        }

        #endregion

        public void Login_ShouldPass()
        {
            this.Login_InvalidCredentials();
            this.Login_UnActivatedUser();
            this.Login_Succeed();
        }

        #region Login

        public void Login_InvalidCredentials()
        {
            ControllerFake<UserAccountController, LogOnViewModel> controller = new ControllerFake<UserAccountController, LogOnViewModel>();
            LogOnViewModel logOnModel = new LogOnViewModel();
            logOnModel.Email = UserEmailValid;
            logOnModel.Password = "12345asd6";
            ActionResult resultPost = controller.Controller.LogOn(logOnModel);
            Assert.AreEqual(true, ((ViewResult)resultPost).ViewData.ModelState.ContainsKey("Invalid_Credentials"));
            Assert.AreEqual(AccountResources.UserNameOrPasswordInvalid, ((ViewResult)resultPost).ViewData.ModelState.Values.First().Errors[0].ErrorMessage);
        }
        public void Login_UnActivatedUser()
        {
            ControllerFake<UserAccountController, LogOnViewModel> controller = new ControllerFake<UserAccountController, LogOnViewModel>();
            LogOnViewModel logOnModel = new LogOnViewModel();
            logOnModel.Email = UserEmailValidUnActivated;
            logOnModel.Password = UserPassword;
            ActionResult resultPost = controller.Controller.LogOn(logOnModel);
            Assert.AreEqual(true, ((ViewResult)resultPost).ViewData.ModelState.ContainsKey("Invalid_Credentials"));
            Assert.AreEqual(AccountResources.UserNameOrPasswordInvalid, ((ViewResult)resultPost).ViewData.ModelState.Values.First().Errors[0].ErrorMessage);
        }
        public void Login_Succeed()
        {
            CommonTests.Login_Succeed(UserEmailValid, UserPassword);
        }

        #endregion
    }
}
