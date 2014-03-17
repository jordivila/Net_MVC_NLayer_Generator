using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Web;
using System.Web.Profile;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Globalization;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Resources.Helpers.GeneratedResxClasses;

namespace $customNamespace$.Models.Profile
{
    public interface IUserProfileModel
    {
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime? BirthDate { get; set; }
        Gender? Gender { get; set; }
        CultureInfo Culture { get; set; }
        string CultureName { get; set; }
        ThemesAvailable? Theme { get; set; }
    }

    [DataContract]
    public class UserProfileModel : IUserProfileModel, IDisposable
    {
        public UserProfileModel() { }

        public UserProfileModel(ProfileBase profileBase)
        {
            this.FirstName = (string)profileBase[baseModel.GetInfo(()=>this.FirstName).Name];
            this.LastName = (string)profileBase[baseModel.GetInfo(() => this.LastName).Name];
            this.BirthDate = (DateTime?)profileBase[baseModel.GetInfo(() => this.BirthDate).Name];
            
            //this.Gender = (Gender?)profileBase[baseModel.GetInfo(() => this.Gender).Name];
            this.Gender = Enum.IsDefined(typeof(Gender), string.Format("{0}",profileBase[baseModel.GetInfo(() => this.Gender).Name])) ?
                                        (Gender?)Enum.Parse(typeof(Gender), (string)profileBase[baseModel.GetInfo(() => this.Gender).Name])
                                        :
                                        null;

            //this.Culture = Enum.IsDefined(typeof(CulturesAvailable), string.Format("{0}",profileBase[baseModel.GetInfo(() => this.Culture).Name])) ?
            //                            (CulturesAvailable?)Enum.Parse(typeof(CulturesAvailable), (string)profileBase[baseModel.GetInfo(() => this.Culture).Name])
            //                            :
            //                            $customNamespace$.Models.Enumerations.CulturesAvailable.en_US;
            
            //this.Culture = GlobalizationHelper.CultureInfoGetOrDefault(string.Format("{0}", profileBase[baseModel.GetInfo(() => this.Culture).Name]));
            this.CultureName = (string)profileBase[baseModel.GetInfo(() => this.Culture).Name];
            

            this.Theme = Enum.IsDefined(typeof(ThemesAvailable), string.Format("{0}", profileBase[baseModel.GetInfo(() => this.Theme).Name])) ?
                                        (ThemesAvailable?)Enum.Parse(typeof(ThemesAvailable), (string)profileBase[baseModel.GetInfo(() => this.Theme).Name])
                                        :
                                        ThemesAvailable.Redmond;
        }

        public void SetProfileBasePropertyValues(ref ProfileBase profileBase)
        {
            profileBase.SetPropertyValue(baseModel.GetInfo(() => this.FirstName).Name, this.FirstName);
            profileBase.SetPropertyValue(baseModel.GetInfo(() => this.LastName).Name, this.LastName);
            profileBase.SetPropertyValue(baseModel.GetInfo(() => this.BirthDate).Name, this.BirthDate);
            profileBase.SetPropertyValue(baseModel.GetInfo(() => this.Gender).Name, this.Gender.ToString());
            profileBase.SetPropertyValue(baseModel.GetInfo(() => this.Culture).Name, this.Culture.ToString());
            profileBase.SetPropertyValue(baseModel.GetInfo(() => this.Theme).Name, this.Theme.ToString());
        }

        public void ApplyClientProperties()
        {
            #region Theme
            if (!System.Web.HttpContext.Current.Response.Cookies.AllKeys.Contains(UserRequestModel_Keys.WcfClientThemeSelectedCookieName))
            {
                System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientThemeSelectedCookieName, this.Theme.ToString()));
            }
            else
            {
                System.Web.HttpContext.Current.Response.Cookies[UserRequestModel_Keys.WcfClientThemeSelectedCookieName].Value = this.Theme.ToString();
            }
            #endregion

            #region Culture
            if (!System.Web.HttpContext.Current.Response.Cookies.AllKeys.Contains(UserRequestModel_Keys.WcfClientCultureSelectedCookieName))
            {
                System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, this.Culture.ToString()));
            }
            else
            {
                System.Web.HttpContext.Current.Response.Cookies[UserRequestModel_Keys.WcfClientCultureSelectedCookieName].Value = this.Culture.ToString();
            }

            Thread.CurrentThread.CurrentCulture = this.Culture;
            Thread.CurrentThread.CurrentUICulture = this.Culture;


            #endregion        
        }

        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [Display(ResourceType = typeof($customNamespace$.Resources.UserProfile.UserProfileTexts), Name = UserProfileTextsKeys.FirstName)]
        [DataType(DataType.Text)]
        [DataMember]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [Display(ResourceType = typeof($customNamespace$.Resources.UserProfile.UserProfileTexts), Name = UserProfileTextsKeys.LastName)]
        [DataType(DataType.Text)]
        [DataMember]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [Display(ResourceType = typeof($customNamespace$.Resources.UserProfile.UserProfileTexts), Name = UserProfileTextsKeys.BirtDate)]
        [DataType(DataType.Date)]
        [DataMember]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [Display(ResourceType = typeof($customNamespace$.Resources.UserProfile.UserProfileTexts), Name = UserProfileTextsKeys.Gender)]
        [DataMember]
        public Gender? Gender { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [Display(ResourceType = typeof($customNamespace$.Resources.UserProfile.UserProfileTexts), Name = UserProfileTextsKeys.Language)]
        public CultureInfo Culture
        {
            get
            {
                return GlobalizationHelper.CultureInfoGetOrDefault(this.CultureName);
            }
            set
            {
                this.CultureName = value.Name;
            }
        }

        /// <summary>
        /// This property is used to pass CultureInfo Name through WCF as far as System.Globalization.CultureInfo is NOT a serializable type 
        ///
        /// There was an error while trying to serialize parameter http://tempuri.org/:GetResult. 
        /// The InnerException message was 'Type 'System.Globalization.GregorianCalendar' with data contract 
        /// name 'GregorianCalendar:http://schemas.datacontract.org/2004/07/System.Globalization' is not expected. 
        /// Consider using a DataContractResolver or add any types not known statically to the list of known types - for example, 
        /// by using the KnownTypeAttribute attribute or by adding them to the list of known types passed to DataContractSerializer.'.  
        /// </summary>
        [DataMember]
        public string CultureName { get; set; }

        [Required(ErrorMessageResourceType = typeof($customNamespace$.Resources.General.GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [Display(ResourceType = typeof($customNamespace$.Resources.UserProfile.UserProfileTexts), Name = UserProfileTextsKeys.Theme)]
        [DataMember]
        public ThemesAvailable? Theme { get; set; }


        public void Dispose()
        {
            
        }
    }

    [DataContract]
    public class DataResultUserProfile : baseDataResult<UserProfileModel>, IDataResultModel<UserProfileModel>
    {
    }

}