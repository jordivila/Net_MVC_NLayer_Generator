using System.Web.Profile;
using System.Runtime.Caching;
using $customNamespace$.Models;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.UserRequestModel;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace $customNamespace$.DAL.MembershipServices
{
    public class ProfileDAL : BaseDAL, IProfileDAL
    {
        private const string CacheManagerName = "CacheManagerForProfileDAL";
        private ObjectCache _objCacheManager = new MemoryCache(CacheManagerName);
        private CacheItemPolicy _objCachePolicy = new CacheItemPolicy();
        private string CacheManager_GetKey(IUserRequestModel userRequest)
        {
            return string.Format("Cache_DataResultUserProfile_{0}", userRequest.UserFormsIdentity.Name);
        }

        public virtual DataResultUserProfile Create(string userName, IUserRequestModel userRequest)
        {
            UserProfileModel userProfile = new UserProfileModel();
            userProfile.UserName = userName;
            userProfile.Culture = userRequest.UserProfile.Culture;
            userProfile.Theme = userRequest.UserProfile.Theme;

            ProfileBase p = ProfileBase.Create(userName);
            p.SetPropertyValue(baseModel.GetInfo(() => userProfile.Culture).Name, userProfile.Culture.Name.ToString());
            p.SetPropertyValue(baseModel.GetInfo(() => userProfile.Theme).Name, userProfile.Theme.Value.ToString());
            p.Save();

            DataResultUserProfile result = new DataResultUserProfile()
            {
                IsValid = true,
                Data = userProfile,
                MessageType = $customNamespace$.Models.Enumerations.DataResultMessageType.Success
            };

            return result;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public virtual DataResultUserProfile Get(IUserRequestModel userRequest)
        {
            if (!_objCacheManager.Contains(this.CacheManager_GetKey(userRequest)))
            {
                ProfileBase p = ProfileBase.Create(userRequest.UserFormsIdentity.Name, userRequest.UserFormsIdentity.IsAuthenticated);
                UserProfileModel userProfile = new UserProfileModel(p);
                DataResultUserProfile result = new DataResultUserProfile()
                {
                    IsValid = true,
                    Data = userProfile,
                    MessageType = DataResultMessageType.Success
                };

                _objCacheManager.Add(this.CacheManager_GetKey(userRequest), result, _objCachePolicy);
            }
            return (DataResultUserProfile)_objCacheManager.Get(this.CacheManager_GetKey(userRequest));
        }

        public virtual DataResultUserProfile Update(UserProfileModel userProfile, IUserRequestModel userRequest)
        {
            if (_objCacheManager.Contains(this.CacheManager_GetKey(userRequest)))
            {
                _objCacheManager.Remove(this.CacheManager_GetKey(userRequest));
            }

            ProfileBase p = ProfileBase.Create(userRequest.UserFormsIdentity.Name);
            userProfile.SetProfileBasePropertyValues(ref p);
            p.Save();
            DataResultUserProfile result = new DataResultUserProfile()
            {
                IsValid = true,
                Data = userProfile,
                MessageType = DataResultMessageType.Success
            };
            return result;
        }
    }
}