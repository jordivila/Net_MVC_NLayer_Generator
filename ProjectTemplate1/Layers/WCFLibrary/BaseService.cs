using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using $customNamespace$.Models.UserRequestModel;

namespace $safeprojectname$
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode=ConcurrencyMode.Single )]
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

        internal IUserRequestModel<OperationContext, MessageHeaders> UserRequest
        {
            get
            {
                return UserRequestHelper<OperationContext, MessageHeader>.CreateUserRequest() as IUserRequestModel<OperationContext, MessageHeaders>;
            }
        }

        public virtual void Dispose()
        {
            //if (this._userRequest != null)
            //{
                //this._userRequest.Dispose();
            //}
        }

    }
}