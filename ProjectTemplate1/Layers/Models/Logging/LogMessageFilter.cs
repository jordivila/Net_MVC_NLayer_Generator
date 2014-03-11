using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.DataAnnotationsAttributes;
using System.ComponentModel.DataAnnotations;
using $customNamespace$.Resources.Helpers.GeneratedResxClasses;
using $customNamespace$.Resources.DataAnnotations;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using $customNamespace$.Resources.LogViewer;

namespace $customNamespace$.Models.Logging
{
    [DataContract]
    [Serializable]
    public class DataFilterLogger : baseModel, IDataFilter
    {

        [DataMember]
        [XmlElement]
        public string LogTraceListenerSelected { get; set; }

        [DataMember]
        [XmlElement]
        public string LogTraceSourceSelected { get; set; }

        [DataMember]
        [Date(ErrorMessageResourceName = DataAnnotationsResourcesKeys.DateAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [Display(ResourceType = typeof(LogViewerTexts), Name = LogViewerTextsKeys.CreationDateFrom)]
        [Required]
        [XmlElement]
        public DateTime CreationDate { get; set; }

        //[DataMember]
        //[Date(ErrorMessageResourceName = DataAnnotationsResourcesKeys.DateAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        //[DateGreaterThan("CreationDateFrom", ErrorMessageResourceName = LogViewerTextsKeys.CreationDateToGreaterThanFrom, ErrorMessageResourceType = typeof(LogViewerTexts))]
        //[Display(ResourceType = typeof(LogViewerTexts), Name = LogViewerTextsKeys.CreationDateTo)]
        //[Required]
        //[XmlElement]
        //public DateTime CreationDateTo { get; set; }

        //[DataMember]
        //[XmlElement]
        //public TableContinuationToken NextContinuationToken { get; set; }

        //[DataMember]
        //[XmlElement]
        //public TableContinuationToken PreviousContinuationToken { get; set; }

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