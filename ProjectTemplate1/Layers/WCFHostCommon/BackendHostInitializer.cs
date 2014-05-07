using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using $customNamespace$.Models;
using $customNamespace$.Models.Configuration;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Unity;
using $customNamespace$.WCF.ServicesHostCommon.Unity;
using $customNamespace$.WCF.ServicesLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace $customNamespace$.WCF.ServicesHostCommon
{
    public abstract class BackendHostInitializer : IDisposable
    {
        private List<ServiceHost> servicesHost = new List<ServiceHost>();

        public BackendHostInitializer(BackEndUnityContainerAvailable unityContainer)
        {
            this.DatabaseCnnStrings_Init();
            this.EnterpriseLibrary_Init(UnityContainerProvider.GetContainer(unityContainer));
            this.BackEndServices_Init();
        }


        #region Database Strings

        protected abstract void DatabaseCnnStrings_Init();

        #endregion

        #region Enterprise Library

        private void EnterpriseLibrary_Init(IUnityContainer unityContainer)
        {
            DependencyFactory.SetUnityContainerProviderFactory(unityContainer);
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            LogWriterFactory logWriterFactory = new LogWriterFactory();
            Logger.SetLogWriter(logWriterFactory.Create());
        }

        #endregion

        #region BackEndServices

        private void BackEndServices_Init()
        {
            this.BackEndServices_EndpointsInit();
            this.BackEndServices_HostsInit();
        }
        protected abstract void BackEndServices_EndpointsInit();
        protected abstract void BackEndServices_TraceEvent(string eventName, ServiceHost service);
        protected void BackEndServices_HostsInit()
        {
            System.Configuration.Configuration configCurrent = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServicesSection servicesSection = configCurrent.GetSection("system.serviceModel/services") as ServicesSection;

            Assembly serviceLibraryAssembly = Assembly.GetAssembly(typeof(BaseService));
            for (int i = 0; i < servicesSection.Services.Count; i++)
            {
                this.BackEndServices_HostCreate(serviceLibraryAssembly.GetType(servicesSection.Services[i].Name));
            }
        }
        private ServiceHost BackEndServices_HostCreate(Type serviceType)
        {
            ServiceHost serviceHost = new ServiceHost(serviceType);
            serviceHost.Closed += delegate(object sender, EventArgs e) { this.BackEndServices_TraceEvent("Closed: ", (ServiceHost)sender); };
            serviceHost.Closing += delegate(object sender, EventArgs e) { this.BackEndServices_TraceEvent("Closing: ", (ServiceHost)sender); };
            serviceHost.Faulted += delegate(object sender, EventArgs e) { this.BackEndServices_TraceEvent("Faulted: ", (ServiceHost)sender); };
            serviceHost.Opened += delegate(object sender, EventArgs e) { this.BackEndServices_TraceEvent("Opened: ", (ServiceHost)sender); };
            serviceHost.Opening += delegate(object sender, EventArgs e) { this.BackEndServices_TraceEvent("Opening: ", (ServiceHost)sender); };
            //serviceHost.UnknownMessageReceived += delegate(object sender, UnknownMessageReceivedEventArgs e) { traceThis(string.Format("{0} \n {1}", "UnknownMessageReceived:", e.Message), (ServiceHost)sender); };

            try
            {
                serviceHost.Open();
            }
            catch (CommunicationException communicationException)
            {
                Trace.TraceError("Could not start WCF service host. {0}", communicationException.Message);
            }

            return serviceHost;
        }

        #endregion

        public void Dispose()
        {
            foreach (var item in this.servicesHost)
            {
                item.Close();
            }
        }
    }
}