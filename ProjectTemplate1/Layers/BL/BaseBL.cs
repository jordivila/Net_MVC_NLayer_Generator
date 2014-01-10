using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Unity;

namespace $safeprojectname$
{
    public abstract class BaseBL : IDisposable
    {
        public BaseBL()
        {

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


