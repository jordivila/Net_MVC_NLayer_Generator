using $safeprojectname$.ProxyProviders;

namespace $safeprojectname$.Profile
{
    public class ProviderProxyProfileServices : ProviderBaseChannel<IProviderProfile>, IProviderProfile
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