using System;
using System.ServiceModel;

namespace $safeprojectname$.Profile
{
    [ServiceContract]
    public interface IProviderProfile: IDisposable
    {
        [OperationContract]
        DataResultUserProfile Get();

        [OperationContract]
        DataResultUserProfile Update(UserProfileModel userProfile);
    }
}
