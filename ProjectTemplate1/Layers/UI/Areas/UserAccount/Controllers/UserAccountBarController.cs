using System.Web.Mvc;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.Unity;
using $safeprojectname$.Controllers;
using $safeprojectname$.Models;

namespace $safeprojectname$.Areas.UserAccount.Controllers
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

        private IProviderProfile providerProfile;

        public UserAccountBarController() 
        {
            using (DependencyFactory dependencyFactory = new DependencyFactory())
            {
                providerProfile = dependencyFactory.Unity.Resolve<IProviderProfile>();
            }
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

            //if ($customNamespace$.Models.Configuration.ApplicationConfiguration.IsDebugMode)
            //{
                //System.Threading.Thread.Sleep(5000);
            //}

            if(model.BaseViewModelInfo.UserIsLoggedIn)
            {
                model.BaseViewModelInfo.UserProfile = this.providerProfile.Get().Data;
            }

            return View("~/Areas/UserAccount/Views/UserAccount/UserAccountBar.cshtml", model);
        }

    }
}
