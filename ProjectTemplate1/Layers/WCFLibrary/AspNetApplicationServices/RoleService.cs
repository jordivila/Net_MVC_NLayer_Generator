using System.Linq;
using $customNamespace$.BL.MembershipServices;
using $customNamespace$.Models.Roles;

namespace $safeprojectname$.AspNetApplicationServices
{
    public class RoleService : BaseService, IProviderRoles
    {
        IRoleAdminBL _bl = null;
        public RoleService()
        {
            this._bl = new RoleAdminBL();
        }
        public override void Dispose()
        {
            if (this._bl != null)
            {
                this._bl.Dispose();
            }

            base.Dispose();
        }
        public string[] GetRolesForCurrentUser()
        {
            if (string.IsNullOrEmpty(this.UserRequest.WcfAuthenticationCookieValue))
            {
                return new string[0];
            }
            else
            {
                return this._bl.FindByUserName(this.UserRequest.UserFormsIdentity.Name).Data;
            }
        }
        public bool IsCurrentUserInRole(string roleName)
        {
            if (string.IsNullOrEmpty(this.UserRequest.WcfAuthenticationCookieValue))
            {
                return false;
            }
            else
            {
                return this._bl.FindByUserName(this.UserRequest.UserFormsIdentity.Name).Data.Contains(roleName);
            }
        }
    }
}
