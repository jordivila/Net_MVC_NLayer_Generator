using System;
using System.Collections.Generic;
using System.Globalization;
using $customNamespace$.Models.Globalization;

namespace $customNamespace$.Models.Configuration.ConfigSections.ClientResources
{
    public interface IClientResourcesConfiguration
    {
        List<string> JQueryLibScripts { get; set; }
        string jQueryValidationLocalizationPath(LocalizationResourcesHelper localizationHelper);
        string jQueryGlobalizeLozalizationPath(LocalizationResourcesHelper localizationHelper);
        string jQueryUILocalizationPath(LocalizationResourcesHelper localizationHelper);
        List<string> WebSiteCommonScripts { get; set; }
        List<string> WebSitePageInitScripts { get; set; }
        List<string> WebSiteCommonStyleSheets { get; set; }
        string WebSiteCommonAjax { get; set; }


        string CDN_RootFolderName { get; set; }

        string CDN_JS_RootFolderName { get; set; }
        string CDN_JS_VirtualRoot { get; set; }
        string CDN_JS_CommonFileName(CultureInfo culture, string Version);

        string CDN_CSS_RootFolderName { get; set; }
        string CDN_CSS_VirtualRoot { get; set; }
        string CDN_CSS_CommonFileName(string Version);
    }

    public class ClientResourcesConfiguration : IClientResourcesConfiguration
    {
        public List<string> JQueryLibScripts
        {
            get
            {
                return new List<string>() { 
                                            "~/Scripts/jQuery/jquery-1.9.1.min.js",
                                            "~/Scripts/jquery-ui-1.10.0/ui/minified/jquery-ui.min.js",
                                            "~/Scripts/jquery-validation-1.11.0/dist/jquery.validate.min.js",
                                            "~/Scripts/jQuery-globalize/lib/globalize.js"
                };


                //return new List<string>() { 
                //                            "~/Scripts/jQuery/jquery-1.9.1.min.js",
                //                            "~/Scripts/jquery-ui-1.10.0/ui/minified/jquery-ui.min.js",
                //                            "~/Scripts/jquery-validation-1.11.0/dist/jquery.validate.min.js",
                //                            "~/Scripts/jQuery-globalize/lib/globalize.js"
                //};
            }
            set
            {

            }
        }
        public string jQueryUILocalizationPath(LocalizationResourcesHelper localizationHelper)
        {
            if (string.IsNullOrEmpty(localizationHelper.CultureDatePicker))
            {
                return string.Empty;
            }
            else
            {
                return string.Format("~/Scripts/jquery-ui-1.10.0/ui/i18n/jquery.ui.datepicker{0}.js", string.Format("-{0}", localizationHelper.CultureDatePicker));
            }
        }
        public string jQueryValidationLocalizationPath(LocalizationResourcesHelper localizationHelper)
        {
            if (string.IsNullOrEmpty(localizationHelper.CultureValidate))
            {
                return string.Empty;
            }
            else
            {
                return string.Format("~/Scripts/jquery-validation-1.11.0/localization/messages{0}.js", string.Format("_{0}", localizationHelper.CultureValidate));
            }
        }
        public string jQueryGlobalizeLozalizationPath(LocalizationResourcesHelper localizationHelper)
        {
            return string.Format("~/Scripts/jQuery-globalize/lib/cultures/globalize.culture.{0}.js", localizationHelper.CultureGlobalization);
        }

        public List<string> WebSiteCommonScripts
        {
            get
            {
                return new List<string>() { "~/Scripts/Template.Init.js",
                                            string.Format("~/Scripts/{0}",WebSiteCommonAjax),
                                            "~/Scripts/Template.Init.Widget.js",
                                            "~/Scripts/Template.Widget.Base.js",
                                            "~/Scripts/Template.Widget.jQueryzer.js",
                                            "~/Scripts/Template.Widget.Form.js",
                                            "~/Scripts/Template.Widget.Boolean.js",
                                            "~/Scripts/Template.Widget.Grid.js",
                                            "~/Scripts/Template.Widget.AjaxProgress.js",
                                            "~/Scripts/Template.Widget.ButtonWrapper.js",
                                            "~/Scripts/Template.Widget.UserOptions.js",
                                            "~/Scripts/ui-dateSelector/ui-dateSelector.js",
                                            "~/Scripts/ui-widgetMsg/ui-widgetMsg.js",
                                            "~/Scripts/Template.Widget.Dialogs.js",
                                            "~/Scripts/Template.Widget.DialogInline.js",
                                            "~/Scripts/ui-widgetTreeList/ui-widgetTreeList.js"
                };
            }
            set
            {

            }
        }
        public string WebSiteCommonAjax
        {
            get
            {
                return "Template.Init.Ajax.js";
            }
            set { }
        }
        public List<string> WebSitePageInitScripts
        {
            get
            {
                return new List<string>() { "~/Scripts/Template.Widget.Page.js" };
            }
            set
            {

            }
        }
        public List<string> WebSiteCommonStyleSheets
        {
            get
            {
                return new List<string>() { "~/Content/reset.css", 
                                            "~/Content/Site.css", 
                                            "~/Content/font-awesome.css",
                                            "~/Content/Site.JqueryUI.IconsExtendWithFontAwsome.css",
                                            "~/Scripts/ui-widgetMsg/ui-widgetMsg.css" ,
                                            "~/Scripts/ui-dateSelector/ui-dateSelector.css" ,
                                            "~/Scripts/ui-widgetTreeList/ui-widgetTreeList.css"
                                        };
            }
            set
            {

            }
        }


        public string CDN_RootFolderName
        {
            get
            {
                return "CDN";
            }
            set { }
        }

        public string CDN_JS_RootFolderName
        {
            get
            {
                return "Scripts";
            }
            set { }
        }
        public string CDN_JS_VirtualRoot
        {
            get
            {
                return string.Format("~/{0}/{1}/", this.CDN_RootFolderName, this.CDN_JS_RootFolderName);
            }
            set
            {

            }
        }
        public string CDN_JS_CommonFileName(CultureInfo culture, string Version)
        {
            return string.Format("Template.CDN.Common.{0}.js", culture.Name);
        }

        public string CDN_CSS_RootFolderName
        {
            get
            {
                return "Css";
            }
            set { }
        }
        public string CDN_CSS_VirtualRoot
        {
            get
            {
                return string.Format("~/{0}/{1}/", this.CDN_RootFolderName, this.CDN_CSS_RootFolderName);
            }
            set { }
        }
        public string CDN_CSS_CommonFileName(string Version)
        {
            return "Template.CDN.Common.css";
            //return string.Format("Template.CDN.Common.{0}.css", Version);
        }
    }

    public class LocalizationResourcesHelper : baseModel, IDisposable
    {
        public CultureInfo CultureInfo { get; set; }
        public string Culture { get; set; }
        public string CultureGlobalization { get; set; }
        public string CultureDatePicker { get; set; }
        public string CultureValidate { get; set; }

        public LocalizationResourcesHelper(CultureInfo culture)
        {
            this.Init(culture);
        }

        private void Init(CultureInfo culture)
        {
            this.CultureInfo = culture;
            this.Culture = this.CultureInfo.Name;
            this.CultureGlobalization = GlobalizationHelper.EnglishUS;
            this.CultureDatePicker = string.Empty;
            this.CultureValidate = string.Empty;

            if (this.Culture == GlobalizationHelper.SpanishInternacional)
            {
                CultureGlobalization = this.CultureInfo.Name;
                CultureDatePicker = "es";
                CultureValidate = "es";
            }

            if (this.Culture == GlobalizationHelper.EnglishGB)
            {
                CultureGlobalization = this.CultureInfo.Name;
                CultureDatePicker = this.CultureInfo.Name;
                CultureValidate = string.Empty;
            }
        }

        public void Dispose()
        {

        }
    }
}