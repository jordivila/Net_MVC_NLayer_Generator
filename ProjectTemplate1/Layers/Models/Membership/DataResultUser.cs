using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Security;
using $safeprojectname$.Common;
using $customNamespace$.Resources.Helpers.GeneratedResxClasses;
using $customNamespace$.Resources.UserAdministration;
//using $customNamespace$.Resources.Helpers.GeneratedResxClasses;



namespace $safeprojectname$.Membership
{
    public interface IMembershipUserWrapper
    {
        string Comment { get; set; }
        DateTime CreateDate { get; set; }
        string Email { get; set; }
        bool IsApproved { get; set; }
        bool IsLockedOut { get; set; }
        bool IsOnline { get; set; }
        DateTime LastActivityDate { get; set; }
        DateTime LastLockoutDate { get; set; }
        DateTime LastLoginDate { get; set; }
        DateTime LastPasswordChangedDate { get; set; }
        string PasswordQuestion { get; set; }
        string ProviderName { get; set; }
        Guid ProviderUserKey { get; set; }
        string UserName { get; set; }
    }

    [DataContract]
    public class MembershipUserWrapper : baseModel, IMembershipUserWrapper
    {
        public MembershipUserWrapper() { }
        public MembershipUserWrapper(MembershipUser userSource)
        {
            this.Comment = userSource.Comment;
            this.CreateDate = userSource.CreationDate;
            this.Email = userSource.Email;
            this.IsApproved = userSource.IsApproved;
            this.IsLockedOut = userSource.IsLockedOut;
            this.IsOnline = userSource.IsOnline;
            this.LastActivityDate = userSource.LastActivityDate;
            this.LastLockoutDate = userSource.LastLockoutDate;
            this.LastLoginDate = userSource.LastLoginDate;
            this.LastPasswordChangedDate = userSource.LastPasswordChangedDate;
            this.PasswordQuestion = userSource.PasswordQuestion;
            this.ProviderName = userSource.ProviderName;
            this.ProviderUserKey = Guid.Parse(userSource.ProviderUserKey.ToString());
            this.UserName = userSource.UserName;
        }

        public MembershipUser GetMembershipUser()
        {
            return new MembershipUser(this.ProviderName, this.UserName, this.ProviderUserKey, this.Email, this.PasswordQuestion, this.Comment, this.IsApproved, this.IsLockedOut, this.CreateDate, this.LastLoginDate, this.LastActivityDate, this.LastPasswordChangedDate, this.LastLockoutDate);
        }

        [DataMember]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.Comments)]
        public string Comment { get; set; }

        [DataMember]
        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.CreationDate)]
        public DateTime CreateDate { get; set; }

        [DataMember]
        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.Email)]
        public string Email { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.Approved)]
        public bool IsApproved { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.Locked)]
        public bool IsLockedOut { get; set; }

        [DataMember]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.Online)]
        public bool IsOnline { get; set; }

        [DataMember]
        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.LastActivity)]
        public DateTime LastActivityDate { get; set; }

        [DataMember]
        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.LastLockOut)]
        public DateTime LastLockoutDate { get; set; }

        [DataMember]
        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.LastLogin)]
        public DateTime LastLoginDate { get; set; }

        [DataMember]
        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.LastPasswordChange)]
        public DateTime LastPasswordChangedDate { get; set; }

        [DataMember]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.PasswordQuestion)]
        public string PasswordQuestion { get; set; }

        [DataMember]
        public string ProviderName { get; set; }

        [DataMember]
        public Guid ProviderUserKey { get; set; }

        [DataMember]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(UserAdminTexts), Name = UserAdminTextsKeys.UserName)]
        public string UserName { get; set; }
    }

    [DataContract]
    public class DataResultUser : baseDataResult<MembershipUserWrapper>, IDataResultModel<MembershipUserWrapper>
    {
    }
}
