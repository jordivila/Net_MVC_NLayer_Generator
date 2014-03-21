using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Unity;
using System.Collections.Generic;
using $customNamespace$.WCF.ServicesLibrary.AspNetApplicationServices;
using $customNamespace$.WCF.ServicesLibrary.AspNetApplicationServices.Admin;
using $customNamespace$.WCF.ServicesLibrary.LoggingServices;
using $customNamespace$.WCF.ServicesLibrary.SyndicationServices;


namespace $customNamespace$.WCF.ServicesLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    [LoggingServiceBehavior]
    public abstract class BaseService : IDisposable
    {
        public BaseService()
        {
            Thread.CurrentThread.CurrentCulture = this.UserRequest.UserProfile.Culture;
            Thread.CurrentThread.CurrentUICulture = this.UserRequest.UserProfile.Culture;
        }

        private IUserRequestModel<OperationContext, MessageHeaders> _userRequest = null;

        internal IUserRequestModel<OperationContext, MessageHeaders> UserRequest
        {
            get
            {
                if (_userRequest == null)
                {
                    _userRequest = DependencyFactory.Resolve<IUserRequestModel<OperationContext, MessageHeaders>>();
                }

                return _userRequest;
            }
        }

        public static List<Type> GetAllServiceTypes()
        {
            return new List<Type>() { 
                typeof(AuthenticationService),
                typeof(MembershipServices),
                typeof(RoleServiceAdmin),
                typeof(ProfileService),
                typeof(RoleService),
                typeof(LoggingService),
                typeof(SyndicationService)
            };
        }

        public virtual void Dispose()
        {

        }

    }
}