using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using $safeprojectname$.Common;

namespace $safeprojectname$.Logging
{
    [DataContract]
    [Serializable]
    public class DataFilterLogger : baseModel, IDataFilter
    {
        //[DataMember]
        //[XmlElement]
        //public DateTime? CreationDate { get; set; }

        [DataMember]
        [XmlElement]
        public string LogTraceSourceSelected { get; set; }

        [DataMember]
        [XmlElement]
        public LogMessageModel LogMessage { get; set; }

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
}
