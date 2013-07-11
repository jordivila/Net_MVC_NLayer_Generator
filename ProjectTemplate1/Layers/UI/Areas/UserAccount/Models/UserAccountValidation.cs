using System.Web.Security;

namespace $safeprojectname$.Areas.UserAccount.Models
{
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for a full list of status codes
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return $customNamespace$.Resources.UserAdministration.UserAdminTexts.DuplicateUserName;

                case MembershipCreateStatus.DuplicateEmail:
                    return $customNamespace$.Resources.UserAdministration.UserAdminTexts.DuplicateEmail;

                case MembershipCreateStatus.InvalidPassword:
                    return $customNamespace$.Resources.UserAdministration.UserAdminTexts.InvalidPassword;

                case MembershipCreateStatus.InvalidEmail:
                    return $customNamespace$.Resources.UserAdministration.UserAdminTexts.InvalidEmail;

                case MembershipCreateStatus.InvalidAnswer:
                    return $customNamespace$.Resources.UserAdministration.UserAdminTexts.InvalidAnswer;

                case MembershipCreateStatus.InvalidQuestion:
                    return $customNamespace$.Resources.UserAdministration.UserAdminTexts.InvalidQuestion;

                case MembershipCreateStatus.InvalidUserName:
                    return $customNamespace$.Resources.UserAdministration.UserAdminTexts.InvalidUserName;

                case MembershipCreateStatus.ProviderError:
                    return $customNamespace$.Resources.General.GeneralTexts.UnexpectedError;

                case MembershipCreateStatus.UserRejected:
                    return $customNamespace$.Resources.General.GeneralTexts.UnexpectedError;

                default:
                    return $customNamespace$.Resources.General.GeneralTexts.UnexpectedError;
            }
        }
    }
}