using Microsoft.Practices.Unity;
using $customNamespace$.DAL.MembershipServices;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.Unity;

namespace $safeprojectname$.MembershipServices
{
    public interface IProfileBL : IProviderProfile
    {
        DataResultUserProfile Create(string userName);
    }

    public class ProfileBL : BaseBL, IProfileBL
    {
        private IProfileDAL _dal;
        public ProfileBL()
        {
            using (DependencyFactory dependencyFactory = new DependencyFactory())
            {
                _dal = dependencyFactory.Unity.Resolve<IProfileDAL>();
            }
        }
        public override void Dispose()
        {
            base.Dispose();

            if (this._dal != null)
            {
                this._dal.Dispose();
            }
        }

        //public Dictionary<string, object> GetAllPropertiesForCurrentUser(bool authenticatedUserOnly)
        //{
        //    if (this.UserRequest.UserIsLoggedIn)
        //    {
        //        return this._dal.GetAllPropertiesForCurrentUser(authenticatedUserOnly);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public Dictionary<string, object> GetPropertiesForCurrentUser(IEnumerable<string> properties, bool authenticatedUserOnly)
        //{
        //    if (this.UserRequest.UserIsLoggedIn)
        //    {
        //        return this._dal.GetPropertiesForCurrentUser(properties, authenticatedUserOnly);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public ProfilePropertyMetadata[] GetPropertiesMetadata()
        //{
        //    return this._dal.GetPropertiesMetadata();
        //}

        //public Collection<string> SetPropertiesForCurrentUser(IDictionary<string, object> values, bool authenticatedUserOnly)
        //{
        //    if (this.UserRequest.UserIsLoggedIn)
        //    {
        //        return this._dal.SetPropertiesForCurrentUser(values, authenticatedUserOnly);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public DataResultUserProfile Create(string userName)
        {
            return this._dal.Create(userName);
        }

        public DataResultUserProfile Get()
        {
            return this._dal.Get();
        }

        public DataResultUserProfile Update(UserProfileModel userProfile)
        {
            DataResultUserProfile result = this._dal.Update(userProfile);
            result.Message = Resources.UserAdministration.UserAdminTexts.UserSaved;
            return result;
        }
    }
}
