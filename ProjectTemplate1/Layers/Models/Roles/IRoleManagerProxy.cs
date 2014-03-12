using System;
using System.ServiceModel;
using $customNamespace$.Models.Common;

namespace $customNamespace$.Models.Roles
{
    [ServiceContract]
    public interface IRoleManagerProxy : IDisposable
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
