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
        private volatile List<ServiceHost> servicesHost = new List<ServiceHost>();

        public override void Run()
        {
            HostInitializer hostInitializer = new HostInitializer();
            hostInitializer.Start_EnterpriseLibrary(UnityContainerProvider.GetContainer(BackEndUnityContainerAvailable.Real));

            List<Type> allServices = BaseService.GetAllServiceTypes();
            foreach (var item in allServices)
            {
                this.StartWCFService(item, item.GetInterfaces().Where(x => x.Namespace.Contains("$customNamespace$.Models")).First());
            }

            foreach (var item in servicesHost)
            {
                Trace.TraceInformation(string.Format("{0}", item.Description.Endpoints.First().Address.Uri));
            }

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

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        private void StartWCFService(Type serviceType, Type contractType)
        {
            Trace.TraceInformation("Starting WCF service host...");
            ServiceHost serviceHost = new ServiceHost(serviceType);

            // Use NetTcpBinding with no security
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);

            // Define an external endpoint for client traffic
            RoleInstanceEndpoint externalEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["External"];
            serviceHost.AddServiceEndpoint(contractType, binding, String.Format("net.tcp://{0}/External/{1}", externalEndPoint.IPEndpoint, contractType.Name));

            // Define an internal endpoint for inter-role traffic
            RoleInstanceEndpoint internalEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Internal"];
            serviceHost.AddServiceEndpoint(contractType, binding, String.Format("net.tcp://{0}/Internal/{1}", internalEndPoint.IPEndpoint, contractType.Name));

            try
            {
                serviceHost.Open();
                Trace.TraceInformation("WCF service host started successfully.");
            }
            catch (TimeoutException timeoutException)
            {
                Trace.TraceError("The service operation timed out. {0}", timeoutException.Message);
            }
            catch (CommunicationException communicationException)
            {
                Trace.TraceError("Could not start WCF service host. {0}", communicationException.Message);
            }

            this.servicesHost.Add(serviceHost);
        }
    }
}