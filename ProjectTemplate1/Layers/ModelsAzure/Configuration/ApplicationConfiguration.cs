using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.Serialization;
using $customNamespace$.Models.Configuration.ConfigSections.AzureRoles;

namespace $customNamespace$.Models.Configuration
{
    public static class ApplicationConfigurationAzure
    {
        private static IAzureRolesConfiguration _AzureRolesConfigurationSection = null;
        public static IAzureRolesConfiguration AzureRolesConfigurationSection
        {
            get
            {
                if (_AzureRolesConfigurationSection == null)
                {
                    _AzureRolesConfigurationSection = new AzureRolesConfiguration();
                }

                return _AzureRolesConfigurationSection;
            }
        }
    }
}