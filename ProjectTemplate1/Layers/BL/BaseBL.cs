using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.Models.Unity;
using System.Transactions;

namespace $customNamespace$.BL
{
    public abstract class BaseBL : IDisposable
    {
        public BaseBL()
        {

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

        internal TransactionScope TransactionScopeCreate()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                                            new TransactionOptions()
                                            {
                                                IsolationLevel = IsolationLevel.ReadCommitted
                                            });
        }

        public virtual void Dispose()
        {

        }
    }
}