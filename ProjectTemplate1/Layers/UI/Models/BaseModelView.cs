using System;
using $customNamespace$.Models;
using $customNamespace$.Models.Configuration.ConfigSections.ClientResources;
using $customNamespace$.Models.Profile;
using $customNamespace$.UI.Web.Controllers;

namespace $customNamespace$.UI.Web.Models
{
    public class baseViewModel : baseModel, IDisposable
    {
        private baseViewModelInfo _baseViewModelInfo;
        public baseViewModelInfo BaseViewModelInfo
        {
            get 
            {
                if (this._baseViewModelInfo == null)
                {
                    this._baseViewModelInfo = new baseViewModelInfo();
                }
                return _baseViewModelInfo; 
            }
        }
        public void Dispose()
        {
            this._baseViewModelInfo.Dispose();
        }
    }

    interface IBaseViewModel
    {
        LocalizationResourcesHelper LocalizationResources { get; }
        bool IsUserIntendedController { get; }
        string Title { get; set; }
        string UserFormsIdentityName { get; }
        bool UserIsLoggedIn { get; }
        DateTime? UserLastLogin { get; }
        bool IsMobileDevice { get; }
        UserProfileModel UserProfile { get; }
    }

    public class baseViewModelInfo : IBaseViewModel, IDisposable
    {
        public baseViewModelInfo() 
        {
            this.LocalizationResources = new LocalizationResourcesHelper(MvcApplication.UserRequest.UserProfile.Culture);
            this.Breadcrumb = new Breadcrumb();
        }
        public string Title { get; set; }
        public bool IsUserIntendedController
        {
            get
            {
                return ControllerHelper.IsUserIntendedController(Controllers.ControllerHelper.GetControllerTypeForCurrentRequest());
            }
        }
        public string UserFormsIdentityName
        {
            get
            {
                if (this.IsUserIntendedController)
                {
                    return MvcApplication.UserRequest.UserFormsIdentity.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public bool UserIsLoggedIn
        {
            get
            {
                if (this.IsUserIntendedController)
                {
                    return MvcApplication.UserRequest.UserIsLoggedIn;
                }
                else
                {
                    return false;
                }
            }
        }
        public DateTime? UserLastLogin
        {
            get
            {
                if (this.IsUserIntendedController)
                {
                    return DateTime.Now;
                }
                else
                {
                    return null;
                }
            }
        }
        public LocalizationResourcesHelper LocalizationResources { get; set; }
        public bool IsMobileDevice 
        {
            get
            {
                //We could add here some logic in order to let mobile devices tu use desktop view
                return System.Web.HttpContext.Current.Request.Browser.IsMobileDevice;
            }
        }
        public Breadcrumb Breadcrumb { get; set; }
        public UserProfileModel _userProfile = null;
        public UserProfileModel UserProfile
        {
            get 
            {
                if (this._userProfile == null)
                {
                    return MvcApplication.UserRequest.UserProfile;
                }
                else
                {
                    return this._userProfile;
                }
            }
            set
            {
                this._userProfile = value;
            }
        }
        public void Dispose()
        {
            this.LocalizationResources.Dispose();
            this.Breadcrumb.Dispose();
            if (this._userProfile != null)
            {
                this._userProfile.Dispose();
            }
        }
    }
}