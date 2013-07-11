using System;
using System.Net.Mail;

namespace $safeprojectname$.SmtpModels
{
    public interface ISmtpClient: IDisposable
    {
        void Send(MailMessage mailMessage);
    }

	public class SmtpClientProxy : ISmtpClient
	{
		private readonly SmtpClient _smtpClient;

		public SmtpClientProxy()
		{
			_smtpClient = new SmtpClient();
		}

        public void Send(MailMessage mailMessage)
        {
            _smtpClient.Send(mailMessage);
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }
    }

    public class SmtpClientMock: ISmtpClient
    {
        public SmtpClientMock()
        {

        }

        public void Send(MailMessage mailMessage)
        {

        }

        public void Dispose()
        {
            
        }
    }

}