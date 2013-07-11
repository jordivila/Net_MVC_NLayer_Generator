using System;
using $customNamespace$.Models.Membership;

namespace $safeprojectname$.MembershipServices
{
    public interface IMembershipDAL : IProviderMembership
    {
        DataResultUserCantAccess  ResetPassword(MembershipUserWrapper user, string newPassword, string confirmNewPassword);
        DataResultUserActivate ActivateAccount(MembershipUserWrapper user, Guid activateUserToken);
    }
}
