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

            unityContainerReal.RegisterType(typeof(IAuthenticationProxy), typeof(AuthenticationProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IMembershipProxy), typeof(MembershipProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IRoleManagerProxy), typeof(RoleManagerProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IRolesProxy), typeof(RolesProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProfileProxy), typeof(ProfileProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ILoggingProxy), typeof(LoggingProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ISyndicationProxy), typeof(SyndicationProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IUserRequestModel<HttpContext, HttpCookieCollection>), typeof(UserRequestModelHttpClient), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IUserSessionModel<HttpContext, HttpSessionState>), typeof(UserSessionHttp), new InjectionMember[0]);
            return unityContainerReal;
        }
    }

    internal class UnityContainerProviderDevelopment : UnityContainerProvider
    {
        internal override IUnityContainer GetContainer()
        {
            IUnityContainer unityContainerReal = new UnityContainer();

            unityContainerReal.RegisterType(typeof(IAuthenticationProxy), typeof(AuthenticationProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IMembershipProxy), typeof(MembershipProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IRoleManagerProxy), typeof(RoleManagerProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IRolesProxy), typeof(RolesProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProfileProxy), typeof(ProfileProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ILoggingProxy), typeof(LoggingProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ISyndicationProxy), typeof(SyndicationProxy), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IUserRequestModel<HttpContext, HttpCookieCollection>), typeof(UserRequestModelHttpClient), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IUserSessionModel<HttpContext, HttpSessionState>), typeof(UserSessionHttp), new InjectionMember[0]);
            return unityContainerReal;
        }
    }
}
