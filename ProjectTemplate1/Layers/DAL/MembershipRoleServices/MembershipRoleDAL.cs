using System.Web.Security;
using $customNamespace$.Models.Common;

namespace $safeprojectname$.MembershipServices
{
    public class RoleAdminDAL : BaseDAL, IRoleAdminDAL
    {
        public RoleAdminDAL()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public virtual DataResultBoolean AddToRoles(string userName, string[] roles)
        {
            Roles.AddUserToRoles(userName, roles);
            return new DataResultBoolean() { Data = true };
        }

        public virtual DataResultBoolean Create(string roleName)
        {
            Roles.CreateRole(roleName);
            return new DataResultBoolean() { Data = true };
        }

        public virtual DataResultBoolean Delete(string roleName)
        {
            return new DataResultBoolean() { Data = Roles.DeleteRole(roleName, true) }; //throws exception if role has members
        }

        public virtual DataResultStringArray FindAll()
        {
            return new DataResultStringArray() { Data = Roles.GetAllRoles() };
        }

        public virtual DataResultStringArray FindByUserName(string userName)
        {
            return new DataResultStringArray() { Data = Roles.GetRolesForUser(userName) };
        }

        public virtual DataResultStringArray FindUserNamesByRole(string roleName)
        {
            return new DataResultStringArray() { Data = Roles.GetUsersInRole(roleName) };
        }

        public virtual DataResultBoolean IsInRole(string user, string roleName)
        {
            return new DataResultBoolean() { Data = Roles.IsUserInRole(user, roleName) };
        }

        public virtual DataResultBoolean RemoveFromRoles(string user, string[] roles)
        {
            Roles.RemoveUserFromRoles(user, roles);
            return new DataResultBoolean() { Data = true };
        }

        public virtual DataResultBoolean RemoveUsersFromRole(string[] users, string[] roles)
        {
            Roles.RemoveUsersFromRoles(users, roles);
            return new DataResultBoolean() { Data = true };
        }

        public virtual DataResultBoolean RoleExists(string roleName)
        {
            return new DataResultBoolean() { Data = Roles.RoleExists(roleName) };
        }
    }
}
