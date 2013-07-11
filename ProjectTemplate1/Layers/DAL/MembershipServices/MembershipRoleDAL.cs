using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Caching;
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

        private string CacheManagerName = "CacheManagerForRolesDAL";
        private string CacheManager_GetKeyFindAll()
        {
            return "Cache_GetKeyFindAll";
        }
        private string CacheManager_GetKeyFindByUserName(string userName)
        {
            return string.Format("Cache_GetKeyFindByUserName_{0}", userName);
        }

        public virtual DataResultBoolean AddToRoles(string userName, string[] roles)
        {
            ICacheManager _objCacheManager = CacheFactory.GetCacheManager(this.CacheManagerName);
            _objCacheManager.Remove(this.CacheManager_GetKeyFindByUserName(userName));

            Roles.AddUserToRoles(userName, roles);
            return new DataResultBoolean() { Data = true };
        }

        public virtual DataResultBoolean Create(string roleName)
        {
            ICacheManager _objCacheManager = CacheFactory.GetCacheManager(this.CacheManagerName);
            _objCacheManager.Remove(this.CacheManager_GetKeyFindAll());

            Roles.CreateRole(roleName);
            return new DataResultBoolean() { Data = true };
        }

        public virtual DataResultBoolean Delete(string roleName)
        {
            ICacheManager _objCacheManager = CacheFactory.GetCacheManager(this.CacheManagerName);
            _objCacheManager.Remove(this.CacheManager_GetKeyFindAll());

            return new DataResultBoolean() { Data = Roles.DeleteRole(roleName, true) }; //throws exception if role has members
        }

        public virtual DataResultStringArray FindAll()
        {
            ICacheManager _objCacheManager = CacheFactory.GetCacheManager(this.CacheManagerName);
            if (!_objCacheManager.Contains(this.CacheManager_GetKeyFindAll()))
            {
                _objCacheManager.Add(this.CacheManager_GetKeyFindAll(), new DataResultStringArray() { Data = Roles.GetAllRoles() });
            }
            return (DataResultStringArray)_objCacheManager.GetData(this.CacheManager_GetKeyFindAll());
        }

        public virtual DataResultStringArray FindByUserName(string userName)
        {
            ICacheManager _objCacheManager = CacheFactory.GetCacheManager(this.CacheManagerName);
            if (!_objCacheManager.Contains(this.CacheManager_GetKeyFindByUserName(userName)))
            {
                _objCacheManager.Add(this.CacheManager_GetKeyFindByUserName(userName), new DataResultStringArray() { Data = Roles.GetRolesForUser(userName) });
            }
            return (DataResultStringArray)_objCacheManager.GetData(this.CacheManager_GetKeyFindByUserName(userName));
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
            ICacheManager _objCacheManager = CacheFactory.GetCacheManager(this.CacheManagerName);
            _objCacheManager.Remove(this.CacheManager_GetKeyFindByUserName(user));

            Roles.RemoveUserFromRoles(user, roles);
            return new DataResultBoolean() { Data = true };
        }

        public virtual DataResultBoolean RemoveUsersFromRole(string[] users, string[] roles)
        {
            foreach (var item in users)
            {
                ICacheManager _objCacheManager = CacheFactory.GetCacheManager(this.CacheManagerName);
                _objCacheManager.Remove(this.CacheManager_GetKeyFindByUserName(item));
            }

            Roles.RemoveUsersFromRoles(users, roles);
            return new DataResultBoolean() { Data = true };
        }

        public virtual DataResultBoolean RoleExists(string roleName)
        {
            return new DataResultBoolean() { Data = Roles.RoleExists(roleName) };
        }
    }
}
