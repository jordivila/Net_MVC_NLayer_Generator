using System.Web.Mvc;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.Unity;
using $customNamespace$.UI.Web.Controllers;
using $customNamespace$.UI.Web.Models;

namespace $customNamespace$.UI.Web.Areas.UserAccount.Controllers
{
    public class UserAccountBarController : Controller, IControllerWithClientResources
    {
        public string[] GetControllerJavascriptResources
        {
            get { return new string[0]; }
        }

        public string[] GetControllerStyleSheetResources
        {
            get { return new string[0]; }
        }

        private IProfileProxy providerProfile;

        public UserAccountBarController()
        {
            this.providerProfile = DependencyFactory.Resolve<IProfileProxy>();
        }

        protected override void Dispose(bool disposing)
        {
            if (this.providerProfile != null)
            {
                this.providerProfile.Dispose();
            }

            base.Dispose(disposing);
        }

        public ActionResult UserAccountBar()
        {
            baseViewModel model = new baseViewModel();

            if (model.BaseViewModelInfo.UserIsLoggedIn)
            {
                model.BaseViewModelInfo.UserProfile = this.providerProfile.Get().Data;
            }

            return View("~/Areas/UserAccount/Views/UserAccount/UserAccountBar.cshtml", model);
        }

    }
}
