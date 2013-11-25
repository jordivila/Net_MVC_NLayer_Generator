using System;
using System.ServiceModel;
using System.Reflection;
using System.Diagnostics;

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
            ServiceHost svcAuthentication = Host_Create(typeof(AuthenticationService));
            ServiceHost svcMembership = Host_Create(typeof(MembershipServices));
            ServiceHost svcRolesManager = Host_Create(typeof(RoleServiceAdmin));
            ServiceHost svcProfiles = Host_Create(typeof(ProfileService));
            ServiceHost svcRoles = Host_Create(typeof(RoleService));
            ServiceHost svcLogging = Host_Create(typeof(LoggingService));
            ServiceHost svcSyndication = Host_Create(typeof(SyndicationService));

            Console.WriteLine("Press <ENTER> to stop services...");
            Console.ReadLine();

            svcAuthentication.Close();
            svcRoles.Close();
            svcMembership.Close();
            svcProfiles.Close();
            svcRolesManager.Close();
            svcLogging.Close();
            svcSyndication.Close();
        }

        static MethodBase GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            return sf.GetMethod();
        }

        static ServiceHost Host_Create(Type serviceType)
        {
            ServiceHost host = new ServiceHost(serviceType);
            host.Open();
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

        static void Host_EventTrace(string eventName, ServiceHost service)
        {
            Console.WriteLine(string.Format("{0} {1}", eventName, service.BaseAddresses[0]));
        }

        static void Host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void Host_Opening(object sender, EventArgs e)
        {
            Host_EventTrace(GetCurrentMethod().Name, (ServiceHost)sender);
        }

        static void Host_Opened(object sender, EventArgs e)
        {
            Host_EventTrace(GetCurrentMethod().Name, (ServiceHost)sender);
        }

        static void Host_Faulted(object sender, EventArgs e)
        {
            Host_EventTrace(GetCurrentMethod().Name, (ServiceHost)sender);
        }

        static void Host_Closing(object sender, EventArgs e)
        {
            Host_EventTrace(GetCurrentMethod().Name, (ServiceHost)sender);
        }

        static void Host_Closed(object sender, EventArgs e)
        {
            Host_EventTrace(GetCurrentMethod().Name, (ServiceHost)sender);
        }
    }
}
