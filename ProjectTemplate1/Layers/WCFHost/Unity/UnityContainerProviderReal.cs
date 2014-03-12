using Microsoft.Practices.Unity;
using $customNamespace$.DAL.LoggingServices;
using $customNamespace$.DAL.MembershipRoleServices;
using $customNamespace$.DAL.MembershipServices;
using $customNamespace$.DAL.SyndicationServices;
using $customNamespace$.DAL.TokenTemporaryPersistenceServices;
using $customNamespace$.Models.Logging;
using $customNamespace$.Models.Membership;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.Roles;
using $customNamespace$.Models.SmtpModels;
using $customNamespace$.Models.Syndication;
using $customNamespace$.Models.TokenPersistence;
using $customNamespace$.Models.UserRequestModel;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace $customNamespace$.WCF.Unity
{
    internal class UnityContainerProviderReal : UnityContainerProvider
    {
        internal override IUnityContainer GetContainer()
        {
            IUnityContainer unityContainerReal = new UnityContainer();
            unityContainerReal.RegisterType(typeof(ISmtpClient), typeof(SmtpClientMock), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IMembershipDAL), typeof(MembershipDAL), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IRoleAdminDAL), typeof(RoleAdminDAL), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProfileDAL), typeof(ProfileDAL), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ILoggingDAL), typeof(LoggingDAL), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ISyndicationDAL), typeof(SyndicationDAL), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ITokenTemporaryPersistenceDAL), typeof(TokenTemporaryPersistenceDAL), new InjectionMember[0]);

            //unityContainerReal.RegisterType(typeof(IUserRequestModel<OperationContext, MessageHeaders>), typeof(UserRequestModelNetTcp), new InjectionMember[0]);
            //unityContainerReal.RegisterType(typeof(IUserRequestModel<OperationContext, MessageHeaders>), typeof(UserRequestModelHttpServer), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IUserRequestModel<OperationContext, MessageHeaders>), typeof($customBindingUserRequestModelAtServer$), new InjectionMember[0]);
            return unityContainerReal;
        }
    }
}