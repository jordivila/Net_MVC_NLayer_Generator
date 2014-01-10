using Microsoft.Practices.Unity;
using $customNamespace$.DAL.MembershipServices;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.Unity;

namespace $safeprojectname$.MembershipServices
{
    public class ProfileBL : BaseBL, IProviderProxyProfileServices
    {
        private IProviderProfileDAL _dal;

        public ProfileBL()
        {
            _dal = DependencyFactory.Resolve<IProviderProfileDAL>();

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
