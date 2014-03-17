using Microsoft.Practices.Unity;
using $customNamespace$.DAL.MembershipServices;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.Unity;

namespace $customNamespace$.BL.MembershipServices
{
    public class ProfileBL : BaseBL, IProfileBL
    {
        private IProfileDAL _dal;

        public ProfileBL()
        {
            _dal = DependencyFactory.Resolve<IProfileDAL>();

        }
        public override void Dispose()
        {
            base.Dispose();

            if (this._dal != null)
            {
                this._dal.Dispose();
            }
        }

        public DataResultUserProfile Create(string userName)
        {
            return this._dal.Create(userName, this.UserRequest);
        }

        public DataResultUserProfile Get()
        {
            return this._dal.Get(this.UserRequest);
        }

        public DataResultUserProfile Update(UserProfileModel userProfile)
        {
            DataResultUserProfile result = this._dal.Update(userProfile, this.UserRequest);
            result.Message = Resources.UserAdministration.UserAdminTexts.UserSaved;
            return result;
        }
    }
}
