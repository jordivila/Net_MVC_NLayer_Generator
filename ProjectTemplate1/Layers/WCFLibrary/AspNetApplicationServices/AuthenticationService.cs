using System.Web.Security;
using $customNamespace$.BL.AuthenticationServices;
using $customNamespace$.Models.Authentication;

namespace $safeprojectname$.AspNetApplicationServices
{
    public class AuthenticationService : BaseService, IAuthenticationProxy
    {
        IAuthenticationBL authBL = null;

        public AuthenticationService()
        {
            authBL = new AuthenticationBL();
        }
        public override void Dispose()
        {
            if (this.authBL != null)
            {
                this.authBL.Dispose();
            }

            base.Dispose();
        }
        public FormsIdentity GetFormsIdentity()
        {
            return this.authBL.GetFormsIdentity();
        }
        public bool IsLoggedIn()
        {
            return this.authBL.IsLoggedIn();
        }
        public bool LogIn(string userName, string password, string customCredential, bool isPersistent)
        {
            return this.authBL.LogIn(userName, password, customCredential, isPersistent);
        }
        public void LogOut()
        {
            this.authBL.LogOut();
        }
        public bool ValidateUser(string userName, string password, string customCredential)
        {
            return this.authBL.ValidateUser(userName, password, customCredential);
        }
    }
}
