using System;
using $customNamespace$.BL.MembershipServices;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Membership;

namespace $safeprojectname$.AspNetApplicationServices.Admin
{
    public class MembershipServices : BaseService, IProviderMembership
    {
        private IMembershipBL _bl;

        public MembershipServices()
        {
            _bl = new MembershipBL();
        }
        public override void Dispose()
        {
            base.Dispose();

            if (this._bl != null)
            {
                this._bl.Dispose();
            }
        }

        public DataResultUserCantAccess CantAccessYourAccount(string urlActiveteForm, string email)
        {
            return this._bl.CantAccessYourAccount(urlActiveteForm, email);
        }
        public DataResultUserCantAccess ResetPassword(Guid guid, string newPassword, string confirmNewPassword)
        {
            return this._bl.ResetPassword(guid, newPassword, confirmNewPassword);
        }
        public DataResultBoolean ChangePassword(string username, string oldPassword, string newPassword, string newPasswordConfirm)
        {
            return this._bl.ChangePassword(username, oldPassword, newPassword, newPasswordConfirm);
        }
        public DataResultUserCreateResult CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, string activateFormVirtualPath)
        {
            return this._bl.CreateUser(username, password, email, passwordQuestion, passwordAnswer, activateFormVirtualPath);
        }
        public DataResultBoolean DeleteUser(string username, bool deleteAllRelatedData)
        {
            return this._bl.DeleteUser(username, deleteAllRelatedData);
        }
        public DataResultUserSearch GetUserList(DataFilterUserList filter)
        {
            DataResultUserSearch result = this._bl.GetUserList(filter);
            return result;
        }
        public DataResultProviderSettings Settings()
        {
            return this._bl.Settings();
        }
        public DataResultBoolean UnlockUser(string userName)
        {
            return this._bl.UnlockUser(userName);
        }
        public DataResultBoolean UpdateUser(MembershipUserWrapper user)
        {
            return this._bl.UpdateUser(user);
        }
        public DataResultBoolean ValidateUser(string userName, string passWord)
        {
            return this._bl.ValidateUser(userName, passWord);
        }
        public DataResultUserActivate ActivateAccount(Guid activationUserToken)
        {
            return this._bl.ActivateAccount(activationUserToken);
        }
        public DataResultUser GetUserByGuid(object providerUserKey, bool userIsOnline)
        {
            return this._bl.GetUserByGuid(providerUserKey, userIsOnline);
        }
        public DataResultUser GetUserByName(string username, bool userIsOnline)
        {
            return this._bl.GetUserByName(username, userIsOnline);
        }
    }
}
