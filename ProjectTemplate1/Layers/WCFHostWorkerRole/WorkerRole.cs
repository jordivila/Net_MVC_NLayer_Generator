using Microsoft.WindowsAzure.ServiceRuntime;
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

namespace $safeprojectname$
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
            // Use NetTcpBinding with no security
            //NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);

            //// Define an external endpoint for client traffic
            //RoleInstanceEndpoint externalEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["External"];
            //host.AddServiceEndpoint(contractType, binding, String.Format("net.tcp://{0}/External/{1}", externalEndPoint.IPEndpoint, contractType.Name));

            //// Define an internal endpoint for inter-role traffic
            RoleInstanceEndpoint internalEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Internal"];
            //host.AddServiceEndpoint(contractType, binding, String.Format("net.tcp://{0}/Internal/{1}", internalEndPoint.IPEndpoint, contractType.Name));

            ServiceEndpoint endpointConfigFile = host.Description.Endpoints.First();
            EndpointAddress endpointAddressToUse = new EndpointAddress(new Uri(
                endpointConfigFile.ListenUri.ToString().Replace(
                                                endpointConfigFile.ListenUri.Authority,
                                                string.Format("{0}:{1}",
                                                                internalEndPoint.IPEndpoint.Address.ToString(),
                                                                internalEndPoint.IPEndpoint.Port))).ToString());

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
