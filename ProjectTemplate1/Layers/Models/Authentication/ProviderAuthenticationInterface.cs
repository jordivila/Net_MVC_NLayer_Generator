using System;
using System.ServiceModel;
using System.Web.Security;

namespace $safeprojectname$.Authentication
{
    [ServiceContract]
    public interface IProviderAuthentication: IDisposable
    {
        [OperationContract]
        bool IsLoggedIn();

        [OperationContract]
        bool LogIn(string userName, string password, string customCredential, bool isPersistent);

        [OperationContract]
        void LogOut();

        [OperationContract]
        bool ValidateUser(string userName, string password, string customCredential);

        [OperationContract]
        FormsIdentity GetFormsIdentity();
    }
}
