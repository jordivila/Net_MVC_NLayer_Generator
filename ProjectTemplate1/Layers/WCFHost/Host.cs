using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Unity;
using $customNamespace$.WCF.ServicesHostCommon;
using $customNamespace$.WCF.ServicesHostCommon.Unity;
using $customNamespace$.WCF.ServicesLibrary;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace $customNamespace$.WCF.ServicesHost
{
    class Program
    {
        static void Main()
        {
            using (BackendHostInitializer hostInitializer = new BackendHostInitializer(BackEndUnityContainerAvailable.Real, HostingPlatform.Custom))
            {
                Console.WriteLine("Press <ENTER> to stop services...");
                Console.ReadLine();
            }
        }
    }
}