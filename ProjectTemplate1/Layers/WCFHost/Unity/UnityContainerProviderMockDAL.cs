using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.SmtpModels;
using $customNamespace$.DAL.MembershipServices;
using $customNamespace$.DAL.LoggingServices;
using $customNamespace$.DAL.SyndicationServices;
using $customNamespace$.DAL.TokenTemporaryPersistenceServices;
using $customNamespace$.Models.UserRequestModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using $customNamespace$.Models.Profile;



namespace $customNamespace$.WCF.Unity
{
    internal class UnityContainerProviderMockDAL : UnityContainerProvider
    {
        internal override IUnityContainer GetContainer()
        {
            IUnityContainer unityContainerReal = new UnityContainer();
            unityContainerReal.RegisterType(typeof(ISmtpClient), typeof(SmtpClientMock), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IMembershipDAL), typeof(MembershipDALMock), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IRoleAdminDAL), typeof(RoleAdminDALMock), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IProviderProfileDAL), typeof(ProfileDALMock), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ILoggingDAL), typeof(LoggingDALMock), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ISyndicationDAL), typeof(SyndicationDAL), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(ITokenTemporaryPersistenceDAL), typeof(TokenTemporaryPersistenceDALMock), new InjectionMember[0]);
            ///TODO: check binding configuration and use Http or NetTcp instance based on config values
            unityContainerReal.RegisterType(typeof(IUserRequestModel<OperationContext, MessageHeaders>), typeof(UserRequestModelNetTcp), new InjectionMember[0]);
            unityContainerReal.RegisterType(typeof(IUserRequestModel<OperationContext, MessageHeaders>), typeof(UserRequestModelHttp), new InjectionMember[0]);
            return unityContainerReal;
        }
    }
}