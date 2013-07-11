
namespace $safeprojectname$.Models
{
    //public class LocalizationResourcesHelper : baseModel, IDisposable
    //{
    //    public CultureInfo CultureInfo { get; set; }
    //    public string Culture { get; set; }
    //    public string CultureGlobalization { get; set; }
    //    public string CultureDatePicker { get; set; }
    //    public string CultureValidate { get; set; }

    //    public LocalizationResourcesHelper()
    //    {
    //        this.Init(MvcApplication.UserRequest.UserProfile.Culture);
    //    }

    //    public LocalizationResourcesHelper(CultureInfo culture)
    //    {
    //        this.Init(culture);
    //    }

    //    private void Init(CultureInfo culture)
    //    {
    //        this.CultureInfo = culture;
    //        this.Culture = this.CultureInfo.Name;
    //        this.CultureGlobalization = GlobalizationHelper.EnglishUS;
    //        this.CultureDatePicker = string.Empty;
    //        this.CultureValidate = string.Empty;

    //        if (this.Culture == GlobalizationHelper.SpanishInternacional)
    //        {
    //            CultureGlobalization = this.CultureInfo.Name;
    //            CultureDatePicker = "es";
    //            CultureValidate = "es";
    //        }

    //        if (this.Culture == GlobalizationHelper.EnglishGB)
    //        {
    //            CultureGlobalization = this.CultureInfo.Name;
    //            CultureDatePicker = this.CultureInfo.Name;
    //            CultureValidate = string.Empty;
    //        }
    //    }

    //    public string jQueryUILocalizationPath
    //    {
    //        get
    //        {
    //            if (string.IsNullOrEmpty(this.CultureDatePicker))
    //            {
    //                return string.Empty;
    //            }
    //            else
    //            {
    //                return string.Format(ApplicationConfiguration.ClientResourcesSettingsSection.JQueryLibUILocalization, string.Format("-{0}", this.CultureDatePicker));
    //            }
    //        }
    //    }

    //    public string jQueryValidationLocalizationPath
    //    {
    //        get
    //        {
    //            if (string.IsNullOrEmpty(this.CultureValidate))
    //            {
    //                return string.Empty;
    //            }
    //            else
    //            {
    //                return string.Format(ApplicationConfiguration.ClientResourcesSettingsSection.JQueryLibValidateLocalization, string.Format("_{0}", this.CultureValidate));
    //            }
    //        }
    //    }

    //    public string jQueryGlobalizeLozalizationPath
    //    {
    //        get
    //        {
    //            return string.Format(ApplicationConfiguration.ClientResourcesSettingsSection.JQueryLibGlobalizeLocalization, this.CultureGlobalization);
    //        }
    //    }

    //    public void Dispose()
    //    {
            
    //    }
    //}
}