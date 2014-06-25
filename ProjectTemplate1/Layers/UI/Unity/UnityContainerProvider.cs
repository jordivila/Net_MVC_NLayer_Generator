using Microsoft.Practices.Unity;
using $customNamespace$.Models.Authentication;
using $customNamespace$.Models.Logging;
using $customNamespace$.Models.Membership;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.ProxyProviders;
using $customNamespace$.Models.Roles;
using $customNamespace$.Models.Syndication;
using $customNamespace$.Models.Unity;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.UserSessionPersistence;
using System;
using System.Web;
using System.Web.SessionState;

namespace $customNamespace$.UI.Web.Unity
{
    public class UnityContainerProvider
    {
        public static IUnityContainer GetContainer(FrontEndUnityContainerAvailable containerSelected)
        {
            IUnityContainer result = new UnityContainer();
            result.RegisterType(typeof(IAuthenticationProxy), typeof(AuthenticationProxy), new InjectionMember[0]);
            result.RegisterType(typeof(IMembershipProxy), typeof(MembershipProxy), new InjectionMember[0]);
            result.RegisterType(typeof(IRoleManagerProxy), typeof(RoleManagerProxy), new InjectionMember[0]);
            result.RegisterType(typeof(IRolesProxy), typeof(RolesProxy), new InjectionMember[0]);
            result.RegisterType(typeof(IProfileProxy), typeof(ProfileProxy), new InjectionMember[0]);
            result.RegisterType(typeof(ILoggingProxy), typeof(LoggingProxy), new InjectionMember[0]);
            result.RegisterType(typeof(ISyndicationProxy), typeof(SyndicationProxy), new InjectionMember[0]);
            result.RegisterType(typeof(IUserRequestContextFrontEnd), typeof(UserRequestContextFrontEnd), new InjectionMember[0]);
            result.RegisterType(typeof(IUserSessionModel), typeof(UserSessionAtHttpCookies), new InjectionMember[0]);

            switch (containerSelected)
            {
                case FrontEndUnityContainerAvailable.ProxiesToCustomHost:
                    result.RegisterType(typeof(IClientChannelInitializer<>), typeof(ClientChannelCustomHostInitializer<>), new InjectionMember[0]);
                    break;
                case FrontEndUnityContainerAvailable.ProxiesToAzure:
                    result.RegisterType(typeof(IClientChannelInitializer<>), typeof(ClientChannelAzureInternalRoleInitializer<>), new InjectionMember[0]);
                    break;
                default:
                    throw new Exception("IUnityContainer does not exist in the list of available providers");
            }

            return result;
        }
    }
}