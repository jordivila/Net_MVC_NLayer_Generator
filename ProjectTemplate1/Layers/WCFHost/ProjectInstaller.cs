using System;
using System.ComponentModel;
using System.Configuration.Install;


namespace $safeprojectname$
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_BeforeRollback(object sender, InstallEventArgs e)
        {
            foreach (var item in e.SavedState)
            {
                Console.WriteLine(item);
            }
        }

        private void serviceInstaller1_BeforeUninstall(object sender, InstallEventArgs e)
        {
            foreach (var item in e.SavedState)
            {
                Console.WriteLine(item);
            }
        }
    }
}
