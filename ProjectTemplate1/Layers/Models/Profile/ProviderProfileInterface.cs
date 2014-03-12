using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using $safeprojectname$.UserRequestModel;

namespace $safeprojectname$.Profile
{
    public interface IProfileDAL : IDisposable
    {
        DataResultUserProfile Create(string userName, IUserRequestModel<OperationContext, MessageHeaders> userRequest);

        DataResultUserProfile Get(IUserRequestModel<OperationContext, MessageHeaders> userRequest);

        DataResultUserProfile Update(UserProfileModel userProfile, IUserRequestModel<OperationContext, MessageHeaders> userRequest);
    }
}
