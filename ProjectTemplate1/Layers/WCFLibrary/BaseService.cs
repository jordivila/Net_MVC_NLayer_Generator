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
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;


namespace $customNamespace$.WCF.ServicesLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    [LoggingServiceBehavior]
    public abstract class BaseService : IDisposable
    {
        public BaseService()
        {

        }

        public virtual void Dispose()
        {

        }
    }

    public abstract class BaseServiceWithCustomMessageHeaders : BaseService
    {
        public BaseServiceWithCustomMessageHeaders()
            : base()
        {
            Thread.CurrentThread.CurrentCulture = this.UserRequest.UserProfile.Culture;
            Thread.CurrentThread.CurrentUICulture = this.UserRequest.UserProfile.Culture;
        }

        private IUserRequestModel _userRequest = null;

        internal IUserRequestModel UserRequest
        {
            get
            {
                if (_userRequest == null)
                {
                    _userRequest = DependencyFactory.Resolve<IUserRequestModel>();
                }

                return _userRequest;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}