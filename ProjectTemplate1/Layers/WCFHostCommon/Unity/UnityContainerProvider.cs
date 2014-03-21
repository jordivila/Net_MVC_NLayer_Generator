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
using $customNamespace$.Models.Unity;
using $customNamespace$.Models.UserRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace $customNamespace$.WCF.ServicesHostCommon.Unity
{
    public class UnityContainerProvider
    {
        public static IUnityContainer GetContainer(BackEndUnityContainerAvailable containerSelected)
        {
            IUnityContainer result = new UnityContainer();

            switch (containerSelected)
            {
                case BackEndUnityContainerAvailable.Real:
                    result.RegisterType(typeof(ISmtpClient), typeof(SmtpClientMock), new InjectionMember[0]);
                    result.RegisterType(typeof(IMembershipDAL), typeof(MembershipDAL), new InjectionMember[0]);
                    result.RegisterType(typeof(IRoleAdminDAL), typeof(RoleAdminDAL), new InjectionMember[0]);
                    result.RegisterType(typeof(IProfileDAL), typeof(ProfileDAL), new InjectionMember[0]);
                    result.RegisterType(typeof(ILoggingDAL), typeof(LoggingDAL), new InjectionMember[0]);
                    result.RegisterType(typeof(ISyndicationDAL), typeof(SyndicationDAL), new InjectionMember[0]);
                    result.RegisterType(typeof(ITokenTemporaryPersistenceDAL<>), typeof(TokenTemporaryDatabasePersistenceDAL<>), new InjectionMember[0]);
                    //result.RegisterType(typeof(IUserRequestModel<OperationContext, MessageHeaders>), typeof(UserRequestModelNetTcp), new InjectionMember[0]);
                    //result.RegisterType(typeof(IUserRequestModel<OperationContext, MessageHeaders>), typeof(UserRequestModelHttpServer), new InjectionMember[0]);
                    result.RegisterType(typeof(IUserRequestModel<OperationContext, MessageHeaders>), typeof(UserRequestModelNetTcp), new InjectionMember[0]);
                    break;
                default:
                    throw new Exception("IUnityContainer does not exist in the list of available providers");
            }

            return result;
        }
    }
}
