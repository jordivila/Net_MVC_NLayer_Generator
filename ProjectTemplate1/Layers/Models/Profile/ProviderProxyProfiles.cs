using $safeprojectname$.ProxyProviders;
using System;
using System.ServiceModel;

namespace $safeprojectname$.Profile
{
    [ServiceContract]
    public interface IProviderProxyProfileServices : IDisposable
    {
        [OperationContract]
        DataResultUserProfile Get();

        [OperationContract]
        DataResultUserProfile Update(UserProfileModel userProfile);
    }


    public class ProviderProxyProfileServices : ProviderBaseChannel<IProviderProxyProfileServices>, IProviderProxyProfileServices
    {
        public DataResultUserProfile Get()
        {
            return proxy.Get();
        }
        public DataResultUserProfile Update(UserProfileModel userProfile)
        {
            return this.proxy.Update(userProfile);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}