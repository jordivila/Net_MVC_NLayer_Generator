using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Logging;
using $customNamespace$.Models.Unity;
using System;
using System.Diagnostics;
using System.ServiceModel;


namespace $customNamespace$.Models.Host
{
    public class HostInitializer
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


            //foreach (var item in host.BaseAddresses)
            //{
            //    Console.WriteLine("Service listening at {0}", item.AbsoluteUri);
            //}
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
}
