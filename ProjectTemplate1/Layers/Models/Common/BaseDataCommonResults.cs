using System.Runtime.Serialization;

namespace $safeprojectname$.Common
{
    [DataContract]
    public class DataResultBoolean : baseDataResult<bool>, IDataResultModel<bool>
    {
    }

    [DataContract]
    public class DataResultStringArray : baseDataResult<string[]>, IDataResultModel<string[]>
    {
    }

    [DataContract]
    public class DataResultString : baseDataResult<string>, IDataResultModel<string>
    {
    }

}
