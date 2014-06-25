using System;
using System.Web;
using System.Web.SessionState;
using System.Xml.Serialization;
using System.Linq;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Membership;
using $customNamespace$.Models.Unity;
using $customNamespace$.Models.Cryptography;
using $customNamespace$.Models.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;


namespace $customNamespace$.Models.UserSessionPersistence
{
    public interface IUserSessionModel : IDisposable
    {
        DataFilterUserList UserAdministrationController_LastSearch { get; set; }
    }

    public interface IUserSessionModel<TContext, TObjectCollection> : IUserSessionModel
    {
        TContext Context { get; }
        TObjectCollection ContextBag { get; }
    }

    public abstract class UserSessionBase
    {
        protected const string _CryptoPassword = "UserSessionCryptoPassword";
        protected const string _UserAdministrationController_LastSearch = "lsUAC";

        public UserSessionBase() { }
    }

    [Serializable]
    public class UserSessionAtHttpSessionState : UserSessionBase, IUserSessionModel<HttpContext, HttpSessionState>
    {
        public UserSessionAtHttpSessionState() { }

        [XmlIgnore]
        public HttpContext Context
        {
            get
            {
                return HttpContext.Current;
            }
        }

        [XmlIgnore]
        public HttpSessionState ContextBag
        {
            get
            {
                return this.Context.Session;
            }
        }

        [XmlElement]
        public DataFilterUserList UserAdministrationController_LastSearch
        {
            get
            {
                string jsonSerializedFilter = (string)ContextBag[UserSessionBase._UserAdministrationController_LastSearch];

                if (string.IsNullOrEmpty(jsonSerializedFilter))
                {
                    return null;
                }
                else
                {
                    return baseModel.DeserializeFromJson<DataFilterUserList>(jsonSerializedFilter);
                }
            }
            set
            {
                ContextBag[UserSessionBase._UserAdministrationController_LastSearch] = ((DataFilterUserList)value).SerializeToJson();
            }
        }

        public virtual void Dispose()
        {

        }
    }

    [Serializable]
    public class UserSessionAtHttpCookies : UserSessionBase, IUserSessionModel<HttpContext, HttpCookieCollection>
    {
        public UserSessionAtHttpCookies() { }

        [XmlIgnore]
        public HttpContext Context
        {
            get
            {
                return HttpContext.Current;
            }
        }

        [XmlIgnore]
        public HttpCookieCollection ContextBag
        {
            get
            {
                return this.Context.Request.Cookies;
            }
        }

        [XmlElement]
        public DataFilterUserList UserAdministrationController_LastSearch
        {
            get
            {
                HttpCookie c = ContextBag[UserSessionBase._UserAdministrationController_LastSearch];

                if (c == null)
                {
                    // do nothing
                }

                DataFilterUserList result = null;

                if (c != null)
                {
                    try
                    {
                        result = baseModel.DeserializeFromJson<DataFilterUserList>(Crypto.Decrypt(c.Value, UserSessionBase._CryptoPassword));
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.Write(ex);
                    }
                    
                }

                return result;
            }
            set
            {
                string valueSerialized = Crypto.Encrypt(((DataFilterUserList)value).SerializeToJson(), UserSessionBase._CryptoPassword);

                if (this.Context.Response.Cookies.AllKeys.Contains(UserSessionBase._UserAdministrationController_LastSearch))
                {
                    this.Context.Response.Cookies[UserSessionBase._UserAdministrationController_LastSearch].Value = valueSerialized;
                }
                else
                {
                    this.Context.Response.Cookies.Add(new HttpCookie(UserSessionBase._UserAdministrationController_LastSearch, valueSerialized));
                }
            }
        }

        public virtual void Dispose()
        {

        }
    }

}
