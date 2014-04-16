using System.Collections.Specialized;
using System.Configuration;
using $customNamespace$.Models.Configuration.ConfigSections.ClientResources;
using $customNamespace$.Models.Configuration.ConfigSections.DomainInfo;
using $customNamespace$.Models.Configuration.ConfigSections.Mailing;
using $customNamespace$.Models.Configuration.ConfigSections.AzureRoles;
using $customNamespace$.Models.Configuration.ConfigSections.BackendServices;

namespace $customNamespace$.Models.Configuration
{
    public class ApplicationConfiguration
    {
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

        public enum DatabaseNames : int
        {
            Membership
            , Logging
        }

        public static IMailingConfiguration MailingSettingsSection = new MailingConfiguration();

        public static IDomainInfoConfiguration DomainInfoSettingsSection = new DomainInfoConfiguration();

        public static IClientResourcesConfiguration ClientResourcesSettingsSection = new ClientResourcesConfiguration();

        public static IAzureRolesConfiguration AzureRolesConfigurationSection = new AzureRolesConfiguration();

        public static IBackendServicesConfiguration BackendServicesConfiguration = new BackendServicesConfiguration();
    }
}