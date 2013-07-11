using System;
using System.Collections.Generic;
using System.Linq;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;

namespace $safeprojectname$.MembershipServices
{
    public class RoleAdminDALMock : BaseDAL, IRoleAdminDAL
    {
        private static object LockObject = new object();
        private static Dictionary<string, List<string>> source = new Dictionary<string, List<string>>() { 
            {SiteRoles.Administrator.ToString(), new List<string>()},
            {SiteRoles.Guest.ToString(), new List<string>()}
        };

        public RoleAdminDALMock()
            : base()
        {

        }

        public DataResultBoolean AddToRoles(string userName, string[] roles)
        {
            bool result = false;

            lock (LockObject)
            {
                foreach (var item in roles)
                {
                    if (source.ContainsKey(item))
                    {
                        source[item].Add(userName);
                        result = true;
                    }
                }

            }
            return new DataResultBoolean() { Data = result };
        }
        public DataResultBoolean Create(string roleName)
        {
            lock (LockObject)
            {
                source.Add(roleName, new List<string>());
            }

            return new DataResultBoolean() { Data = true };
        }
        public DataResultBoolean Delete(string roleName)
        {
            lock (LockObject)
            {
                if (source.ContainsKey(roleName))
                {
                    if (source[roleName].Count > 0)
                    {
                        throw new Exception("ASP NET Membership throws exception if role has members");
                    }
                    else
                    {
                        source.Remove(roleName);
                    }
                }
            }
            return new DataResultBoolean() { Data = true };
        }
        public DataResultStringArray FindAll()
        {
            DataResultStringArray result = null;
            lock (LockObject)
            {
                result = new DataResultStringArray() { Data = source.Select(x => x.Key).ToArray() };
            }
            return result;
        }
        public DataResultStringArray FindByUserName(string userName)
        {
            DataResultStringArray result = null;
            lock (LockObject)
            {
                result = new DataResultStringArray() { Data = source.Where(k => k.Value.Contains(userName)).Select(x => x.Key).ToArray() };
            }
            return result;
        }
        public DataResultStringArray FindUserNamesByRole(string roleName)
        {
            DataResultStringArray result = null;
            lock (LockObject)
            {
                result = new DataResultStringArray() { Data = source.Where(x => x.Key == roleName).Select(x => x.Value).SelectMany(k => k).Distinct().ToArray() };
            }
            return result;
        }
        public DataResultBoolean IsInRole(string user, string roleName)
        {
            DataResultBoolean result = null;
            lock (LockObject)
            {
                result = new DataResultBoolean() { Data = source.Where(x => x.Key == roleName).Where(k => k.Value.Contains(user)).ToList().Count > 0 };
            }
            return result;
        }
        public DataResultBoolean RemoveFromRoles(string user, string[] roles)
        {
            DataResultBoolean result = null;
            lock (LockObject)
            {
                foreach (var item in roles)
                {
                    if (source.ContainsKey(item))
                    {
                        source[item].Remove(user);
                        result = new DataResultBoolean() { Data = true };
                    }
                }
            }
            return result;
        }
        public DataResultBoolean RemoveUsersFromRole(string[] users, string[] roles)
        {
            DataResultBoolean result = null;
            lock (LockObject)
            {
                foreach (var item in roles)
                {
                    if (source.ContainsKey(item))
                    {
                        foreach (var itemUser in users)
                        {
                            source[item].Remove(itemUser);
                            result = new DataResultBoolean() { Data = true };
                        }
                    }
                }
            }
            return result;
        }
        public DataResultBoolean RoleExists(string roleName)
        {
            DataResultBoolean result = null;
            lock (LockObject)
            {
                result = new DataResultBoolean() { Data = source.ContainsKey(roleName) };
            }
            return result;
        }
    }
}
