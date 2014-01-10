using System;
using Microsoft.Practices.Unity;
using $safeprojectname$.Configuration;

namespace $safeprojectname$.Unity
{
    public static class DependencyFactory
    {
        private static IUnityContainer _unity;

        public static void SetUnityContainerProviderFactory(IUnityContainer container)
        {
            if (DependencyFactory._unity == null)
            {
                DependencyFactory._unity = container;
            }
            else
            {
                // No se puede cambiar el container de dependencias una vez iniciado
                // Eso seria un foyong que no sabe ni dooonde se ha metido
                throw new Exception("DependencyFactory cannot be changed once intialized");
            }
        }

        public static T Resolve<T>()
        {
            return DependencyFactory._unity.Resolve<T>();
        }
    }
}
