using System;
using System.ServiceModel;

namespace $safeprojectname$.Roles
{
    [ServiceContract]
    public interface IProviderRoles: IDisposable
    {
        [OperationContract]
        string[] GetRolesForCurrentUser();

        [OperationContract]
        bool IsCurrentUserInRole(string roleName);
    }
}
