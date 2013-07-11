using System.Web.Security;
using $safeprojectname$.ProxyProviders;

namespace $safeprojectname$.Authentication
{
    public class ProviderProxyAuthentication : ProviderBaseChannel<IProviderAuthentication>, IProviderAuthentication
    {
        public bool LogIn(string userName, string password, string customCredential, bool isPersistent)
        {
            return this.proxy.LogIn(userName, password, customCredential, isPersistent);
        }
        public void LogOut()
        {
            this.proxy.LogOut();
        }
        public bool IsLoggedIn()
        {
            return this.proxy.IsLoggedIn();
        }
        public bool ValidateUser(string userName, string password, string customCredential)
        {
            return this.proxy.ValidateUser(userName, password, customCredential);
        }
        public FormsIdentity GetFormsIdentity()
        {
            return this.proxy.GetFormsIdentity();
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
