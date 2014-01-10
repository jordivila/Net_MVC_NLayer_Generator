using System;
using System.Collections.Generic;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Profile;

using $customNamespace$.Models.UserRequestModel;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace $safeprojectname$.MembershipServices
{
    public class ProfileDALMock : ProfileDAL, IProviderProfileDAL
    {
        public static Dictionary<string, UserProfileModel> source = new Dictionary<string, UserProfileModel>();
        public static Dictionary<string, string> sourcePassword = new Dictionary<string, string>();

        public override DataResultUserProfile Create(string userName, IUserRequestModel<OperationContext, MessageHeaders> userRequest)
        {
            UserProfileModel userProfile = new UserProfileModel();
            userProfile.UserName = userName;
            userProfile.Culture = userRequest.UserProfile.Culture;
            userProfile.Theme = userRequest.UserProfile.Theme;

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

        public override DataResultUserProfile Update(UserProfileModel userProfile, IUserRequestModel<OperationContext, MessageHeaders> userRequest)
        {
            if (source.ContainsKey(userRequest.UserFormsIdentity.Name))
            {
                source[userRequest.UserFormsIdentity.Name] = userProfile;

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

        public override DataResultUserProfile Get(IUserRequestModel<OperationContext, MessageHeaders> userRequest)
        {
            if (source.ContainsKey(userRequest.UserFormsIdentity.Name))
            {
                DataResultUserProfile result = new DataResultUserProfile()
                {
                    IsValid = true,
                    Data = source[userRequest.UserFormsIdentity.Name],
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
