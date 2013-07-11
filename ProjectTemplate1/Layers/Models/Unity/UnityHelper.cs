using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using $safeprojectname$.Configuration;

namespace $safeprojectname$.Unity
{

    public class DependencyFactory : IDisposable
    {
        private static IUnityContainer _unity;

        public static void ConfigurationSet(IUnityContainer container)
        {
            // Used at testing runtime to set Mock objects 
            DependencyFactory._unity = container;
        }

        public IUnityContainer Unity
        {
            get
            {
                if (DependencyFactory._unity == null)
                {
                    DependencyFactory._unity = DependencyFactory.ConfigurationLoadDefault(new UnityContainer());
                }
                return DependencyFactory._unity;
            }
        }

        public static IUnityContainer ConfigurationLoadDefault(IUnityContainer container)
        {
            if (string.IsNullOrEmpty(ApplicationConfiguration.UnitySettingsSection.CurrentContainer))
            {
                container.LoadConfiguration();
            }
            else
            {
                container.LoadConfiguration(ApplicationConfiguration.UnitySettingsSection.CurrentContainer);
            }
            return container;
        }

        public virtual void Dispose()
        {
            if (DependencyFactory._unity != null)
            {
                DependencyFactory._unity.Dispose();
            }
        }
    }


    //public class DependencyFactory : IDisposable
    //{
    //    private IUnityContainer _unity;

    //    public IUnityContainer Unity
    //    {
    //        get
    //        {
    //            if (this._unity == null)
    //            {
    //                this._unity = new UnityContainer();

    //                if (string.IsNullOrEmpty(ApplicationConfiguration.UnitySettingsSection.CurrentContainer))
    //                {
    //                    this._unity.LoadConfiguration();
    //                }
    //                else
    //                {
    //                    this._unity.LoadConfiguration(ApplicationConfiguration.UnitySettingsSection.CurrentContainer);
    //                }
    //            }
    //            return this._unity;
    //        }
    //    }

    //    public virtual void Dispose()
    //    {
    //        if (this._unity != null)
    //        {
    //            this._unity.Dispose();
    //        }
    //    }
    //}

}
