using $customNamespace$.BL.MembershipServices;
using $customNamespace$.Models.Profile;

namespace $customNamespace$.WCF.ServicesLibrary.AspNetApplicationServices
{
    public class ProfileService : BaseServiceWithCustomMessageHeaders, IProfileProxy
    {
        ProfileBL _bl = null;

        public ProfileService()
        {
            this._bl = new ProfileBL();
        }
        public override void Dispose()
        {
            if (this._bl != null)
            {
                this._bl.Dispose();
            }

            base.Dispose();
        }

        public DataResultUserProfile Get()
        {
            return this._bl.Get();
        }
        public DataResultUserProfile Update(UserProfileModel userProfile)
        {
            return this._bl.Update(userProfile);
        }
    }
}