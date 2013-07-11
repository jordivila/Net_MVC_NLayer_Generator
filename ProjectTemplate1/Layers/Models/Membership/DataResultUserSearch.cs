using System.Runtime.Serialization;
using $safeprojectname$.Common;

namespace $safeprojectname$.Membership
{
    [DataContract]
    public class DataResultUserSearch : baseDataResult<DataResultUserList>, IDataResultModel<DataResultUserList>
    {
    }
}
