using System;
using System.Collections.Generic;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Profile;

namespace $safeprojectname$.MembershipServices
{
    public class ProfileDALMock : ProfileDAL, IProfileDAL
    {
        public static Dictionary<string, UserProfileModel> source = new Dictionary<string, UserProfileModel>();
        public static Dictionary<string, string> sourcePassword = new Dictionary<string, string>();

        public override DataResultUserProfile Create(string userName)
        {
            UserProfileModel userProfile = new UserProfileModel();
            userProfile.UserName = userName;
            userProfile.Culture = this.UserRequest.UserProfile.Culture;
            userProfile.Theme = this.UserRequest.UserProfile.Theme;

            if (source.ContainsKey(userName))
            {
                throw new Exception("UserName already exists");
            }
            else
            {
                source.Add(userName, userProfile);
            }

            DataResultUserProfile result = new DataResultUserProfile()
            {
                IsValid = true,
                Data = userProfile,
                MessageType = DataResultMessageType.Success
            };

            return result;
        }

        public override DataResultUserProfile Update(UserProfileModel userProfile)
        {
            if (source.ContainsKey(this.UserRequest.UserFormsIdentity.Name))
            {
                source[this.UserRequest.UserFormsIdentity.Name] = userProfile;

                DataResultUserProfile result = new DataResultUserProfile()
                {
                    IsValid = true,
                    Data = userProfile,
                    MessageType = DataResultMessageType.Success
                };

                return result;
            }
            else
            {
                throw new Exception("Username does not exists");
            }
        }

        public override DataResultUserProfile Get()
        {
            if (source.ContainsKey(this.UserRequest.UserFormsIdentity.Name))
            {
                DataResultUserProfile result = new DataResultUserProfile()
                {
                    IsValid = true,
                    Data = source[this.UserRequest.UserFormsIdentity.Name],
                    MessageType = DataResultMessageType.Success
                };

                return result;
            }
            else
            {
                throw new Exception("Username does not exists");
            }
        }

    }
}
