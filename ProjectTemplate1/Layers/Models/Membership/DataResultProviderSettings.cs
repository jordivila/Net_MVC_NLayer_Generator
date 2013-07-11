using System.Runtime.Serialization;
using $safeprojectname$.Common;

namespace $safeprojectname$.Membership
{
    [DataContract]
    public class DataResultProviderSettings : baseDataResult<MembershipProviderSettings>, IDataResultModel<MembershipProviderSettings>
    {
    }
}
