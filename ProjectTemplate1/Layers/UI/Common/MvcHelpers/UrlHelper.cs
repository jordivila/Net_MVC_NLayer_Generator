
using System.Web.Mvc;
using $customNamespace$.Models.Cryptography;
using $customNamespace$.UI.Web.Controllers;
using $customNamespace$.UI.Web.Models;

namespace $customNamespace$.UI.Web.Common.Mvc.Html
{
    public static class UrlHelperExtension
    {
        public static string CultureSet(this UrlHelper helper)
        {
            return helper.Action("Set", "Culture", new { });
        }

        public static string ThemeSet(this UrlHelper helper)
        {
            return helper.Action("Set", "Theme", new { });
        }

        public static string JavaScriptDispatcher(this UrlHelper helper, string controller, baseViewModelInfo baseModel)
        {
            string url = string.Format("{0}?{1}={2}&{3}={4}&{5}={6}",

                            helper.Action("Javascript", "ResourceDispatcher", new { Area = string.Empty }),

                            ResourceDispatcherController.ResourceDispatchParamControllerKey,
                            System.Web.HttpUtility.UrlEncode(Crypto.Encrypt(controller, ResourceDispatcherController.ResourceDispatchCryptoPasswordKey)),

                            ResourceDispatcherController.ResourceDispatchParamVersionKey,
                            $customNamespace$.UI.Web.MvcApplication.Version,

                            ResourceDispatcherController.ResourceDispatchParamCultureKey,
                            baseModel.LocalizationResources.Culture);

            return url;
        }

        public static string StylesheetDispatcher(this UrlHelper helper, string controller, baseViewModelInfo baseModel)
        {
            string url = string.Format("{0}?{1}={2}&{3}={4}&{5}={6}",

                            helper.Action("StyleSheet", "ResourceDispatcher", new { Area = string.Empty }),

                            ResourceDispatcherController.ResourceDispatchParamControllerKey,
                            System.Web.HttpUtility.UrlEncode(Crypto.Encrypt(controller, ResourceDispatcherController.ResourceDispatchCryptoPasswordKey)),

                            ResourceDispatcherController.ResourceDispatchParamVersionKey,
                            $customNamespace$.UI.Web.MvcApplication.Version,

                            ResourceDispatcherController.ResourceDispatchParamCultureKey,
                            baseModel.LocalizationResources.Culture);

            return url;
        }
    }
}