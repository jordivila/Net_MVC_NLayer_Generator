using System.Configuration;

namespace $safeprojectname$.Configuration.ConfigSections.DomainInfo
{
    public interface IDomainInfoConfiguration
    {
        string DomainName { get; set; }
        string SecurityProtocol { get; set; }
    }

    internal class DomainInfoSettingsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("domainInfo")]
        public DomainInfoElement DomainInfo
        {
            get
            {
                DomainInfoElement domainInfo = (DomainInfoElement)base["domainInfo"];
                return domainInfo;
            }
        }
    }

    internal class DomainInfoElement : ConfigurationElement
    {
        [ConfigurationProperty("domainName")]
        public string DomainName
        {
            get
            {
                return (string)this["domainName"];
            }
            set
            {
                this["domainName"] = value;
            }
        }

        [ConfigurationProperty("securityProtocol")]
        public string SecurityProtocol
        {
            get
            {
                return (string)this["securityProtocol"];
            }
            set
            {
                this["securityProtocol"] = value;
            }
        }
    }

    public class DomainInfoConfiguration : IDomainInfoConfiguration
    {
        private static DomainInfoSettingsConfigSection domainInfoConfigSection = (DomainInfoSettingsConfigSection)System.Configuration.ConfigurationManager.GetSection("templateSettings/domainInfoSettings");

        public string DomainName
        {
            get
            {
                //return $safeprojectname$.Configuration.ApplicationConfiguration.DomainInfoSettingsSection.DomainInfo.DomainName;
                //DomainInfoSettingsConfigSection featuresSection = (DomainInfoSettingsConfigSection)System.Configuration.ConfigurationManager.GetSection("templateSettings/domainInfoSettings");
                //return featuresSection.DomainInfo.DomainName;
                return domainInfoConfigSection.DomainInfo.DomainName;
            }
            set
            {

            }
        }

        public string SecurityProtocol
        {
            get
            {
                //return $safeprojectname$.Configuration.ApplicationConfiguration.DomainInfoSettingsSection.DomainInfo.SecurityProtocol;
                //DomainInfoSettingsConfigSection featuresSection = (DomainInfoSettingsConfigSection)System.Configuration.ConfigurationManager.GetSection("templateSettings/domainInfoSettings");
                //return featuresSection.DomainInfo.SecurityProtocol;
                return domainInfoConfigSection.DomainInfo.SecurityProtocol;
            }
            set
            {

            }
        }
    }
}
