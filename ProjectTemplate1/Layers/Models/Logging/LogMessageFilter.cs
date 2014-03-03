using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.DataAnnotationsAttributes;
using System.ComponentModel.DataAnnotations;
using $customNamespace$.Resources.Helpers.GeneratedResxClasses;
using $customNamespace$.Resources.DataAnnotations;

namespace $customNamespace$.Models.Logging
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
        [Date(ErrorMessageResourceName = DataAnnotationsResourcesKeys.DateAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [Display(ResourceType = typeof(LogViewerTextsKeys), Name = LogViewerTextsKeys.CreationDateFrom)]
        [Required]
        [XmlElement]
        public DateTime CreationDateFrom { get; set; }

        [DataMember]
        [Date(ErrorMessageResourceName = DataAnnotationsResourcesKeys.DateAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [DateGreaterThan("CreationDateFrom", ErrorMessageResourceName = LogViewerTextsKeys.CreationDateToGreaterThanFrom, ErrorMessageResourceType = typeof(LogViewerTextsKeys))]
        [Display(ResourceType = typeof(LogViewerTextsKeys), Name = LogViewerTextsKeys.CreationDateTo)]
        [Required]
        [XmlElement]
        public DateTime CreationDateTo { get; set; }



        //[DataMember]
        //[XmlElement]
        //public LogMessageModel LogMessage { get; set; }

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
