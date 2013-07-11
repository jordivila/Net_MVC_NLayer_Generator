using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.DataAnnotationsAttributes;
using $customNamespace$.Models.Membership;
using $customNamespace$.Models.Profile;
using $customNamespace$.Resources.Account;
using $customNamespace$.Resources.DataAnnotations;
using $customNamespace$.Resources.Helpers.GeneratedResxClasses;
using $safeprojectname$.Models;
using $safeprojectname$.Common.Mvc.Attributes;

namespace $safeprojectname$.Areas.UserAccount.Models
{
    [NonValidateModelOnHttpGet]
    public class ChangePasswordViewModel: baseViewModel
    {
        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.OldPassword)]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.NewPassword)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.NewPasswordConfirm)]
        [Compare("NewPassword", ErrorMessageResourceType = typeof($customNamespace$.Resources.Account.AccountResources), ErrorMessageResourceName = AccountResourcesKeys.NewPasswordConfirmError)]
        public string ConfirmPassword { get; set; }

        public int MinPasswordLength { get; set; }

        public DataResultBoolean Result { get; set; }
    }

    [NonValidateModelOnHttpGet]
    public class LogOnViewModel: baseViewModel
    {
        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [Email(ErrorMessageResourceName = DataAnnotationsResourcesKeys.EmailAddressAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.EmailAddress)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.Password)]
        public string Password { get; set; }

        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.RememberMe)]
        public bool RememberMe { get; set; }

        public string UrlReferer { get; set; }
    }

    [NonValidateModelOnHttpGet]
    public class RegisterViewModel: baseViewModel
    {
        //[Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        //[Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.UserName)]
        //public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [Email(ErrorMessageResourceName = DataAnnotationsResourcesKeys.EmailAddressAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [Display(ResourceType=typeof($customNamespace$.Resources.Account.AccountResources), Name= AccountResourcesKeys.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof($customNamespace$.Resources.Account.AccountResources), ErrorMessageResourceName = AccountResourcesKeys.NewPasswordConfirmError)]
        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.ConfirmPassword)]
        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        public string ConfirmPassword { get; set; }

        public int PasswordLength { get; set; }

        public CreatedAccountResultModel Result { get; set; }
    }

    [NonValidateModelOnHttpGet]
    public class CantAccessYourAccountViewModel: baseViewModel
    {
        [Required(ErrorMessageResourceName = DataAnnotationsResourcesKeys.ModelBinderConfig_ValueRequired, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [Email(ErrorMessageResourceName = DataAnnotationsResourcesKeys.EmailAddressAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [Display(ResourceType = typeof(AccountResources),  Name = AccountResourcesKeys.EmailAddress)]
        public string EmailAddress { get; set; }

        public DataResultUserCantAccess Result { get; set; }
    }

    [NonValidateModelOnHttpGet]
    public class AccountActivationClientModel : baseViewModel
    {
        public Guid ActivateUserToken { get; set; }
        public DataResultUserActivate Result { get; set; }
    }

    [NonValidateModelOnHttpGet]
    public class ResetPasswordClientModel : baseViewModel
    {
        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.NewPassword)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof($customNamespace$.Resources.Account.AccountResources), Name = AccountResourcesKeys.NewPasswordConfirm)]
        [Compare("NewPassword", ErrorMessageResourceType = typeof($customNamespace$.Resources.Account.AccountResources), ErrorMessageResourceName = AccountResourcesKeys.NewPasswordConfirmError)]
        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        public string ConfirmPassword { get; set; }

        public int MinPasswordLength { get; set; }

        public DataResultBoolean Result { get; set; }
    }

    [NonValidateModelOnHttpGet]
    public class DashBoardViewModel : baseViewModel
    {
        public MenuModel SiteAdminMenu { get; set; }
        public MenuModel ProfileMenu { get; set; }
        public UserProfileModel UserProfile { get; set; }
    }

    [NonValidateModelOnHttpGet]
    public class UserAccountBarModel
    { 
        
    }
}