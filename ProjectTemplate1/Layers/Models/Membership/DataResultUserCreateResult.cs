using System;
using System.Runtime.Serialization;
using System.Web.Security;
using $safeprojectname$.Common;

namespace $safeprojectname$.Membership
{
    public interface ICreatedAccountResultModel
    {
        Guid ActivateUserToken { get; set; }
        MembershipCreateStatus CreateStatus { get; set; }
    }

    [DataContract]
    public class CreatedAccountResultModel : ICreatedAccountResultModel
    {
        public CreatedAccountResultModel() { }
        public CreatedAccountResultModel(MembershipCreateStatus status)
        {
            this.CreateStatus = status;
        }

        [DataMember]
        public Guid ActivateUserToken { get; set; }

        [DataMember]
        public MembershipCreateStatus CreateStatus { get; set; }
    }

    [DataContract]
    public class DataResultUserCreateResult : baseDataResult<CreatedAccountResultModel>, IDataResultModel<CreatedAccountResultModel>
    {

    }
}
