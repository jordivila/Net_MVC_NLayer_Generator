using System;
using System.ServiceModel;
using $customNamespace$.WCF.ServicesLibrary.AspNetApplicationServices;
using $customNamespace$.WCF.ServicesLibrary.AspNetApplicationServices.Admin;
using $customNamespace$.WCF.ServicesLibrary.LoggingServices;
using $customNamespace$.WCF.ServicesLibrary.SyndicationServices;

namespace $safeprojectname$
{
    class Program
    {
        static void Main()
        {
            Func<Type, ServiceHost> createHost = delegate(Type serviceType)
            {
                ServiceHost host = new ServiceHost(serviceType);
                host.Open();
                foreach (var item in host.BaseAddresses)
                {
                    Console.WriteLine("Service listening at {0}.", item.AbsoluteUri);
                }
                host.Closed += new EventHandler(host_Closed);
                host.Closing += new EventHandler(host_Closing);
                host.Faulted += new EventHandler(host_Faulted);
                host.Opened += new EventHandler(host_Opened);
                host.Opening += new EventHandler(host_Opening);
                host.UnknownMessageReceived += new EventHandler<UnknownMessageReceivedEventArgs>(host_UnknownMessageReceived);
                return host;
            };

            ServiceHost svcAuthentication = createHost(typeof(AuthenticationService));
            ServiceHost svcMembership = createHost(typeof(MembershipServices));
            ServiceHost svcRolesManager = createHost(typeof(RoleServiceAdmin));
            ServiceHost svcProfiles = createHost(typeof(ProfileService));
            ServiceHost svcRoles = createHost(typeof(RoleService));
            ServiceHost svcLogging = createHost(typeof(LoggingService));
            ServiceHost svcSyndication = createHost(typeof(SyndicationService));

            Console.WriteLine("Press <ENTER> to terminate services.");
            Console.ReadLine();

            svcAuthentication.Close();
            svcRoles.Close();
            svcMembership.Close();
            svcProfiles.Close();
            svcRolesManager.Close();
            svcLogging.Close();
            svcSyndication.Close();
        }

        static void host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void host_Opening(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("{0} opening", ((ServiceHost)sender).BaseAddresses));
        }

        static void host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("{0} opened", ((ServiceHost)sender).BaseAddresses));
        }

        static void host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("{0} faulted", ((ServiceHost)sender).BaseAddresses));
        }

        static void host_Closing(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("{0} closing", ((ServiceHost)sender).BaseAddresses));
        }

        static void host_Closed(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("{0} closed", ((ServiceHost)sender).BaseAddresses));
        }



    }
}
