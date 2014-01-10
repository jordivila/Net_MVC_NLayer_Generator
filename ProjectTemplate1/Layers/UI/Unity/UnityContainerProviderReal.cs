using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Profile;
using $safeprojectname$.Unity;
using $customNamespace$.Models.Authentication;
using $customNamespace$.Models.Membership;
using $customNamespace$.Models.Roles;
using $customNamespace$.Models.Logging;
using $customNamespace$.Models.Syndication;
using System.Web;
using $customNamespace$.Models.UserSessionPersistence;
using System.Web.SessionState;


namespace $safeprojectname$.Unity
{
    internal class UnityContainerProviderReal: UnityContainerProvider
    {
        internal override IUnityContainer GetContainer()
        {
            IUnityContainer unityContainerReal = new UnityContainer();

            unityContainerReal.RegisterType(typeof(IProviderAuthentication), typeof(ProviderProxyAuthentication), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProviderMembership), typeof(ProviderProxyMembership), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProviderRoleManager), typeof(ProviderProxyRoleManager), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProviderRoles), typeof(ProviderProxyRole), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProviderProxyProfileServices), typeof(ProviderProxyProfileServices), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProviderLogging), typeof(ProviderProxyLogging), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProviderSyndication), typeof(ProviderProxySyndication), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IUserRequestModel<HttpContext, HttpCookieCollection>), typeof(UserRequestModelHttpClient), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IUserSessionModel<HttpContext, HttpSessionState>), typeof(UserSessionHttp), new InjectionMember[0]);
            return unityContainerReal;
        }
    }
}
