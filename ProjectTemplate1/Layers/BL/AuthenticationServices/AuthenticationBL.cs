using System.Web.Security;
using $safeprojectname$.MembershipServices;
using $customNamespace$.Models.Authentication;
using $customNamespace$.Models.Common;

namespace $safeprojectname$.AuthenticationServices
{
    public interface IAuthenticationBL : IAuthenticationProxy
    {
        FormsAuthenticationTicket SetTicket(string userName);
    }

    //public interface CopyOfIAuthenticationBL : IAuthenticationProxy
    //{
    //    FormsAuthenticationTicket SetTicket(string userName);
    //}

    public class AuthenticationBL : BaseBL, IAuthenticationBL
    {
        IMembershipBL bl = null;

        public AuthenticationBL()
        {
            bl = new MembershipBL();
        }
        public override void Dispose()
        {
            if (this.bl != null)
            {
                this.bl.Dispose();
            }

            base.Dispose();
        }
        public FormsIdentity GetFormsIdentity()
        {
            if (!string.IsNullOrEmpty(this.UserRequest.WcfAuthenticationCookieValue))
            {
                return this.UserRequest.UserFormsIdentity;
            }
            else
            {
                return null;
            }
        }
        public bool IsLoggedIn()
        {
            bool result = false;

            if (!string.IsNullOrEmpty(this.UserRequest.WcfAuthenticationCookieValue))
            {
                try
                {
                    if (this.UserRequest.UserFormsIdentity.IsAuthenticated)
                    {
                        if (!this.UserRequest.UserFormsIdentity.Ticket.Expired)
                        {
                            this.SetTicket(FormsAuthentication.RenewTicketIfOld(this.UserRequest.UserFormsIdentity.Ticket));
                            result = true;
                        }
                    }
                    //return this.UserRequest.UserFormsIdentity.IsAuthenticated;
                }
                catch 
                {
                    //return false;
                }
            }
            else
            {
                //return false;
            }

            if (!result)
            {
                this.LogOut();
            }

            return result;
        }

        public FormsAuthenticationTicket SetTicket(string userName)
        {
            FormsAuthenticationTicket fTicket = new FormsAuthenticationTicket(userName, false, (int)FormsAuthentication.Timeout.TotalMinutes);
            return this.SetTicket(fTicket);
        }

        private FormsAuthenticationTicket SetTicket(FormsAuthenticationTicket fTicket)
        {
            this.UserRequest.WcfAuthenticationCookieValue = FormsAuthentication.Encrypt(fTicket);
            return fTicket;
        }

        public bool LogIn(string userName, string password, string customCredential, bool isPersistent)
        {
            DataResultBoolean result = bl.ValidateUser(userName, password);
            if (result.Data == true)
            {
                this.SetTicket(userName);
            }
            return result.Data;
        }
        public void LogOut()
        {
            this.UserRequest.WcfAuthenticationCookieValue = string.Empty;
        }
        public bool ValidateUser(string userName, string password, string customCredential)
        {
            IMembershipBL bl = new MembershipBL();
            DataResultBoolean result = bl.ValidateUser(userName, password);
            return result.Data;
        }
    }

    
}
