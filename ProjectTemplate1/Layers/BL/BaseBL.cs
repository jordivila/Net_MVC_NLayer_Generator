using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using $customNamespace$.Models.UserRequestModel;

namespace $safeprojectname$
{
    public abstract class BaseBL : IDisposable
    {
        public BaseBL()
        {

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
            
        }
    }
}


