using System;
using System.ServiceModel;
using $safeprojectname$.Common;

namespace $safeprojectname$.Roles
{
    [ServiceContract]
    public interface IProviderRoleManager : IDisposable
    {
        [OperationContract]
        DataResultBoolean AddToRoles(string userName, string[] roles);

        [OperationContract]
        DataResultBoolean Create(string roleName);

        [OperationContract]
        DataResultBoolean Delete(string roleName);

        [OperationContract]
        DataResultStringArray FindAll();

        [OperationContract]
        DataResultStringArray FindByUserName(string userName);

        [OperationContract]
        DataResultStringArray FindUserNamesByRole(string roleName);

        [OperationContract]
        DataResultBoolean IsInRole(string user, string roleName);

        [OperationContract]
        DataResultBoolean RemoveFromRoles(string userName, string[] roles);

        [OperationContract]
        DataResultBoolean RoleExists(string roleName);
    }
}
