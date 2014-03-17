using System;
using $customNamespace$.Models.Common;

namespace $customNamespace$.Models.Membership
{
    public interface IMembershipDAL : IDisposable
    {
        DataResultUserActivate ActivateAccount(MembershipUserWrapper user, Guid activateUserToken);
        DataResultUserCantAccess CantAccessYourAccount(string activateFormVirtualPath, string email);
        DataResultBoolean ChangePassword(string username, string oldPassword, string newPassword, string newPasswordConfirm);
        DataResultUserCreateResult CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, string activateFormVirtualPath);
        DataResultBoolean DeleteUser(string username, bool deleteAllRelatedData);
        DataResultUser GetUserByGuid(object providerUserKey, bool userIsOnline);
        DataResultUser GetUserByName(string username, bool userIsOnline);
        DataResultUserSearch GetUserList(DataFilterUserList filter);
        DataResultUserCantAccess ResetPassword(MembershipUserWrapper user, string newPassword, string confirmNewPassword);
        DataResultProviderSettings Settings();
        DataResultBoolean UnlockUser(string userName);
        DataResultBoolean UpdateUser(MembershipUserWrapper user);
        DataResultBoolean ValidateUser(string userName, string password);
    }
}