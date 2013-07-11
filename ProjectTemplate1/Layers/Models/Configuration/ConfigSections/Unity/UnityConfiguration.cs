using System.Configuration;

namespace $safeprojectname$.Configuration.ConfigSections.Unity
{
    public interface IUnityConfiguration
    {
        string CurrentContainer { get; set; }
    }

    internal class UnitySettingsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("unityInfo")]
        public UnityInfoElement UnityInfo
        {
            get
            {
                UnityInfoElement UnityInfo = (UnityInfoElement)base["unityInfo"];
                return UnityInfo;
            }
        }
    }

    internal class UnityInfoElement : ConfigurationElement
    {
        [ConfigurationProperty("currentContainer")]
        public string CurrentContainer
        {
            get
            {
                return (string)this["currentContainer"];
            }
            set
            {
                this["currentContainer"] = value;
            }
        }
    }

    public class UnityConfiguration : IUnityConfiguration
    {
        private static UnitySettingsConfigSection UnityConfigSection = (UnitySettingsConfigSection)System.Configuration.ConfigurationManager.GetSection("templateSettings/unitySettings");

        public string CurrentContainer
        {
            get
            {
                return UnityConfigSection.UnityInfo.CurrentContainer;
            }
            set
            {

            }
        }
    }
}
