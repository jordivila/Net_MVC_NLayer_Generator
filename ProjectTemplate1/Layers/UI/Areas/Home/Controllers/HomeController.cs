using System;
using System.Web.Mvc;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Globalization;
using $customNamespace$.UI.Web.Controllers;
using $customNamespace$.UI.Web.Models;

namespace $customNamespace$.UI.Web.Areas.Home.Controllers
{
    [Serializable]
    public class HomeController : Controller, IControllerWithClientResources
    {
        public string[] GetControllerJavascriptResources
        {
            get { return new string[0]; }
        }

        public string[] GetControllerStyleSheetResources
        {
            get { return new string[0]; }
        }

        public ActionResult Index()
        {
            baseViewModel model = new baseViewModel();
            model.BaseViewModelInfo.Title = "Asp.Net MVC N-Tier application template";
            return View(model);
        }

        public ActionResult About()
        {
            baseViewModel model = new baseViewModel();
            model.BaseViewModelInfo.Title = "About";
            return View(model);
        }

        private ActionResult SettingsApplied()
        {
            baseViewModel model = new baseViewModel();
            model.BaseViewModelInfo.Title = "Settings applied";
            return View(model);
        }

        public ActionResult ThemeSet(string id)
        {
            ThemesAvailable themeSelected = (ThemesAvailable)Enum.Parse(typeof(ThemesAvailable), id);
            MvcApplication.UserRequest.UserProfile.Theme = themeSelected;
            MvcApplication.UserRequest.UserProfile.ApplyClientProperties();
            return this.SettingsApplied();
        }

        public ActionResult CultureSet(string id)
        {
            MvcApplication.UserRequest.UserProfile.Culture = GlobalizationHelper.CultureInfoGetOrDefault(id);
            MvcApplication.UserRequest.UserProfile.ApplyClientProperties();
            return this.SettingsApplied();
        }

    }
}