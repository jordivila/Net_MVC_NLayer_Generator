using System;
using $safeprojectname$.Common;
using $safeprojectname$.ProxyProviders;


namespace $safeprojectname$.Membership
{
    public class MembershipProxy : ProviderBaseChannel<IMembershipProxy>, IMembershipProxy
    {
        public DataResultUserCantAccess CantAccessYourAccount(string activateFormVirtualPath, string email)
        {
            return this.proxy.CantAccessYourAccount(activateFormVirtualPath, email);
        }
        public DataResultUserCantAccess ResetPassword(Guid guid, string newPassword, string confirmNewPassword)
        {
            return this.proxy.ResetPassword(guid, newPassword, confirmNewPassword);
        }
        public DataResultBoolean ChangePassword(string username, string oldPassword, string newPassword, string newPasswordConfirm)
        {
            return this.proxy.ChangePassword(username, oldPassword, newPassword, newPasswordConfirm);
        }
        public DataResultUserCreateResult CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, string activateFormVirtualPath)
        {
            return this.proxy.CreateUser(username, password, email, passwordQuestion, passwordAnswer, activateFormVirtualPath);
        }
        public DataResultBoolean DeleteUser(string username, bool deleteAllRelatedData)
        {
            return this.proxy.DeleteUser(username, deleteAllRelatedData);
        }
        public DataResultUser GetUserByGuid(object providerUserKey, bool userIsOnline)
        {
            return this.proxy.GetUserByGuid(providerUserKey, userIsOnline);
        }
        public DataResultUser GetUserByName(string username, bool userIsOnline)
        {
            return this.proxy.GetUserByName(username, userIsOnline);
        }
        public DataResultUserSearch GetUserList(DataFilterUserList filter)
        {
            return this.proxy.GetUserList(filter);
        }
        public DataResultProviderSettings Settings()
        {
            return this.proxy.Settings();
        }
        public DataResultBoolean UnlockUser(string userName)
        {
            return this.proxy.UnlockUser(userName);
        }
        public DataResultBoolean UpdateUser(MembershipUserWrapper user)
        {
            return this.proxy.UpdateUser(user);
        }
        public DataResultBoolean ValidateUser(string userName, string passWord)
        {
            return this.proxy.ValidateUser(userName, passWord);
        }
        public DataResultUserActivate ActivateAccount(Guid activationUserToken)
        {
            return this.proxy.ActivateAccount(activationUserToken);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}