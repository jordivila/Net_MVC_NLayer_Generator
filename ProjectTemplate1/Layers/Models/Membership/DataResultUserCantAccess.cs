using System;
using System.Runtime.Serialization;
using $safeprojectname$.Common;

namespace $safeprojectname$.Membership
{
    public interface ICantAccessMyAccountModel
    {
        Guid ChangePasswordToken { get; set; }
        MembershipUserWrapper User { get; set; }
    }

    [DataContract]
    public class CantAccessMyAccountModel : ICantAccessMyAccountModel
    {
        [DataMember]
        public MembershipUserWrapper User { get; set; }

        [DataMember]
        public Guid ChangePasswordToken { get; set; }
    }

    [DataContract]
    public class DataResultUserCantAccess : baseDataResult<CantAccessMyAccountModel>, IDataResultModel<CantAccessMyAccountModel>
    {
    }

}
