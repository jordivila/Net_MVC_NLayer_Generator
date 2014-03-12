using System;

namespace $customNamespace$.Models.Membership
{
    public interface IMembershipDAL : IMembershipProxy
    {
        DataResultUserCantAccess  ResetPassword(MembershipUserWrapper user, string newPassword, string confirmNewPassword);
        DataResultUserActivate ActivateAccount(MembershipUserWrapper user, Guid activateUserToken);
    }
}
