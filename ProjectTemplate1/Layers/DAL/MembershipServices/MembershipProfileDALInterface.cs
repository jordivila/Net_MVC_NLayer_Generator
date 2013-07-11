using $customNamespace$.Models.Profile;

namespace $safeprojectname$.MembershipServices
{
    public interface IProfileDAL : IProviderProfile
    {
        DataResultUserProfile Create(string userName);
    }
}
