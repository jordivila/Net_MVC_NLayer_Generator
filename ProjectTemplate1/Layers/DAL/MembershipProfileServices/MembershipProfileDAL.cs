using System.Web.Profile;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using $customNamespace$.Models;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Profile;

namespace $safeprojectname$.MembershipServices
{
    public class ProfileDAL : BaseDAL, IProfileDAL
    {
        public ProfileDAL()
        {

        }

        public virtual DataResultUserProfile Create(string userName)
        {
            UserProfileModel userProfile = new UserProfileModel();
            userProfile.UserName = userName;
            userProfile.Culture = this.UserRequest.UserProfile.Culture;
            userProfile.Theme = this.UserRequest.UserProfile.Theme;

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

            //UserProfileDALHelper p = (UserProfileDALHelper)UserProfileDALHelper.Create(username);
            //p.Culture = this.UserRequest.CultureSelected;
            //p.Theme = this.UserRequest.ThemeSelected;
            //p.Save();
            //return p;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private string CacheManagerName = "CacheManagerForProfileDAL";
        private string CacheManager_GetKey()
        {
            return string.Format("Cache_DataResultUserProfile_{0}", this.UserRequest.UserFormsIdentity.Name);
        }

        public virtual DataResultUserProfile Get()
        {
            ICacheManager _objCacheManager = CacheFactory.GetCacheManager(CacheManagerName);
            if (!_objCacheManager.Contains(this.CacheManager_GetKey()))
            {
                ProfileBase p = ProfileBase.Create(this.UserRequest.UserFormsIdentity.Name, this.UserRequest.UserFormsIdentity.IsAuthenticated);
                UserProfileModel userProfile = new UserProfileModel(p);
                DataResultUserProfile result = new DataResultUserProfile()
                {
                    IsValid = true,
                    Data = userProfile,
                    MessageType = DataResultMessageType.Success
                };

                _objCacheManager.Add(this.CacheManager_GetKey(), result);
            }
            return (DataResultUserProfile)_objCacheManager.GetData(this.CacheManager_GetKey());
        }

        public virtual DataResultUserProfile Update(UserProfileModel userProfile)
        {
            ICacheManager _objCacheManager = CacheFactory.GetCacheManager(CacheManagerName);
            if (_objCacheManager.Contains(this.CacheManager_GetKey()))
            {
                _objCacheManager.Remove(this.CacheManager_GetKey());
            }

            ProfileBase p = ProfileBase.Create(this.UserRequest.UserFormsIdentity.Name);
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
