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
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace $customNamespace$.WCF.ServicesHostCommon
{
    public class BackendHostInitializer : IDisposable
    {
        private List<ServiceHost> servicesHost = new List<ServiceHost>();

        public BackendHostInitializer(BackEndUnityContainerAvailable unityContainer, HostingPlatform hostingPlatform)
        {
            ///TODO: acabar esto. Al guardar la confoguracion en azure peta por unauthorized exception
            this.ConfigurationShared_Init(@"Template.WCF.ServiceHostCommon.config");

            this.EnterpriseLibrary_Init(UnityContainerProvider.GetContainer(unityContainer));
            this.BackEndServices_Init(hostingPlatform);
        }

        #region Enterprise Library Methods

        private void EnterpriseLibrary_Init(IUnityContainer unityContainer)
        {
            DependencyFactory.SetUnityContainerProviderFactory(unityContainer);
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            LogWriterFactory logWriterFactory = new LogWriterFactory();
            Logger.SetLogWriter(logWriterFactory.Create());
        }

        #endregion

        #region Host Methods

        private void BackEndServices_Init(HostingPlatform hostingPlatform)
        {
            foreach (var item in BaseService.GetAllServiceTypes())
            {
                this.servicesHost.Add(this.BackEndServices_Host_Create(item, hostingPlatform));
            }
        }
        private ServiceHost BackEndServices_Host_Create(Type serviceType, HostingPlatform hostingPlatform)
        {
            ServiceHost host = this.BackEndServices_Host_Init(serviceType, hostingPlatform);
            Action<string, ServiceHost> traceThis = delegate(string eventName, ServiceHost service)
            {
                Trace.TraceInformation(string.Format("{0} {1}", eventName, service.Description.Endpoints.First().Contract.ContractType.FullName));
            };

            host.Closed += delegate(object sender, EventArgs e) { traceThis(baseModel.GetCurrentMethod().Name, (ServiceHost)sender); };
            host.Closing += delegate(object sender, EventArgs e) { traceThis(baseModel.GetCurrentMethod().Name, (ServiceHost)sender); };
            host.Faulted += delegate(object sender, EventArgs e) { traceThis(baseModel.GetCurrentMethod().Name, (ServiceHost)sender); };
            host.Opened += delegate(object sender, EventArgs e) { traceThis(baseModel.GetCurrentMethod().Name, (ServiceHost)sender); };
            host.Opening += delegate(object sender, EventArgs e) { traceThis(baseModel.GetCurrentMethod().Name, (ServiceHost)sender); };
            host.UnknownMessageReceived += delegate(object sender, UnknownMessageReceivedEventArgs e) { traceThis(string.Format("{0} \n {1}", baseModel.GetCurrentMethod().Name, e.Message), (ServiceHost)sender); };

            try
            {
                host.Open();
            }
            catch (CommunicationException communicationException)
            {
                Trace.TraceError("Could not start WCF service host. {0}", communicationException.Message);
            }

            return host;
        }
        private ServiceHost BackEndServices_Host_Init(Type serviceType, HostingPlatform hostingPlatform)
        {
            Type contractType = serviceType.GetInterfaces()
                                            .Where(x => x.CustomAttributes.Any(c => c.AttributeType == typeof(ServiceContractAttribute)))
                                            .First();

            ContractDescription contractDescription = ContractDescription.GetContract(contractType);
            ServiceHost serviceHost = new ServiceHost(serviceType);
            Binding backendBinding = ApplicationConfiguration.BackendServicesConfiguration.GetBinding();
            IPEndPoint backendIPEndPoint = ApplicationConfiguration.BackendServicesConfiguration.GetIPEndPoint(hostingPlatform);
            serviceHost.AddServiceEndpoint(contractDescription.ContractType,
                                            backendBinding,
                                            ApplicationConfiguration.BackendServicesConfiguration.GetEndpoint(hostingPlatform, backendBinding, contractDescription));
            return serviceHost;
        }

        #endregion

        #region Shared Configuration Methods

        public void ConfigurationShared_Init(string sharedFile)
        {
            // Load shared configuration file into memory
            System.Configuration.Configuration configMapped;
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = sharedFile;
            configMapped = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            // Merge application current configuration
            System.Configuration.Configuration configCurrent = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configMapped.SaveAs(configCurrent.FilePath, ConfigurationSaveMode.Minimal, true);

            this.ConfigurationShared_RefreshChilds(null, configMapped.RootSectionGroup.SectionGroupName, configMapped);
        }

        private void ConfigurationShared_RefreshChilds(ConfigurationSectionGroup parentGroup, string sectionGroupName, System.Configuration.Configuration configMapped)
        {
            ConfigurationSectionGroup group = null;

            if (parentGroup == null)
            {
                group = configMapped.RootSectionGroup;
            }
            else
            {
                group = parentGroup.SectionGroups[sectionGroupName];
            }

            if (group != null)
            {
                if (group.Sections != null)
                {

                    foreach (var item in group.Sections.Keys)
                    {
                        string sectionName = string.Format("{0}{1}{2}",
                                                            group.SectionGroupName,
                                                            string.IsNullOrEmpty(group.SectionGroupName) ? string.Empty : "/",
                                                            group.Sections[(string)item].SectionInformation.Name);

                        if (configMapped.GetSection(sectionName).ElementInformation.IsPresent)
                        {
                            ConfigurationManager.RefreshSection(sectionName);
                        }
                        else
                        {
                            //Console.Write(sectionName);
                        }
                    }
                }

                foreach (var item in group.SectionGroups.Keys)
                {
                    ConfigurationShared_RefreshChilds(group, group.SectionGroups[(string)item].Name, configMapped);
                }
            }
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
