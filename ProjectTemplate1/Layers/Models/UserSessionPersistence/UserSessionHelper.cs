using System;
using System.Web;
using System.Web.SessionState;
using System.Xml.Serialization;
using Microsoft.Practices.Unity;
using $safeprojectname$.Membership;
using $safeprojectname$.Unity;


namespace $safeprojectname$.UserSessionPersistence
{
    public interface IUserSessionModel<TContext, TObjectCollection> : IDisposable
    {
        TContext Context { get; }
        TObjectCollection ContextBag { get; }

        DataFilterUserList UserAdministrationController_LastSearch { get; set; }
    }

    public static class UserSessionHelper<TContext, TObjectCollection>
    {
        private static IUserSessionModel<HttpContext, HttpSessionState> httpUserSessionPersistence = null;
        //private static IUserSessionModel<OperationContext, MessageHeaders> ntpTcpUserSessionPersistence = null;

        public static object CreateUserSession()
        {
            object result = null;

            if (typeof(TContext) == typeof(HttpContext))
            {
                if (UserSessionHelper<TContext, TObjectCollection>.httpUserSessionPersistence == null)
                {
                    using (DependencyFactory dependencyFactory = new DependencyFactory())
                    {
                        UserSessionHelper<TContext, TObjectCollection>.httpUserSessionPersistence = (IUserSessionModel<HttpContext, HttpSessionState>)dependencyFactory.Unity.Resolve<IUserSessionModel<HttpContext, HttpSessionState>>();
                    }
                }
                result = UserSessionHelper<TContext, TObjectCollection>.httpUserSessionPersistence;
            }

            //if (typeof(TContext) == typeof(OperationContext))
            //{
            //    if (UserSessionHelper<TContext, TObjectCollection>.ntpTcpUserSessionPersistence == null)
            //    {
            //        UserSessionHelper<TContext, TObjectCollection>.ntpTcpUserSessionPersistence = (IUserSessionModel<OperationContext, MessageHeaders>)DependencyFactory.Unity.Resolve<IUserSessionModel<OperationContext, MessageHeaders>>();
            //    }
            //    result = UserSessionHelper<TContext, TObjectCollection>.ntpTcpUserSessionPersistence;
            //}

            if (result == null)
            {
                throw new NotImplementedException("IUserSessionModel Not supported");
            }
            else
            {
                return result;
            }
        }
    }

    [Serializable]
    public class UserSessionHttp : IUserSessionModel<HttpContext, HttpSessionState>
    {
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
                string jsonSerializedFilter = (string)ContextBag["UserAdministrationController_LastSearch"];
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
                ContextBag["UserAdministrationController_LastSearch"] = ((DataFilterUserList)value).SerializeToJson();
            }
        }

        public virtual void Dispose()
        {

        }
    }
}
