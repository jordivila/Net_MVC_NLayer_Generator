using $customNamespace$.BL.MembershipServices;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Roles;

namespace $safeprojectname$.AspNetApplicationServices.Admin
{
    public class RoleServiceAdmin : BaseService, IRoleManagerProxy
    {
        private IRoleAdminBL _bl;
        public RoleServiceAdmin()
        {
            _bl = new RoleAdminBL();
        }
        public override void Dispose()
        {
            base.Dispose();

            if (this._bl != null)
            {
                this._bl.Dispose();
            }
        }
        public DataResultBoolean AddToRoles(string userName, string[] roles)
        {
            return this._bl.AddToRoles(userName, roles);
        }
        public DataResultBoolean Create(string roleName)
        {
            return this._bl.Create(roleName);
        }
        public DataResultBoolean Delete(string roleName)
        {
            return this._bl.Delete(roleName);
        }
        public DataResultStringArray FindAll()
        {
            return this._bl.FindAll();
        }
        public DataResultStringArray FindByUserName(string userName)
        {
            return this._bl.FindByUserName(userName);
        }
        public DataResultStringArray FindUserNamesByRole(string roleName)
        {
            return this._bl.FindUserNamesByRole(roleName);
        }
        public DataResultBoolean IsInRole(string user, string roleName)
        {
            return this._bl.IsInRole(user, roleName);
        }
        public DataResultBoolean RemoveFromRoles(string userName, string[] roles)
        {
            return this._bl.RemoveFromRoles(userName, roles);
        }
        public DataResultBoolean RoleExists(string roleName)
        {
            return _bl.RoleExists(roleName);
        }
    }
}
