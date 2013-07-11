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
using $customNamespace$.Tests.Common.Actions;
using $customNamespace$.Tests.Common.Controllers;
using $customNamespace$.UI.Web.Areas.UserAccount;
using $customNamespace$.UI.Web.Areas.UserAccount.Controllers;
using $customNamespace$.UI.Web.Areas.UserAccount.Models;

namespace $safeprojectname$.TestCases.UserAccount
{
    [TestClass]
    public class UserAccountControllerTest : TestControllerBase<UserAccountAreaRegistration>
    {
        static ControllerFake<UserAccountController> controller;
        static string UserNameValid = Guid.NewGuid().ToString();
        static string UserEmailValid = string.Format("{0}@valid.com", UserNameValid);
        static string UserPassword = "123456";
        static Guid UserNameValidActivationToken;
        static Guid CantAccessMyAccountToken;
        static string UserNameValidUnActivated = Guid.NewGuid().ToString();
        static string UserEmailValidUnActivated = string.Format("{0}@valid.com", UserNameValidUnActivated);

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

            UserAccountControllerTest.controller = new ControllerFake<UserAccountController>();
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            controller.Dispose();
        }

        [TestMethod]
        public void UserAccountControllerTest_Security()
        {
            Expression<Func<ChangePasswordViewModel, ActionResult>> methodChangePassword = m => controller.Controller.ChangePassword(new ChangePasswordViewModel());
            Expression<Func<ChangePasswordViewModel, ActionResult>> dashBoard = m => controller.Controller.Dashboard();

            List<string> methodsWithAuthorizeAttributeExpected = new List<string>() { 
                (methodChangePassword.Body as MethodCallExpression).Method.Name 
                , (dashBoard.Body as MethodCallExpression).Method.Name
            };

            List<MethodInfo> methodsWithAuthorizeAttributeResulted = typeof(UserAccountController)
                                                                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                                                .Where(x => methodsWithAuthorizeAttributeExpected.Contains(x.Name) &&
                                                                            x.GetCustomAttributes(typeof($customNamespace$.UI.Web.Common.Mvc.Attributes.AuthorizeAttribute), true)
                                                                            .Count() == 1).ToList();

            Assert.AreEqual(true, methodsWithAuthorizeAttributeResulted.Count == methodsWithAuthorizeAttributeExpected.Count);
        }

        #region Register User

        [TestMethod]
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
            Assert.AreEqual(true, Helper<RegisterViewModel, string>.PropertyHasAttribute(model, expConfirm, typeof(CompareAttribute)));
        }

        [TestMethod]
        public void Register_InvalidEmail()
        {
            Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            model.Object.Email = "jordi.invalidMail.com";
            ActionResult resultInvalid = controller.Controller.Register(model.Object);
            bool invalidFound = ((RegisterViewModel)((ViewResult)resultInvalid).Model).Result.CreateStatus == MembershipCreateStatus.InvalidEmail;
            Assert.AreEqual(true, invalidFound);
        }

        [TestMethod]
        public void Register_InvalidPassword()
        {
            
            Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            model.Object.Email = UserEmailValid;
            model.Object.Password = string.Empty;
            ActionResult resultInvalid = controller.Controller.Register(model.Object);
            bool invalidFound = ((RegisterViewModel)((ViewResult)resultInvalid).Model).Result.CreateStatus == MembershipCreateStatus.InvalidPassword;
            Assert.AreEqual(true, invalidFound);
        }

        [TestMethod]
        public void Register_Succeed()
        {
            $customNamespace$.Tests.Common.MembershipServices.CommonTests.Register_Succeed(controller, UserEmailValid, UserPassword, ref UserAccountControllerTest.UserNameValidActivationToken);
            //Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            //model.Object.Email = UserEmailValid;
            //model.Object.Password = UserPassword;
            //ActionResult resultValid = controller.Controller.Register(model.Object);
            //bool validFound = ((RegisterViewModel)((ViewResult)resultValid).Model).Result.CreateStatus == MembershipCreateStatus.Success;
            //Assert.AreEqual(true, validFound);
            //UserNameValidActivationToken = ((RegisterViewModel)((ViewResult)resultValid).Model).Result.ActivateUserToken;
        }

        [TestMethod]
        public void Register_CreateUnActivatedAccount()
        {
            
            Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            model.Object.Email = UserEmailValidUnActivated;
            model.Object.Password = UserPassword;
            ActionResult resultValid = controller.Controller.Register(model.Object);
            bool validFound = ((RegisterViewModel)((ViewResult)resultValid).Model).Result.CreateStatus == MembershipCreateStatus.Success;
            Assert.AreEqual(true, validFound);
            //UserNameValidActivationToken = ((RegisterViewModel)((ViewResult)resultValid).Model).Result.ActivateUserToken;    // We will NOT activate this account
        }

        [TestMethod]
        public void Register_Duplicated()
        {
            
            Mock<RegisterViewModel> model = new Mock<RegisterViewModel>();
            model.Object.Email = UserEmailValid;
            model.Object.Password = UserPassword;
            ActionResult resultInvalid = controller.Controller.Register(model.Object);
            MembershipCreateStatus status = ((RegisterViewModel)((ViewResult)resultInvalid).Model).Result.CreateStatus;
            bool invalidFound = (status == MembershipCreateStatus.DuplicateEmail) || (status == MembershipCreateStatus.DuplicateUserName);
            Assert.AreEqual(true, invalidFound);
        }

        [TestMethod]
        public void Register_ActivateAccount_UnexistingToken()
        {
            
            ActionResult resultInvalid = controller.Controller.Activate(Guid.NewGuid().ToString());
            MembershipUserWrapper user = ((AccountActivationClientModel)((ViewResult)resultInvalid).Model).Result.Data.User;
            bool invalidFound = user == null;
            Assert.AreEqual(true, invalidFound);
        }

        [TestMethod]
        public void Register_ActivateAccount()
        {
            $customNamespace$.Tests.Common.MembershipServices.CommonTests.Register_ActivateAccount(controller, UserAccountControllerTest.UserNameValidActivationToken); 
            //ActionResult resultValid = controller.Controller.Activate(UserAccountControllerTest.UserNameValidActivationToken.ToString());
            //Assert.AreEqual(true, resultValid.GetType() == typeof(RedirectResult));
            //Assert.AreEqual(true, (((RedirectResult)resultValid).Url == controller.Controller.RedirectResultOnLogIn().Url));
        }

        #endregion

        #region Cant Access My Account

        [TestMethod]
        public void CantAccessMyAccount_Validation()
        {
            CantAccessYourAccountViewModel model = new CantAccessYourAccountViewModel() { EmailAddress = string.Empty };
            Expression<Func<CantAccessYourAccountViewModel, string>> expEmail = m => model.EmailAddress;
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expEmail)).GetCustomAttributes(typeof(DisplayAttribute), false).Count() > 0);
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expEmail)).GetCustomAttributes(typeof(RequiredAttribute), false).Count() > 0);
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expEmail)).GetCustomAttributes(typeof(EmailAttribute), false).Count() > 0);
        }

        [TestMethod]
        public void CantAccessMyAccount_UnexistingEmail()
        {
            
            ActionResult resultInvalid = controller.Controller.CantAccessYourAccount(new CantAccessYourAccountViewModel()
            {
                EmailAddress = "someUnexistingEmail@gmail.com"
            });
            Assert.AreEqual(true, ((CantAccessYourAccountViewModel)((ViewResult)resultInvalid).Model).Result.IsValid == false);
        }

        [TestMethod]
        public void CantAccessMyAccount_Succeed()
        {
            $customNamespace$.Tests.Common.MembershipServices.CommonTests.CantAccessMyAccount_Succeed(controller, UserEmailValid, ref UserAccountControllerTest.CantAccessMyAccountToken);
        }

        #endregion

        #region Reset Password
        [TestMethod]
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
            Assert.AreEqual(true, model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expConfirmPassword)).GetCustomAttributes(typeof(CompareAttribute), false).Count() > 0);
            Assert.AreEqual(true, Helper<ResetPasswordClientModel, string>.PropertyHasDataType(model, expConfirmPassword, DataType.Password));
        }

        [TestMethod]
        public void ResetPassword_InvalidPassword()
        {
            ResetPasswordClientModel model = new ResetPasswordClientModel();
            ActionResult actionResult = controller.Controller.ResetPassword(Guid.NewGuid().ToString(), model);
            Assert.AreEqual(false, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.IsValid);
            Assert.AreEqual(true, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.Message == Resources.Account.AccountResources.InvalidPassword);
        }

        [TestMethod]
        public void ResetPassword_InvalidConfirmPassword()
        {
            ResetPasswordClientModel model = new ResetPasswordClientModel() { NewPassword = "123456"};
            ActionResult actionResult = controller.Controller.ResetPassword(Guid.NewGuid().ToString(), model);
            Assert.AreEqual(false, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.IsValid);
            Assert.AreEqual(true, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.Message == Resources.Account.AccountResources.NewPasswordConfirmError);
        }

        [TestMethod]
        public void ResetPassword_InvalidToken()
        {
            ResetPasswordClientModel model = new ResetPasswordClientModel() { NewPassword = "123456", ConfirmPassword="123456" };
            ActionResult actionResult = controller.Controller.ResetPassword(Guid.NewGuid().ToString(), model);
            Assert.AreEqual(false, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.IsValid);
            Assert.AreEqual(true, ((ResetPasswordClientModel)((ViewResult)actionResult).Model).Result.Message == Resources.Account.AccountResources.CantAccessYourAccount_TokenExpired);
        }

        [TestMethod]
        public void ResetPassword_Succeed()
        {
            $customNamespace$.Tests.Common.MembershipServices.CommonTests.ResetPassword_Succeed(controller, UserEmailValid, CantAccessMyAccountToken, UserPassword);
        }

        #endregion

        #region Login

        [TestMethod]
        public void Login_InvalidCredentials()
        {
            LogOnViewModel logOnModel = new LogOnViewModel();
            logOnModel.Email = UserEmailValid;
            logOnModel.Password = "12345asd6";
            ActionResult resultPost = controller.Controller.LogOn(logOnModel);
            Assert.AreEqual(true, ((ViewResult)resultPost).ViewData.ModelState.ContainsKey("Invalid_Credentials"));
            Assert.AreEqual($customNamespace$.Resources.Account.AccountResources.UserNameOrPasswordInvalid, ((ViewResult)resultPost).ViewData.ModelState.Values.First().Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Login_UnActivatedUser()
        {
            LogOnViewModel logOnModel = new LogOnViewModel();
            logOnModel.Email = UserEmailValidUnActivated;
            logOnModel.Password = UserPassword;
            ActionResult resultPost = controller.Controller.LogOn(logOnModel);
            Assert.AreEqual(true, ((ViewResult)resultPost).ViewData.ModelState.ContainsKey("Invalid_Credentials"));
            Assert.AreEqual($customNamespace$.Resources.Account.AccountResources.UserNameOrPasswordInvalid, ((ViewResult)resultPost).ViewData.ModelState.Values.First().Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Login_Succeed()
        {
            $customNamespace$.Tests.Common.MembershipServices.CommonTests.Login_Succeed(controller, UserEmailValid, UserPassword);
        }

        #endregion
    }
}
