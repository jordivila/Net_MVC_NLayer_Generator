using System.Collections.Specialized;
using System.Configuration;
using $safeprojectname$.Configuration.ConfigSections.ClientResources;
using $safeprojectname$.Configuration.ConfigSections.DomainInfo;
using $safeprojectname$.Configuration.ConfigSections.Mailing;

namespace $safeprojectname$.Configuration
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
    }
}