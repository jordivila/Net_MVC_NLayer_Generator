using System;
using System.Net.Mail;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Configuration.ConfigSections.DomainInfo;
using $customNamespace$.Models.Configuration.ConfigSections.Mailing;
using $customNamespace$.Models.SmtpModels;
using $customNamespace$.Models.Unity;

namespace $safeprojectname$.Mailing
{
    public static class MailingHelper
    {
        static MailingHelper()
        {
            using (DependencyFactory dependencyFactory = new DependencyFactory())
            {
                _SmtpClient = dependencyFactory.Unity.Resolve<ISmtpClient>();
            }
            _MailingConfig = $customNamespace$.Models.Configuration.ApplicationConfiguration.MailingSettingsSection; //new MailingConfiguration();  // DependencyFactory.Unity.Resolve<IMailingConfiguration>(); 
            _DomainConfig = $customNamespace$.Models.Configuration.ApplicationConfiguration.DomainInfoSettingsSection;  // new DomainInfoConfiguration(); // DependencyFactory.Unity.Resolve<IDomainInfoConfiguration>();
        }

        private static ISmtpClient _SmtpClient;
        public static ISmtpClient SmtpClient
        {
            get { return _SmtpClient; }
        }

        private static IMailingConfiguration _MailingConfig;
        public static IMailingConfiguration MailingConfig
        {
            get { return _MailingConfig; }
        }

        private static IDomainInfoConfiguration _DomainConfig;
        public static IDomainInfoConfiguration DomainConfig
        {
            get { return _DomainConfig; }
        }

        public static void Send(Func<MailMessage> mailMessage)
        {
            _SmtpClient.Send(mailMessage());
            //_SmtpClient.Dispose();
        }

        
    }
}