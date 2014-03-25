using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Logging;
using $customNamespace$.Models.Unity;
using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;
using System.Reflection;


namespace $customNamespace$.Models.Host
{
    public partial class HostInitializer
    {
        public HostInitializer()
        {
            this.LoadSharedConfiguration(@"Template.WCF.ServiceHostCommon.config");
        }
    }

    public partial class HostInitializer
    {
        public void Start_EnterpriseLibrary(IUnityContainer unityContainer)
        {
            DependencyFactory.SetUnityContainerProviderFactory(unityContainer);
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            LogWriterFactory logWriterFactory = new LogWriterFactory();
            Logger.SetLogWriter(logWriterFactory.Create());
        }

        public ServiceHost Start_ServiceHost(Type serviceType)
        {
            return this.Start_ServiceHost(serviceType, null);
        }
        public ServiceHost Start_ServiceHost(Type serviceType, Action<ServiceHost> beforeOpen)
        {
            ServiceHost host = new ServiceHost(serviceType);

            if (beforeOpen != null)
            {
                beforeOpen(host);
            }

            try
            {
                host.Open();
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


            foreach (var item in host.BaseAddresses)
            {
                Console.WriteLine("Service listening at {0}", item.AbsoluteUri);
            }

            host.Closed += new EventHandler(Host_Closed);
            host.Closing += new EventHandler(Host_Closing);
            host.Faulted += new EventHandler(Host_Faulted);
            host.Opened += new EventHandler(Host_Opened);
            host.Opening += new EventHandler(Host_Opening);
            host.UnknownMessageReceived += new EventHandler<UnknownMessageReceivedEventArgs>(Host_UnknownMessageReceived);
            return host;
        }

        private void Host_EventTrace(string eventName, ServiceHost service)
        {
            Console.WriteLine(string.Format("{0} {1}", eventName, service.BaseAddresses[0]));
        }
        private void Host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private void Host_Opening(object sender, EventArgs e)
        {
            this.Host_EventTrace(baseModel.GetCurrentMethod().Name, (ServiceHost)sender);
        }
        private void Host_Opened(object sender, EventArgs e)
        {
            this.Host_EventTrace(baseModel.GetCurrentMethod().Name, (ServiceHost)sender);
        }
        private void Host_Faulted(object sender, EventArgs e)
        {
            this.Host_EventTrace(baseModel.GetCurrentMethod().Name, (ServiceHost)sender);
        }
        private void Host_Closing(object sender, EventArgs e)
        {
            this.Host_EventTrace(baseModel.GetCurrentMethod().Name, (ServiceHost)sender);
        }
        private void Host_Closed(object sender, EventArgs e)
        {
            this.Host_EventTrace(baseModel.GetCurrentMethod().Name, (ServiceHost)sender);
        }
    }

    public partial class HostInitializer
    {
        public void LoadSharedConfiguration(string sharedFile)
        {
            // Load shared configuration file into memory
            System.Configuration.Configuration configMapped;
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = sharedFile;
            configMapped = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            // Merge application current configuration
            System.Configuration.Configuration configCurrent = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configMapped.SaveAs(configCurrent.FilePath, ConfigurationSaveMode.Minimal, true);

            // Refresh sections 
            Action<ConfigurationSectionGroup, string> refreshChilds = null;

            refreshChilds = delegate(ConfigurationSectionGroup parentGroup, string sectionGroupName)
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
                        refreshChilds(group, group.SectionGroups[(string)item].Name);
                    }
                }
            };


            refreshChilds(null, configMapped.RootSectionGroup.SectionGroupName);
        }
    }
}