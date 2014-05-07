using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Unity;
using $customNamespace$.WCF.ServicesHostCommon;
using $customNamespace$.WCF.ServicesHostCommon.Unity;
using $customNamespace$.WCF.ServicesLibrary;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;

namespace $customNamespace$.WCF.ServicesHost
{
    class Program
    {
        static void Main()
        {
            using (BackendHostInitializer hostInitializer = new BackendHostInitializerForCustomHost(BackEndUnityContainerAvailable.Real))
            {
                Console.WriteLine("Press <ENTER> to stop services...");
                Console.ReadLine();
            }
        }
    }

    public class BackendHostInitializerForCustomHost : BackendHostInitializer
    {
        public BackendHostInitializerForCustomHost(BackEndUnityContainerAvailable unityContainer)
            : base(unityContainer)
        {

        }

        protected override void DatabaseCnnStrings_Init()
        {
            // Do nothing. Take Database cnn strings from app.config
        }

        protected override void BackEndServices_EndpointsInit()
        {
            // Do nothing. Take service endpoints from app.config
        }

        protected override void BackEndServices_TraceEvent(string eventName, ServiceHost service)
        {
            Console.WriteLine(string.Format("{0} {1}", eventName, service.Description.Endpoints.First().Contract.ContractType.FullName));
        }
    }

}