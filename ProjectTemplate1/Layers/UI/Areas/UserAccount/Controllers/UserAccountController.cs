using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Authentication;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Membership;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.Unity;
using $customNamespace$.Resources.Account;
using $safeprojectname$.Areas.Home;
using $safeprojectname$.Areas.LogViewer;
using $safeprojectname$.Areas.Test;
using $safeprojectname$.Areas.UserAccount.Models;
using $safeprojectname$.Areas.UserAdministration;
using $safeprojectname$.Areas.UserProfile;
using $safeprojectname$.Controllers;
using $safeprojectname$.Models;

namespace $safeprojectname$.Areas.UserAccount.Controllers
{
    public class UserAccountController : Controller, IControllerWithClientResources
    {
        public UserAccountController() 
        {
            this.FormsAuthenticationService = DependencyFactory.Resolve<IAuthenticationProxy>();
            this.FormsMembershipService = DependencyFactory.Resolve<IMembershipProxy>();
            this.FormsProfileService = DependencyFactory.Resolve<IProfileProxy>();
        }

        public string[] GetControllerJavascriptResources
        {
            get
            {
                return new string[0];
                //return new string[3] {"~/Areas/UserAccount/Content/$customNamespace$.WCF.UserAccount.js"
                //                                            ,"~/Areas/UserAccount/Content/$customNamespace$.Widget.LogOn.js"
                //                                            , "~/Areas/UserAccount/Content/$customNamespace$.Widget.logOnRegister.js" 
                //};
            }
        }
        public string[] GetControllerStyleSheetResources
        {
            get
            {
                return new string[1] { "~/Areas/UserAccount/Content/UserAccount.css" };
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (this.FormsAuthenticationService != null)
            {
                this.FormsAuthenticationService.Dispose();
            }

            if (this.FormsMembershipService != null)
            {
                this.FormsMembershipService.Dispose();
            }

            if (this.FormsProfileService != null)
            {
                this.FormsProfileService.Dispose();
            }

            base.Dispose(disposing);
        }

        public IAuthenticationProxy FormsAuthenticationService;
        public IMembershipProxy FormsMembershipService;
        public IProfileProxy FormsProfileService;

        public RedirectResult RedirectResultOnLogIn()
        {
            return new RedirectResult(Url.Account_Dashboard());
        }

        #region Login / Logout
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult LogOn(LogOnViewModel model)
        {
            if (this.RequestType() == HttpVerbs.Get)
            {
                model = new LogOnViewModel();
                model.BaseViewModelInfo.Title = $customNamespace$.Resources.General.GeneralTexts.LogOn;
                model.UrlReferer = Request.UrlReferrer == null ? string.Empty : Request.UrlReferrer.ToString();
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    bool isValidUser = this.FormsAuthenticationService.LogIn(model.Email, model.Password, string.Empty, false); //model.RememberMe);
                    if (isValidUser)
                    {
                        this.FormsProfileService.Get().Data.ApplyClientProperties();

                        if (Url.IsLocalUrl(model.UrlReferer))   // Prevents Open Redirection Attacks
                        {
                            return new RedirectResult(model.UrlReferer);
                        }
                        else
                        {
                            return this.RedirectResultOnLogIn();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Invalid_Credentials", AccountResources.UserNameOrPasswordInvalid);
                    }
                }
                model.BaseViewModelInfo.Title = $customNamespace$.Resources.General.GeneralTexts.LogOn;
                return View(model);
            }

        }
        public ActionResult LogOff()
        {
            FormsAuthenticationService.LogOut();
            return new RedirectResult(Url.Home_Index());
        }
        #endregion

        #region Register
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Register(RegisterViewModel model)
        {
            if (this.RequestType() == HttpVerbs.Get)
            {
                model = new RegisterViewModel();
                model.BaseViewModelInfo.Title = Resources.Account.AccountResources.Register;
                model.PasswordLength = this.FormsMembershipService.Settings().Data.MinRequiredPasswordLength;
                return View(model);
            }

            if (ModelState.IsValid)
            {
                DataResultUserCreateResult result = FormsMembershipService.CreateUser(model.Email, model.Password, model.Email, string.Empty, string.Empty, Url.Account_Activate());
                if (result.IsValid)
                {
                    model.Result = result.Data;

                    if (model.Result.CreateStatus == MembershipCreateStatus.Success)
                    {

                    }
                    else
                    {
                        ModelState.AddModelError(((int)model.Result.CreateStatus).ToString(), AccountValidation.ErrorCodeToString(model.Result.CreateStatus));
                    }
                }
                else
                {
                    ModelState.AddModelError("0", result.Message);
                }
            }
            model.BaseViewModelInfo.Title = Resources.Account.AccountResources.Register;
            model.PasswordLength = this.FormsMembershipService.Settings().Data.MinRequiredPasswordLength;
            return View(model);
        }
        #endregion

        #region CantAccessYourAccount
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CantAccessYourAccount(CantAccessYourAccountViewModel model)
        {
            if (this.RequestType() == HttpVerbs.Get)
            {
                model = new CantAccessYourAccountViewModel();
                model.BaseViewModelInfo.Title = $customNamespace$.Resources.Account.AccountResources.CanNotAccessYourAccount;
                return View(model);
            }

            model.BaseViewModelInfo.Title = $customNamespace$.Resources.Account.AccountResources.CanNotAccessYourAccount;
            if (ModelState.IsValid)
            {
                DataResultUserCantAccess result = this.FormsMembershipService.CantAccessYourAccount(Url.Account_ResetPassword(), model.EmailAddress);
                model.Result = result;

                if (result.IsValid)
                {

                }
                else
                {
                    ModelState.AddModelError("0", result.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region ResetPassword
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ResetPassword(object id, ResetPasswordClientModel model)
        {
            if (this.RequestType() == HttpVerbs.Get)
            {
                model = new ResetPasswordClientModel();
                model.BaseViewModelInfo.Title = $customNamespace$.Resources.Account.AccountResources.ChangePassword;
                model.MinPasswordLength = this.FormsMembershipService.Settings().Data.MinRequiredPasswordLength;
                return View(model);
            }

            model.BaseViewModelInfo.Title = $customNamespace$.Resources.Account.AccountResources.ChangePassword;
            model.MinPasswordLength = this.FormsMembershipService.Settings().Data.MinRequiredPasswordLength;

            if (ModelState.IsValid)
            {
                DataResultUserCantAccess result = this.FormsMembershipService.ResetPassword(Guid.Parse((string)id), model.NewPassword, model.ConfirmPassword);
                model.Result = new DataResultBoolean()
                {
                    IsValid = result.IsValid,
                    Message = result.Message,
                    MessageType = result.MessageType,
                    Data = result.IsValid
                };
                if (result.IsValid)
                {
                    return this.RedirectResultOnLogIn();
                }
                else
                {
                    ModelState.AddModelError("InvalidPassword", result.Message);
                }
            }
            return View(model);

        }
        #endregion

        #region ChangePassword
        [$safeprojectname$.Common.Mvc.Attributes.Authorize]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            Func<ChangePasswordViewModel, ChangePasswordViewModel> setCommonValues = delegate(ChangePasswordViewModel modelTmp)
            {
                modelTmp.BaseViewModelInfo.Title = $customNamespace$.Resources.Account.AccountResources.ChangePassword;
                modelTmp.MinPasswordLength = this.FormsMembershipService.Settings().Data.MinRequiredPasswordLength;
                return modelTmp;
            };

            if (this.RequestType() == HttpVerbs.Get)
            {
                model = setCommonValues(new ChangePasswordViewModel());
                return View(model);
            }
            else
            {
                model = setCommonValues(model);

                if (ModelState.IsValid)
                {
                    FormsIdentity identity = this.FormsAuthenticationService.GetFormsIdentity();
                    DataResultBoolean result = this.FormsMembershipService.ChangePassword(identity.Name, model.OldPassword, model.NewPassword, model.ConfirmPassword);
                    if (result.IsValid)
                    {
                        model.Result = result;
                    }
                    else
                    {
                        ModelState.AddModelError("InvalidPassword", result.Message);
                    }
                }
                return View(model);
            }
        }
        #endregion

        #region Activation
        public ActionResult Activate(object id)
        {
            AccountActivationClientModel model = new AccountActivationClientModel();
            model.ActivateUserToken = Guid.Parse((string)id);
            DataResultUserActivate result = this.FormsMembershipService.ActivateAccount(Guid.Parse((string)id));
            if (result.IsValid)
            {
                return this.RedirectResultOnLogIn();
            }
            else
            {
                model.Result = result;
                return View(model);
            }
        }
        #endregion

        #region DashBoard
        [$safeprojectname$.Common.Mvc.Attributes.Authorize]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Dashboard()
        {
            DashBoardViewModel model = new DashBoardViewModel()
            {
                UserProfile = this.FormsProfileService.Get().Data,
                SiteAdminMenu = new MenuModel()
                {
                    MenuItems = new List<MenuItemModel>() { 
                        new MenuItemModel(){DataAction = UrlHelperUserAdmin.UserAdminIndex(this.Url), Description = Resources.General.GeneralTexts.UserAdmin },
                        new MenuItemModel(){DataAction = LogViewerUrlHelper.LogViewer(this.Url), Description = Resources.General.GeneralTexts.LogViewer },
                        new MenuItemModel(){DataAction = TestsUrlHelper.Index(this.Url),Description = "UI Tests"}
                    }
                },
                ProfileMenu = new MenuModel()
                {
                    MenuItems = new List<MenuItemModel>() { 
                        new MenuItemModel(){DataAction = UserAccountUrlHelper.Account_ChangePassword(this.Url), Description = AccountResources.ChangePassword},
                        new MenuItemModel(){DataAction = UrlHelperUserProfile.UserProfile_Edit(this.Url), Description = Resources.Account.AccountResources.ProfileEdit}
                    }
                }
            };
            
            return View(model);
        }
        #endregion
    }
}
