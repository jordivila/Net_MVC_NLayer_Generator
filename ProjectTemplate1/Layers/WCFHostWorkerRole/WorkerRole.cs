using Microsoft.WindowsAzure.ServiceRuntime;
using $customNamespace$.Models.Configuration;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Unity;
using $customNamespace$.WCF.ServicesHostCommon;
using $customNamespace$.WCF.ServicesHostCommon.Unity;
using $customNamespace$.WCF.ServicesLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;

namespace $customNamespace$.WCF.ServicesHostWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private volatile BackendHostInitializer hostInitializer = null;

        public override void Run()
        {
            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            hostInitializer = new BackendHostInitializer(BackEndUnityContainerAvailable.Real, HostingPlatform.Azure);

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            if (hostInitializer != null)
            {
                hostInitializer.Dispose();
            }

            base.OnStop();
        }
    }
}