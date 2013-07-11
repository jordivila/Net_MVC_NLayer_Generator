using System.ServiceModel;
using System.ServiceProcess;
using $customNamespace$.WCF.ServicesLibrary.AspNetApplicationServices.Admin;

namespace $safeprojectname$
{
    partial class HostService : ServiceBase
    {
        internal static ServiceHost myServiceHost = null;

        public HostService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (myServiceHost != null)
            {
                myServiceHost.Close();
            }

            myServiceHost = new ServiceHost(typeof(MembershipServices));
            myServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (myServiceHost != null)
            {
                myServiceHost.Close();
                myServiceHost = null;
            }
        }
    }
}
