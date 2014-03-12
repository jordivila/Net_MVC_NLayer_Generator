using System.Runtime.Serialization;
using System.Web.Security;

namespace $safeprojectname$.Membership
{
    [DataContract]
    public class MembershipProviderSettings : IMembershipProviderSettings
    {
        [DataMember]
        public  string ApplicationName { get; set; }

        [DataMember]
        public  bool EnablePasswordReset{get; set;}

        [DataMember]
        public  bool EnablePasswordRetrieval{get; set;}

        [DataMember]
        public  int MaxInvalidPasswordAttempts{get; set;}

        [DataMember]
        public  int MinRequiredNonAlphanumericCharacters{get; set;}

        [DataMember]
        public  int MinRequiredPasswordLength{get; set;}

        [DataMember]
        public  int PasswordAttemptWindow{get; set;}

        [DataMember]
        public  MembershipPasswordFormat PasswordFormat{get; set;}

        [DataMember]
        public  string PasswordStrengthRegularExpression{get; set;}

        [DataMember]
        public  bool RequiresQuestionAndAnswer{get; set;}

        [DataMember]
        public  bool RequiresUniqueEmail { get; set; }
    }

    public interface IMembershipProviderSettings
    {
        string ApplicationName { get; set; }
        bool EnablePasswordReset { get; set; }
        bool EnablePasswordRetrieval { get; set; }
        int MaxInvalidPasswordAttempts { get; set; }
        int MinRequiredNonAlphanumericCharacters { get; set; }
        int MinRequiredPasswordLength { get; set; }
        int PasswordAttemptWindow { get; set; }
        MembershipPasswordFormat PasswordFormat { get; set; }
        string PasswordStrengthRegularExpression { get; set; }
        bool RequiresQuestionAndAnswer { get; set; }
        bool RequiresUniqueEmail { get; set; }
    }
}
