using System.Configuration;

namespace $safeprojectname$.Configuration.ConfigSections.Mailing
{
    public interface IMailingConfiguration
    {
        string SupportTeamEmailAddress { get; set; }
    }

    internal class MailingSettingsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("mailAddresses")]
        public MailAddressesElement MailAddresses
        {
            get
            {
                MailAddressesElement mailAddresses = (MailAddressesElement)base["mailAddresses"];
                return mailAddresses;
            }
        }
    }

    internal class MailAddressesElement : ConfigurationElement
    {
        [ConfigurationProperty("supportTeamEmailAddress")]
        public string SupportTeamEmailAddress
        {
            get
            {
                return (string)this["supportTeamEmailAddress"];
            }
            set
            {
                this["supportTeamEmailAddress"] = value;
            }
        }
    }

    public class MailingConfiguration : IMailingConfiguration
    {
        private static MailingSettingsConfigSection mailingConfigSection = (MailingSettingsConfigSection)System.Configuration.ConfigurationManager.GetSection("templateSettings/mailingSettings");

        public string SupportTeamEmailAddress
        {
            get
            {
                return mailingConfigSection.MailAddresses.SupportTeamEmailAddress;
            }
            set
            {

            }
        }
    }
}
