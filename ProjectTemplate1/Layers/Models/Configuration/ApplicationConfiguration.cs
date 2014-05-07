using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.Serialization;
using $customNamespace$.Models.Configuration.ConfigSections.ClientResources;
using $customNamespace$.Models.Configuration.ConfigSections.DomainInfo;
using $customNamespace$.Models.Configuration.ConfigSections.Mailing;

namespace $customNamespace$.Models.Configuration
{
    public partial class ApplicationConfiguration
    {
        public enum DatabaseNames : int
        {
            [EnumMember(Value = "DbCnnStrMembership")]
            Membership,

            [EnumMember(Value = "DbCnnStrLogging")]
            Logging,

            [EnumMember(Value = "DbCnnStrTokenPersistence")]
            TokenPersistence
        }

        public static bool IsDebugMode
        {
            get
            {
#if DEBUG==true
                return true;
#else
                            return false;
#endif
            }
        }

        public static NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        private static IMailingConfiguration _MailingSettingsSection = null;
        public static IMailingConfiguration MailingSettingsSection
        {
            get
            {
                if (_MailingSettingsSection == null)
                {
                    _MailingSettingsSection = new MailingConfiguration();
                }

                return _MailingSettingsSection;
            }
        }

        private static IDomainInfoConfiguration _DomainInfoSettingsSection = null;
        public static IDomainInfoConfiguration DomainInfoSettingsSection
        {
            get
            {
                if (_DomainInfoSettingsSection == null)
                {
                    _DomainInfoSettingsSection = new DomainInfoConfiguration();
                }

                return _DomainInfoSettingsSection;
            }
        }

        private static IClientResourcesConfiguration _ClientResourcesSettingsSection = null;
        public static IClientResourcesConfiguration ClientResourcesSettingsSection
        {
            get
            {
                if (_ClientResourcesSettingsSection == null)
                {
                    _ClientResourcesSettingsSection = new ClientResourcesConfiguration();
                }

                return _ClientResourcesSettingsSection;
            }
        }
    }
}