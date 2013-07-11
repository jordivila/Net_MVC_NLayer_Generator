using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Security;
using Microsoft.Practices.Unity;
using $safeprojectname$.Authentication;
using $safeprojectname$.Enumerations;
using $safeprojectname$.Globalization;
using $safeprojectname$.Membership;
using $safeprojectname$.Profile;
using $safeprojectname$.Roles;
using $safeprojectname$.Unity;

namespace $safeprojectname$.UserRequestModel
{
    public static class UserRequestModel_Keys
    {
        public static string WcfCustomBehaviourName = "WcfCustomBehaviourName";

        public static string WcfClientCultureSelectedCookieName = ".WCFClientCultureSelected";
        public static string WcfClientThemeSelectedCookieName = ".WCFClientThemeSelected";
        public static string WcfFormsAuthenticationCookieName = ".WCFAUTH";
        public static string WcfSessionIdKey = ".WCFSessionId";

        public static string UserContextFormsIdentityKey = "UserContextFormsIdentityKey";
        public static string UserContextIsLoggedInKey = "UserContextIsLoggedInKey";
        public static string UserContextMembershipKey = "UserContextMembershipKey";
        public static string UserContextRolesKey = "UserContextRolesKey";
        public static string UserContextProfileKey = "UserContextProfileKey";
        public static string UserContextControllerTypeKey = "UserContextControllerTypeKey";
    }

    public interface IUserRequestClientModel : IDisposable
    {
        MembershipUserWrapper UserMembership_GetAndUpdateActivity { get; }
        string[] UserRoles { get; }
    }

    public interface IUserRequestModel<TContext, TObjectCollection> : IDisposable
    {
        TContext Context { get; }
        TObjectCollection ContextBag { get; }
        UserProfileModel UserProfile { get; set; }
        FormsIdentity UserFormsIdentity { get; }
        bool UserIsLoggedIn { get; }
        string WcfAuthenticationCookieValue { get; set; }
        string WcfSessionIdKeyValue { get; }
    }

    public static class UserRequestHelper<TContext, TObjectCollection>
    {
        private static IUserRequestModel<HttpContext, HttpCookieCollection> httpUserRequest = null;
        private static IUserRequestModel<OperationContext, MessageHeaders> ntpTcpUserRequest = null;

        public static object CreateUserRequest()
        {
            object result = null;

            if (typeof(TContext) == typeof(HttpContext))
            {
                if (UserRequestHelper<TContext, TObjectCollection>.httpUserRequest == null)
                {
                    using (DependencyFactory dependencyFactory = new DependencyFactory())
                    {
                        UserRequestHelper<TContext, TObjectCollection>.httpUserRequest = (IUserRequestModel<HttpContext, HttpCookieCollection>)dependencyFactory.Unity.Resolve<IUserRequestModel<HttpContext, HttpCookieCollection>>();
                    }
                }
                result = UserRequestHelper<TContext, TObjectCollection>.httpUserRequest;
            }

            if (typeof(TContext) == typeof(OperationContext))
            {
                if (UserRequestHelper<TContext, TObjectCollection>.ntpTcpUserRequest == null)
                {
                    using (DependencyFactory dependencyFactory = new DependencyFactory())
                    {
                        UserRequestHelper<TContext, TObjectCollection>.ntpTcpUserRequest = (IUserRequestModel<OperationContext, MessageHeaders>)dependencyFactory.Unity.Resolve<IUserRequestModel<OperationContext, MessageHeaders>>();
                    }
                }
                result = UserRequestHelper<TContext, TObjectCollection>.ntpTcpUserRequest;
            }

            if (result == null)
            {
                throw new NotImplementedException("UserRequest Not supported");
            }
            else
            {
                return result;
            }
        }
    }

    public class UserRequestModelHttpClient : UserRequestModelHttp, IUserRequestClientModel
    {
        private IProviderRoles RolesProvider;
        private IProviderMembership MembershipProvider;
        private IProviderAuthentication ProviderAuthentication;

        public UserRequestModelHttpClient()
        {
            using (DependencyFactory dependencyFactory = new DependencyFactory())
            {
                this.RolesProvider = dependencyFactory.Unity.Resolve<IProviderRoles>();
                this.MembershipProvider = dependencyFactory.Unity.Resolve<IProviderMembership>();
                this.ProviderAuthentication = dependencyFactory.Unity.Resolve<IProviderAuthentication>();
            }
        }
        public override FormsIdentity UserFormsIdentity
        {
            get
            {
                if (!this.Context.Items.Contains(UserRequestModel_Keys.UserContextFormsIdentityKey))
                {
                    this.Context.Items[UserRequestModel_Keys.UserContextFormsIdentityKey] = ProviderAuthentication.GetFormsIdentity();
                }
                return (FormsIdentity)this.Context.Items[UserRequestModel_Keys.UserContextFormsIdentityKey];
            }
        }
        public MembershipUserWrapper UserMembership_GetAndUpdateActivity
        {
            get
            {
                if (!this.Context.Items.Contains(UserRequestModel_Keys.UserContextMembershipKey))
                {
                    this.Context.Items[UserRequestModel_Keys.UserContextMembershipKey] = this.MembershipProvider.GetUserByName(this.UserFormsIdentity.Name, true).Data;
                }
                return (MembershipUserWrapper)this.Context.Items[UserRequestModel_Keys.UserContextMembershipKey];
            }
        }
        public override bool UserIsLoggedIn
        {
            get
            {
                if (!this.Context.Items.Contains(UserRequestModel_Keys.UserContextIsLoggedInKey))
                {
                    try
                    {
                        this.Context.Items[UserRequestModel_Keys.UserContextIsLoggedInKey] = this.ProviderAuthentication.IsLoggedIn();
                    }
                    catch (Exception)
                    {
                        // do NOT throw !!! prevents looping exception -> ERR_TOO_MANY_REDIRECTS
                        // throw;
                        this.Context.Items[UserRequestModel_Keys.UserContextIsLoggedInKey] = false;
                    }
                }
                return (bool)this.Context.Items[UserRequestModel_Keys.UserContextIsLoggedInKey];
            }
        }
        public string[] UserRoles
        {
            get
            {
                if (!this.Context.Items.Contains(UserRequestModel_Keys.UserContextRolesKey))
                {
                    this.Context.Items[UserRequestModel_Keys.UserContextRolesKey] = this.RolesProvider.GetRolesForCurrentUser();
                }
                return (string[])this.Context.Items[UserRequestModel_Keys.UserContextRolesKey];
            }

        }
        public override void Dispose()
        {
            if (RolesProvider != null)
            {
                RolesProvider.Dispose();
            }
            if (MembershipProvider != null)
            {
                MembershipProvider.Dispose();
            }
            if (ProviderAuthentication != null)
            {
                ProviderAuthentication.Dispose();
            }
        }
    }

    public class UserRequestModelHttp : IUserRequestModel<HttpContext, HttpCookieCollection>
    {
        public HttpContext Context
        {
            get
            {
                return HttpContext.Current;
            }
        }
        public HttpCookieCollection ContextBag
        {
            get
            {
                return this.Context.Request.Cookies;
            }
        }
        public virtual FormsIdentity UserFormsIdentity
        {
            get
            {
                if (!this.Context.Items.Contains(UserRequestModel_Keys.UserContextFormsIdentityKey))
                {
                    this.Context.Items[UserRequestModel_Keys.UserContextFormsIdentityKey] = ((FormsIdentity)this.Context.User.Identity);
                }
                return (FormsIdentity)this.Context.Items[UserRequestModel_Keys.UserContextFormsIdentityKey];
            }
        }
        public virtual bool UserIsLoggedIn
        {
            get
            {
                return this.Context.User.Identity.IsAuthenticated;
            }
        }
        public string WcfAuthenticationCookieValue
        {
            get
            {
                if (this.ContextBag.AllKeys.Contains(UserRequestModel_Keys.WcfFormsAuthenticationCookieName))
                {
                    return this.ContextBag[UserRequestModel_Keys.WcfFormsAuthenticationCookieName].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                // This is done in case WcfCookie Value is accessed right after authentication. When cookies yet have not been sent to client browser
                if (ContextBag.AllKeys.Contains(UserRequestModel_Keys.WcfFormsAuthenticationCookieName))
                {
                    this.ContextBag[UserRequestModel_Keys.WcfFormsAuthenticationCookieName].Value = value;
                }
                else
                {
                    Context.Response.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, value));
                }



                if (ContextBag.AllKeys.Contains(UserRequestModel_Keys.WcfFormsAuthenticationCookieName))
                {
                    Context.Response.Cookies[UserRequestModel_Keys.WcfFormsAuthenticationCookieName].Value = value;
                }
                else
                {
                    Context.Response.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, value));
                }
            }
        }
        public string WcfSessionIdKeyValue
        {
            get
            {
                if (this.ContextBag.AllKeys.Contains(UserRequestModel_Keys.WcfSessionIdKey))
                {
                    return this.ContextBag[UserRequestModel_Keys.WcfSessionIdKey].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        internal CultureInfo CultureSelected
        {
            get
            {
                if (!this.ContextBag.AllKeys.Contains(UserRequestModel_Keys.WcfClientCultureSelectedCookieName))
                {
                    List<string> cultureNamesAvailable = GlobalizationHelper.CultureInfoAvailableList().Select(p=>p.Name).ToList();
                    List<string> cultureNameBrowserDetected = this.Context.Request.UserLanguages.Select(p=>p.Split(';')[0]).ToList();
                    List<string> cultureNameSelected = cultureNamesAvailable.Intersect(cultureNameBrowserDetected).ToList();
                    CultureInfo culture = GlobalizationHelper.CultureInfoGetOrDefault(cultureNameSelected.Count() > 0 ? cultureNameSelected.First() : string.Empty);
                    this.Context.Response.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, culture.Name));
                    this.ContextBag.Add(new HttpCookie(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, culture.ToString()));
                    return culture;
                }
                else
                {
                    return GlobalizationHelper.CultureInfoGetOrDefault(this.ContextBag[UserRequestModel_Keys.WcfClientCultureSelectedCookieName].Value);
                }
            }
            set
            {
                if (!this.ContextBag.AllKeys.Contains(UserRequestModel_Keys.WcfClientCultureSelectedCookieName))
                {
                    this.Context.Response.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, value.Name));
                }
                else
                {
                    this.Context.Response.Cookies[UserRequestModel_Keys.WcfClientCultureSelectedCookieName].Value = value.Name;
                }
            }
        }
        internal ThemesAvailable ThemeSelected
        {
            get
            {
                if (!this.ContextBag.AllKeys.Contains(UserRequestModel_Keys.WcfClientThemeSelectedCookieName))
                {
                    ThemesAvailable themeSelected = ThemesAvailable.Redmond;
                    this.Context.Response.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientThemeSelectedCookieName, themeSelected.ToString()));
                    return themeSelected;
                }
                else
                {
                    ThemesAvailable themeSelected = (ThemesAvailable)Enum.Parse(typeof(ThemesAvailable), this.ContextBag[UserRequestModel_Keys.WcfClientThemeSelectedCookieName].Value.ToString().Replace("-","_"));
                    return themeSelected;
                }
            }
            set
            {
                if (!this.ContextBag.AllKeys.Contains(UserRequestModel_Keys.WcfClientThemeSelectedCookieName))
                {
                    this.Context.Response.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientThemeSelectedCookieName, value.ToString()));
                }
                else
                {
                    this.Context.Response.Cookies[UserRequestModel_Keys.WcfClientThemeSelectedCookieName].Value = value.ToString();
                }
            }
        }
        public UserProfileModel UserProfile
        {
            get 
            {
                if (!this.Context.Items.Contains(UserRequestModel_Keys.UserContextProfileKey))
                {
                    UserProfileModel userProfile = new UserProfileModel();
                    userProfile.Culture = this.CultureSelected;
                    userProfile.Theme = this.ThemeSelected;
                    this.Context.Items.Add(UserRequestModel_Keys.UserContextProfileKey, userProfile);
                }
                return (UserProfileModel)this.Context.Items[UserRequestModel_Keys.UserContextProfileKey];
            }
            set
            {
                this.Context.Items[UserRequestModel_Keys.UserContextProfileKey] = value;
            }
        }
        public virtual void Dispose()
        {

        }
    }

    public class UserRequestModelNetTcp : IUserRequestModel<OperationContext, MessageHeaders>
    {
        public OperationContext Context
        {
            get
            {
                return OperationContext.Current;
            }
        }
        public MessageHeaders ContextBag
        {
            get
            {
                return this.Context.RequestContext.RequestMessage.Headers;
                //return this.Context.IncomingMessageHeaders;
            }
            set
            {
                Context.OutgoingMessageHeaders.Add(MessageHeader.CreateHeader(value[0].Name, value[0].Namespace, value.GetHeader<string>(0)));
            }
        }
        public FormsIdentity UserFormsIdentity
        {
            get
            {
                FormsAuthenticationTicket fTicket = FormsAuthentication.Decrypt(this.WcfAuthenticationCookieValue);
                FormsIdentity fIdentity = new FormsIdentity(fTicket);
                return fIdentity;
            }
        }
        public bool UserIsLoggedIn
        {
            get
            {
                return this.UserFormsIdentity.IsAuthenticated;
            }
        }
        public string WcfAuthenticationCookieValue
        {
            get
            {
                return ContextBag.GetHeader<string>(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, UserRequestModel_Keys.WcfCustomBehaviourName);
            }
            set
            {
                OperationContext.Current.OutgoingMessageHeaders.Add(MessageHeader.CreateHeader(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, UserRequestModel_Keys.WcfCustomBehaviourName, value));
            }
        }
        public string WcfSessionIdKeyValue
        {
            get
            {
                return ContextBag.GetHeader<string>(UserRequestModel_Keys.WcfSessionIdKey, UserRequestModel_Keys.WcfCustomBehaviourName);
            }
        }
        internal CultureInfo CultureSelected
        {
            get
            {
                string cultureName = ContextBag.GetHeader<string>(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, UserRequestModel_Keys.WcfCustomBehaviourName);
                return GlobalizationHelper.CultureInfoGetOrDefault(cultureName);
            }
            set
            {
                // We should not set culture at service layer. This should be done at client side
            }
        }
        internal ThemesAvailable ThemeSelected
        {
            get
            {
                return ContextBag.GetHeader<ThemesAvailable>(UserRequestModel_Keys.WcfClientThemeSelectedCookieName, UserRequestModel_Keys.WcfCustomBehaviourName);
            }
            set
            {
                // We should not set culture at service layer. This should be done at client side
            }
        }
        internal CultureInfo CultureInfoSelected
        {
            get
            {
                return this.CultureSelected;
            }
        }
        public UserProfileModel UserProfile
        {
            get
            {
                UserProfileModel userProfile = new UserProfileModel();
                userProfile.Culture = this.CultureSelected;
                userProfile.Theme = this.ThemeSelected;
                return userProfile;
            }
            set
            {
                
            }
        }
        public void Dispose()
        {

        }
    }

    public class UserRequestModelHttpServer : IUserRequestModel<OperationContext, MessageHeaders>
    {
        public OperationContext Context
        {
            get
            {
                return OperationContext.Current;
            }
        }
        public MessageHeaders ContextBag
        {
            get
            {
                MessageHeaders m = new MessageHeaders(this.Context.IncomingMessageHeaders);

                string cookie = ((HttpRequestMessageProperty)this.Context.RequestContext.RequestMessage.Properties[HttpRequestMessageProperty.Name]).Headers[HttpRequestHeader.Cookie];
                List<KeyValuePair<string, string>> cookieValues = (from l in cookie.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                                                   select new KeyValuePair<string, string>(
                                                                       l.Split('=')[0], l.Split('=').Length == 2 ? l.Split('=')[1] : string.Empty
                                                                       )
                                                                   ).ToList();
                foreach (var item in cookieValues)
                {
                    m.Add(MessageHeader.CreateHeader(item.Key, UserRequestModel_Keys.WcfCustomBehaviourName, item.Value));
                }

                return m;
                
                //return this.Context.RequestContext.RequestMessage.Headers;
                
            }
            set
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add(HttpResponseHeader.SetCookie, string.Format("{0}={1}", value[0].Name, value.GetHeader<string>(0)));
                //Context.OutgoingMessageHeaders.Add(MessageHeader.CreateHeader(value[0].Name, value[0].Namespace, value.GetHeader<string>(0)));
            }
        }
        public FormsIdentity UserFormsIdentity
        {
            get
            {
                FormsAuthenticationTicket fTicket = FormsAuthentication.Decrypt(this.WcfAuthenticationCookieValue);
                FormsIdentity fIdentity = new FormsIdentity(fTicket);
                return fIdentity;
            }
        }
        public bool UserIsLoggedIn
        {
            get
            {
                return this.UserFormsIdentity.IsAuthenticated;
            }
        }
        public string WcfAuthenticationCookieValue
        {
            get
            {
                return ContextBag.GetHeader<string>(UserRequestModel_Keys.WcfFormsAuthenticationCookieName, UserRequestModel_Keys.WcfCustomBehaviourName);
            }
            set
            {
                string cookieValue = string.Format("{0}={1}", UserRequestModel_Keys.WcfFormsAuthenticationCookieName, value);
                if (string.IsNullOrEmpty(value))
                {
                    cookieValue = string.Format("{0};expires={1};", cookieValue, DateTime.Now.AddYears(-100));
                }
                WebOperationContext.Current.OutgoingResponse.Headers.Add(HttpResponseHeader.SetCookie, cookieValue);
            }
        }
        public string WcfSessionIdKeyValue
        {
            get
            {
                return ContextBag.GetHeader<string>(UserRequestModel_Keys.WcfSessionIdKey, UserRequestModel_Keys.WcfCustomBehaviourName);
            }
        }
        internal CultureInfo CultureSelected
        {
            get
            {

                string cultureName = ContextBag.GetHeader<string>(UserRequestModel_Keys.WcfClientCultureSelectedCookieName, UserRequestModel_Keys.WcfCustomBehaviourName);
                return GlobalizationHelper.CultureInfoGetOrDefault(cultureName);
            }
            set
            {
                // We should not set culture at service layer. This should be done at client side
            }
        }
        internal ThemesAvailable ThemeSelected
        {
            get
            {
                return ContextBag.GetHeader<ThemesAvailable>(UserRequestModel_Keys.WcfClientThemeSelectedCookieName, UserRequestModel_Keys.WcfCustomBehaviourName);
            }
            set
            {
                // We should not set culture at service layer. This should be done at client side
            }
        }
        internal CultureInfo CultureInfoSelected
        {
            get
            {
                return this.CultureSelected;
            }
        }
        public UserProfileModel UserProfile
        {
            get
            {
                UserProfileModel userProfile = new UserProfileModel();
                userProfile.Culture = this.CultureSelected;
                userProfile.Theme = this.ThemeSelected;
                return userProfile;
            }
            set
            {

            }
        }
        public void Dispose()
        {

        }
    }

}