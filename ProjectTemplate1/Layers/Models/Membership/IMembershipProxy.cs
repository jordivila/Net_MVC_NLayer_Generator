using System;
using System.ServiceModel;
using $safeprojectname$.Common;

namespace $safeprojectname$.Membership
{
    [ServiceContract]
    public interface IMembershipProxy: IDisposable
    {
        [OperationContract]
        DataResultUserActivate ActivateAccount(Guid activateUserToken);

        [OperationContract]
        DataResultUserCantAccess CantAccessYourAccount(string activateFormVirtualPath, string email);

        [OperationContract]
        DataResultUserCantAccess ResetPassword(Guid guid, string newPassword, string confirmNewPassword);

        [OperationContract]
        DataResultBoolean ChangePassword(string username, string oldPassword, string newPassword, string newPasswordConfirm);

        [OperationContract]
        DataResultUserCreateResult CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, string urlActivateForm);

        [OperationContract]
        DataResultBoolean DeleteUser(string username, bool deleteAllRelatedData);

        [OperationContract]
        DataResultUser GetUserByGuid(object providerUserKey, bool userIsOnline);

        [OperationContract]
        DataResultUser GetUserByName(string username, bool userIsOnline);

        [OperationContract]
        DataResultUserSearch GetUserList(DataFilterUserList filter);

        [OperationContract]
        DataResultProviderSettings Settings();

        [OperationContract]
        DataResultBoolean UnlockUser(string userName);

        [OperationContract]
        DataResultBoolean UpdateUser(MembershipUserWrapper user);

        [OperationContract]
        DataResultBoolean ValidateUser(string userName, string passWord);
    }
}
