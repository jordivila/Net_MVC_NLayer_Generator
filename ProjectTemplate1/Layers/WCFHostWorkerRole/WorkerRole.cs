using Microsoft.WindowsAzure.ServiceRuntime;
using $customNamespace$.Models.Configuration;
using $customNamespace$.Models.Host;
using $customNamespace$.Models.Unity;
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
        private volatile List<ServiceHost> serviceHostInstances = new List<ServiceHost>();

        public override void Run()
        {
            HostInitializer hostInitializer = new HostInitializer();
            hostInitializer.Start_EnterpriseLibrary(UnityContainerProvider.GetContainer(BackEndUnityContainerAvailable.Real));

            foreach (var item in BaseService.GetAllServiceTypes())
            {
                serviceHostInstances.Add(hostInitializer.Start_ServiceHost(item, Host_SetAzureInternalIPAddress));
            }

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        /// <summary>
        /// This method replace Endpoint Address setted at .config file by Azure Role Internal Ip Address
        /// </summary>
        private Action<ServiceHost> Host_SetAzureInternalIPAddress = delegate(ServiceHost host)
        {
            ServiceEndpoint endpointConfigFile = host.Description.Endpoints.First();
            EndpointAddress endpointAddressToUse = ApplicationConfiguration.AzureRolesConfigurationSection.ReplaceEndpointAddressAuthorityByRoleEndpoint(
                endpointConfigFile.Address,
                ApplicationConfiguration.AzureRolesConfigurationSection.WCF_RoleName,
                ApplicationConfiguration.AzureRolesConfigurationSection.WCF_InternalEndPointName);

            host.Description.Endpoints.First().ListenUri = endpointAddressToUse.Uri;
            host.Description.Endpoints.First().Address = endpointAddressToUse;
        };

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            foreach (var item in serviceHostInstances)
            {
                item.Close();
            }

            base.OnStop();
        }


    }
}