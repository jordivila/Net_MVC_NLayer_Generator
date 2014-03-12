using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Unity;

namespace $safeprojectname$.Unity
{
    public abstract class UnityContainerProvider
    {
        private static UnityContainerProviderReal unityContainerProviderReal = new UnityContainerProviderReal();
        //private static UnityContainerProviderDevelopment unityContainerProviderDevelopment = new UnityContainerProviderDevelopment();

        public static IUnityContainer GetContainer(UnityContainerAvailable containerSelected)
        {
            IUnityContainer result = null;

            switch (containerSelected)
            {
                case UnityContainerAvailable.Real:
                    result = UnityContainerProvider.unityContainerProviderReal.GetContainer();
                    break;
                //case UnityContainerAvailable.MockDALDevelopment:
                //    result = UnityContainerProvider.unityContainerProviderDevelopment.GetContainer();
                //    break;
                default:
                    throw new Exception("IUnityContainer does not exist in the list of available providers");
            }

            return result;
        }

        internal abstract IUnityContainer GetContainer();
    }
}