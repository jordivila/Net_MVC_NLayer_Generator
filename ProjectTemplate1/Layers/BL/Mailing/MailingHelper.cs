using System;
using System.Net.Mail;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Configuration.ConfigSections.DomainInfo;
using $customNamespace$.Models.Configuration.ConfigSections.Mailing;
using $customNamespace$.Models.SmtpModels;
using $customNamespace$.Models.Unity;
using $customNamespace$.Models.Configuration;

namespace $customNamespace$.BL.Mailing
{
    public static class MailingHelper
    {
        static MailingHelper()
        {
            _SmtpClient = DependencyFactory.Resolve<ISmtpClient>();
            _MailingConfig = ApplicationConfiguration.MailingSettingsSection;
            _DomainConfig = ApplicationConfiguration.DomainInfoSettingsSection;
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