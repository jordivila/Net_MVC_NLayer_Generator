using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Unity;

namespace $safeprojectname$
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    [LoggingServiceBehavior]
    public abstract class BaseService : IDisposable
    {
        public BaseService()
        {
            //if (System.ServiceModel.OperationContext.Current != null)
            //{
            Thread.CurrentThread.CurrentCulture = this.UserRequest.UserProfile.Culture;
            Thread.CurrentThread.CurrentUICulture = this.UserRequest.UserProfile.Culture;
            //}
        }

        private static IUserRequestModel<OperationContext, MessageHeaders> _userRequest = null;

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

        public virtual void Dispose()
        {

        }

    }
}