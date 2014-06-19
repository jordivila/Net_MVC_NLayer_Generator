using System;
using $customNamespace$.Models.Common;


namespace $customNamespace$.Models.Roles
{
    public interface IRoleAdminDAL: IDisposable
    {
        DataResultBoolean AddToRoles(string userName, string[] roles);
        DataResultBoolean Create(string roleName);
        DataResultBoolean Delete(string roleName);
        DataResultStringArray FindAll();
        DataResultStringArray FindByUserName(string userName);
        DataResultStringArray FindUserNamesByRole(string roleName);
        DataResultBoolean IsInRole(string user, string roleName);
        DataResultBoolean RemoveFromRoles(string userName, string[] roles);
        DataResultBoolean RoleExists(string roleName);
    }
}
