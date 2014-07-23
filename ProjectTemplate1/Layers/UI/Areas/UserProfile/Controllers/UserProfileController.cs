using System.Web.Mvc;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Configuration.ConfigSections.ClientResources;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.Unity;
using $customNamespace$.UI.Web.Areas.UserProfile.Models;
using $customNamespace$.UI.Web.Controllers;

namespace $customNamespace$.UI.Web.Areas.UserProfile.Controllers
{
    [$customNamespace$.UI.Web.Common.Mvc.Attributes.Authorize]
    public class UserProfileController : Controller, IControllerWithClientResources
    {
        protected override void Dispose(bool disposing)
        {
            if (this.ProviderProfile != null)
            {
                this.ProviderProfile.Dispose();
            }

            base.Dispose(disposing);
        }

        public string[] GetControllerJavascriptResources
        {
            get { return new string[0]; }
        }

        public string[] GetControllerStyleSheetResources
        {
            get { return new string[0]; }
        }

        private IProfileProxy ProviderProfile;

        public UserProfileController()
        {
            this.ProviderProfile = DependencyFactory.Resolve<IProfileProxy>();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(UserProfileIndexModel model)
        {
            model.BaseViewModelInfo.Title = Resources.Account.AccountResources.UserProfile;
            model.UserProfileResult = this.ProviderProfile.Get();
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Edit(UserProfileIndexModel model)
        {
            model.BaseViewModelInfo.Title = Resources.Account.AccountResources.UserProfile;

            if (this.RequestType() == HttpVerbs.Get)
            {
                DataResultUserProfile userProfileResult = this.ProviderProfile.Get();
                model.UserProfileResult = userProfileResult;
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // Set Culture & Theme values currently in use
                    model.UserProfileResult.Data.CultureName = MvcApplication.UserRequest.UserProfile.CultureName;
                    model.UserProfileResult.Data.Theme = MvcApplication.UserRequest.UserProfile.Theme;

                    DataResultUserProfile result = this.ProviderProfile.Update(model.UserProfileResult.Data);
                    model.UserProfileResultUpdated = result;
                    MvcApplication.UserRequest.UserProfile = result.Data;
                    MvcApplication.UserRequest.UserProfile.ApplyClientProperties();
                    model.BaseViewModelInfo.LocalizationResources = new LocalizationResourcesHelper(MvcApplication.UserRequest.UserProfile.Culture);
                }
                return View(model);
            }
        }
    }

}