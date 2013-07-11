using System;
using System.Runtime.Serialization;
using System.ServiceModel.Syndication;
using System.Xml.Serialization;
using $safeprojectname$.Common;

namespace $safeprojectname$.Syndication
{
    [DataContract]
    [Serializable]
    public class DataFilterSyndication : baseModel, IDataFilter
    {
        [DataMember]
        [XmlElement]
        public string CategoryName { get; set; }

        [DataMember]
        [XmlElement]
        public string Uri { get; set; }

        [DataMember]
        [XmlElement]
        public bool IsClientVisible { get; set; }

        [DataMember]
        [XmlElement]
        public int? Page { get; set; }

        [DataMember]
        [XmlElement]
        public int PageSize { get; set; }

        [DataMember]
        [XmlElement]
        public string SortBy { get; set; }

        [DataMember]
        [XmlElement]
        public bool SortAscending { get; set; }
    }

    [DataContract]
    public class DataResultSyndicationItems : baseDataPagedResult<SyndicationItemFormatter>, IDataResultPaginatedModel<SyndicationItemFormatter>
    {
        
    }
}

