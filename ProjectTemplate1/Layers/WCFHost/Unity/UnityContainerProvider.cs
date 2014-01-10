using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace $customNamespace$.WCF.Unity
{
    public enum UnityContainerAvailable
    {
        Real,
        MockDAL,
        MockDALDevelopment
    }

    public abstract class UnityContainerProvider
    {
        private static UnityContainerProviderMockDALDevelopment unityContainerProviderMockDALDevelopment = new UnityContainerProviderMockDALDevelopment();
        private static UnityContainerProviderMockDAL unityContainerProviderMockDAL = new UnityContainerProviderMockDAL();
        private static UnityContainerProviderReal unityContainerProviderReal = new UnityContainerProviderReal();

        public static IUnityContainer GetContainer(UnityContainerAvailable containerSelected)
        {
            IUnityContainer result = null;

            switch (containerSelected)
            {
                case UnityContainerAvailable.Real:
                    result = UnityContainerProvider.unityContainerProviderReal.GetContainer();
                    break;
                case UnityContainerAvailable.MockDAL:
                    result = UnityContainerProvider.unityContainerProviderMockDAL.GetContainer();
                    break;
                case UnityContainerAvailable.MockDALDevelopment:
                    result = UnityContainerProvider.unityContainerProviderMockDALDevelopment.GetContainer();
                    break;
                default:
                    throw new Exception("IUnityContainer does not exist in the list of available providers");
            }

            return result;
        }

        internal abstract IUnityContainer GetContainer();
    }
}