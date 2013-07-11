using System.Runtime.Serialization;
using $safeprojectname$.Common;

namespace $safeprojectname$.Membership
{
    [DataContract]
    public class DataResultUserList : baseDataPagedResult<MembershipUserWrapper>, IDataResultPaginatedModel<MembershipUserWrapper>
    { 
    
    }
}
