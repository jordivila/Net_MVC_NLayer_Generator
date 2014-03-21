using $customNamespace$.Models.Host;
using $customNamespace$.Models.Unity;
using $customNamespace$.WCF.ServicesHostCommon.Unity;
using $customNamespace$.WCF.ServicesLibrary;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace $customNamespace$.WCF.ServicesHost
{
    class Program
    {
        private static List<ServiceHost> serviceHostInstances = new List<ServiceHost>();


        static void Main()
        {
            HostInitializer hostInitializer = new HostInitializer();
            hostInitializer.Start_EnterpriseLibrary(UnityContainerProvider.GetContainer(BackEndUnityContainerAvailable.Real));

            foreach (var item in BaseService.GetAllServiceTypes())
            {
                serviceHostInstances.Add(hostInitializer.Start_ServiceHost(item));
            }

            Console.WriteLine("Press <ENTER> to stop services...");
            Console.ReadLine();

            foreach (var item in serviceHostInstances)
            {
                item.Close();
            }
        }
    }
}