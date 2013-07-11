using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using $safeprojectname$.Common;
using $safeprojectname$.DataAnnotationsAttributes;
using $customNamespace$.Resources.DataAnnotations;
using $customNamespace$.Resources.Helpers.GeneratedResxClasses;
using $customNamespace$.Resources.UserAdministration;
using $customNamespace$.Resources.General;

namespace $safeprojectname$.Membership
{
    [DataContract]
    [Serializable]
    public class DataFilterUserList : baseModel, IDataFilter
    {
        private string _userName;
        [DataMember]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.EmailAddress)]
        [Required(ErrorMessageResourceType = typeof(GeneralTexts), ErrorMessageResourceName = GeneralTextsKeys.Required)]
        [StringLength(1024, MinimumLength = 3, ErrorMessageResourceType = typeof(DataAnnotationsResources), ErrorMessageResourceName = DataAnnotationsResourcesKeys.StringLengthAttribute_Invalid)]
        [XmlElement]
        public string UserName
        {
            get { return _userName == null ? _userName : _userName.Trim(); }
            set { _userName = value; }
        }   

        [DataMember]
        [Date(ErrorMessageResourceName = DataAnnotationsResourcesKeys.DateAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.CreationDateFrom)]
        [XmlElement]
        public DateTime? CreationDateFrom { get; set; }

        [DataMember]
        [Date(ErrorMessageResourceName = DataAnnotationsResourcesKeys.DateAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [DateGreaterThan("CreationDateFrom", ErrorMessageResourceName = UserAdminTextsKeys.CreationDateToGreaterThanFrom, ErrorMessageResourceType = typeof(UserAdminTexts))]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.CreationDateTo)]
        [XmlElement]
        public DateTime? CreationDateTo { get; set; }

        private string _email;
        [DataMember]
        [Email(ErrorMessageResourceName = DataAnnotationsResourcesKeys.EmailAddressAttribute_Invalid, ErrorMessageResourceType = typeof(DataAnnotationsResources))]
        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.EmailAddress)]
        [XmlElement]
        public string Email
        {
            get { return _email == null ? _email : _email.Trim(); }
            set { _email = value; }
        }

        [DataMember]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.Approved)]
        [XmlElement]
        public bool? Approved { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.Locked)]
        [XmlElement]
        public bool? Locked { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.IsOnline)]
        [XmlElement]
        public bool? IsOnline { get; set; }

        private List<string> _isInRoleName;
        [DataMember]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.FilterByRoleName)]
        [XmlArray(ElementName="IsInRoleName")]
        public List<string> IsInRoleName
        {
            get
            {
                if (this._isInRoleName == null)
                    this._isInRoleName = new List<string>();
                return this._isInRoleName;
            }
            set
            {
                this._isInRoleName = value;
            }
        }

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

        [DataMember]
        [XmlElement]
        public bool IsClientVisible { get; set; }
    }
}
