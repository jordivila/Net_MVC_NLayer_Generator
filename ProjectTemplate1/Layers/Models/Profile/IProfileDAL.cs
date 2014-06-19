using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using $customNamespace$.Models.UserRequestModel;

namespace $customNamespace$.Models.Profile
{
    public interface IProfileDAL : IDisposable
    {
        DataResultUserProfile Create(string userName, IUserRequestModel userRequest);

        DataResultUserProfile Get(IUserRequestModel userRequest);

        DataResultUserProfile Update(UserProfileModel userProfile, IUserRequestModel userRequest);
    }
}