using Microsoft.WindowsAzure.ServiceRuntime;
using $customNamespace$.Models.Configuration;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Unity;
using $customNamespace$.WCF.ServicesHostCommon;
using $customNamespace$.WCF.ServicesHostCommon.Unity;
using $customNamespace$.WCF.ServicesLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Configuration;
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

            hostInitializer = new BackendHostInitializerForAzure(BackEndUnityContainerAvailable.Real);

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

    public class BackendHostInitializerForAzure : BackendHostInitializer
    {
        public BackendHostInitializerForAzure(BackEndUnityContainerAvailable unityContainer)
            : base(unityContainer)
        {

        }

        protected override void DatabaseCnnStrings_Init()
        {
            System.Configuration.Configuration configCurrent = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringsSection cnnSection = configCurrent.GetSection("connectionStrings") as ConnectionStringsSection;

            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                cnnSection.ConnectionStrings[ConfigurationManager.ConnectionStrings[i].Name].ConnectionString = ApplicationConfigurationAzure.AzureRolesConfigurationSection.DatabaseCnnStringGetByName(ConfigurationManager.ConnectionStrings[i].Name);
            }

            configCurrent.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        protected override void BackEndServices_EndpointsInit()
        {
            System.Configuration.Configuration configCurrent = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServicesSection servicesSection = configCurrent.GetSection("system.serviceModel/services") as ServicesSection;

            IPEndPoint backendIPEndPoint = ApplicationConfigurationAzure.AzureRolesConfigurationSection.BackEndGetIpEndpoint();

            for (int i = 0; i < servicesSection.Services.Count; i++)
            {
                for (int j = 0; j < servicesSection.Services[i].Endpoints.Count; j++)
                {
                    Uri originalUri = servicesSection.Services[i].Endpoints[j].Address;
                    Uri azureHostUri = new Uri(originalUri.ToString().Replace(originalUri.Authority, backendIPEndPoint.ToString()));
                    servicesSection.Services[i].Endpoints[j].Address = azureHostUri;
                }
            }

            configCurrent.Save();
            ConfigurationManager.RefreshSection("system.serviceModel/services");
        }

        protected override void BackEndServices_TraceEvent(string eventName, ServiceHost service)
        {
            Trace.TraceInformation(string.Format("{0} {1}", eventName, service.Description.Endpoints.First().Contract.ContractType.FullName));
        }
    }

}