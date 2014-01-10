using System.Linq;
using System.Transactions;
using Microsoft.Practices.Unity;
using $customNamespace$.DAL.MembershipServices;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Roles;
using $customNamespace$.Models.Unity;
using $customNamespace$.Resources.UserAdministration;

namespace $safeprojectname$.MembershipServices
{
    public interface IRoleAdminBL : IProviderRoleManager
    {

    }

    public class RoleAdminBL : BaseBL, IRoleAdminBL
    {
        private IRoleAdminDAL _dal;
        public RoleAdminBL()
        {
            _dal = DependencyFactory.Resolve<IRoleAdminDAL>();
        }
        public override void Dispose()
        {
            base.Dispose();

            if (this._dal != null)
            {
                this._dal.Dispose();
            }
        }
        public DataResultBoolean AddToRoles(string userName, string[] roles)
        {
            string[] userRoles = this.FindByUserName(userName).Data.ToArray();
            string[] userNewRoles = (from r in roles
                                     where !userRoles.Contains(r)
                                     select r).ToArray();

            if (userNewRoles.Count() > 0)
            {
                return this._dal.AddToRoles(userName, userNewRoles);
            }
            else
            {
                return new DataResultBoolean()
                {
                    IsValid = true,
                    Message = string.Empty,
                    MessageType = DataResultMessageType.Success,
                    Data = true
                };
            }
        }
        public DataResultBoolean Create(string roleName)
        {
            if (this._dal.RoleExists(roleName).Data)
            {
                return new DataResultBoolean()
                {
                    IsValid = false,
                    Message = string.Format(UserAdminTexts.RoleAlreadyExists, roleName),
                    MessageType = DataResultMessageType.Error,
                    Data = false
                };
            }
            else
            {
                return this._dal.Create(roleName);
            }
        }
        public DataResultBoolean Delete(string roleName)
        {
            if (_dal.RoleExists(roleName).Data)
            {
                if (!(_dal.FindUserNamesByRole(roleName).Data.Count() > 0))
                {
                    return this._dal.Delete(roleName);
                }
                else
                {
                    return new DataResultBoolean()
                    {
                        IsValid = false,
                        Message = UserAdminTexts.RoleHasPendingUsersError,
                        MessageType = DataResultMessageType.Error,
                        Data = false
                    };

                }
            }
            else
            {
                return new DataResultBoolean()
                {
                    IsValid = false,
                    Message = UserAdminTexts.RoleUnexists,
                    MessageType = DataResultMessageType.Error,
                    Data = false
                };
            }
        }
        public DataResultStringArray FindAll()
        {
            return this._dal.FindAll();
        }
        public DataResultStringArray FindByUserName(string userName)
        {
            return this._dal.FindByUserName(userName);
        }
        public DataResultStringArray FindUserNamesByRole(string roleName)
        {
            return this._dal.FindUserNamesByRole(roleName);
        }
        public DataResultBoolean IsInRole(string user, string roleName)
        {
            return this._dal.IsInRole(user, roleName);
        }
        public DataResultBoolean RemoveFromRoles(string userName, string[] roles)
        {
            string[] userRoles = this.FindByUserName(userName).Data.ToArray();
            string[] userExistingRoles = (from r in roles
                                     where userRoles.Contains(r)
                                     select r).ToArray();

            if (userExistingRoles.Count() > 0)
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    DataResultBoolean result = this._dal.RemoveFromRoles(userName, userExistingRoles);
                    if (result.IsValid)
                    {
                        if (this.FindByUserName(userName).Data.Count() > 0)
                        {
                            trans.Complete();
                        }
                        else
                        {
                            result.IsValid = false;
                            result.MessageType = DataResultMessageType.Error;
                            result.Message = UserAdminTexts.UserRolesNeeded;

                            trans.Dispose();
                        }
                    }
                    else
                    {
                        trans.Dispose();
                    }
                    return result;
                }
            }
            else
            {

                return new DataResultBoolean() { Data = true };
            }
        }
        public DataResultBoolean RoleExists(string roleName)
        {
            return _dal.RoleExists(roleName);
        }
    }
}