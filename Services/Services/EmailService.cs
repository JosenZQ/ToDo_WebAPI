using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Services.Interfaces;

namespace Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration gConfig;

        public EmailService(IConfiguration pConfig)
        {
            gConfig = pConfig;
        }

        public void SendEmail(string pReceiver, string pSubject, string pMessage)
        {
            try
            {
                var lEmail = new MimeMessage
                {
                    From = { MailboxAddress.Parse(gConfig.GetSection("Email:Username").Value) },
                    To = { MailboxAddress.Parse(pReceiver) },
                    Subject = pSubject,
                    Body = new TextPart(TextFormat.Html) { Text = pMessage }
                };

                using var lSmtpClient = new SmtpClient();
                lSmtpClient.Connect(
                    gConfig.GetSection("Email:Host").Value,
                    Convert.ToInt32(gConfig.GetSection("Email:Port").Value),
                    SecureSocketOptions.StartTls);

                lSmtpClient.Authenticate(
                    gConfig.GetSection("Email:Username").Value,
                    gConfig.GetSection("Email:Password").Value);

                lSmtpClient.Send(lEmail);
                lSmtpClient.Disconnect(true);
            }
            catch(SslHandshakeException lEx) 
            {
                throw lEx;
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }
    }
}
