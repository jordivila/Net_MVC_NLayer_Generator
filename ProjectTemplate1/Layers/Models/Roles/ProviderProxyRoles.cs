using $safeprojectname$.ProxyProviders;

namespace $safeprojectname$.Roles
{
    public class ProviderProxyRole : ProviderBaseChannel<IProviderRoles>, IProviderRoles
    {
        public string[] GetRolesForCurrentUser()
        { 
            return proxy.GetRolesForCurrentUser();
        }
        public bool IsCurrentUserInRole(string roleName)
        {
            return proxy.IsCurrentUserInRole(roleName);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}