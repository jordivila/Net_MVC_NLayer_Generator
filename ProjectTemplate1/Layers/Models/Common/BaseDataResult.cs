using System.Collections.Generic;
using System.Runtime.Serialization;
using $customNamespace$.Models.Enumerations;

namespace $customNamespace$.Models.Common
{
    [DataContract]
    public abstract class baseDataResult<T> : baseModel, IDataResultModel<T>
    {
        public baseDataResult()
        {
            //by default we expect correct messages to be sent
            this.IsValid = true;
            this.Message = string.Empty;
            this.MessageType = DataResultMessageType.Success;
        }

        [DataMember]
        public T Data { get; set; }

        [DataMember]
        public bool IsValid { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DataResultMessageType MessageType { get; set; }
    }

    [DataContract]
    public abstract class baseDataPagedResult<T> : baseModel, IDataResultPaginatedModel<T>
    {
        [DataMember]
        public virtual int TotalPages
        {
            get
            {
                return this.TotalRows % this.PageSize > 0 ? (this.TotalRows / this.PageSize) + 1 : this.TotalRows / this.PageSize;
            }
            set
            {

            }
        }

        [DataMember]
        public int TotalRows { get; set; }

        [DataMember]
        public int? Page { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public string SortBy { get; set; }

        [DataMember]
        public bool SortAscending { get; set; }

        [DataMember]
        public List<T> Data { get; set; }
    }

    public interface IDataResultModel<T>
    {
        T Data { get; set; }
        bool IsValid { get; set; }
        string Message { get; set; }
        DataResultMessageType MessageType { get; set; }
    }

    public interface IDataResultPaginatedModel<T> : IPaginable, ISortable, IPaginated
    {
        List<T> Data { get; set; }
    }

    public interface IDataFilter : IPaginable, ISortable
    {
        bool IsClientVisible { get; set; }
    }

    public interface IPaginable
    {
        int? Page { get; set; }
        int PageSize { get; set; }
    }

    public interface IPaginated
    {
        int TotalPages { get; set; }
        int TotalRows { get; set; }
    }

    public interface ISortable
    {
        string SortBy { get; set; }
        bool SortAscending { get; set; }
    }
}
