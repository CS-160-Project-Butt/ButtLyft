using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AASC.Partner.API.Utilities
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await SendViaExchangeServerAsync(message);            
        }

        private async Task SendViaExchangeServerAsync(IdentityMessage message)
        {
            SmtpClient client = new SmtpClient();
            var domain = ConfigurationManager.AppSettings["emailService:Domain"];
            var account = ConfigurationManager.AppSettings["emailService:Account"];
            var password = ConfigurationManager.AppSettings["emailService:Password"];
            var adminEmail = ConfigurationManager.AppSettings["emailService:Email"];
            var host = ConfigurationManager.AppSettings["emailService:Host"];

            NetworkCredential credential = new NetworkCredential(account, password, domain);
            client.Host = host;
            client.UseDefaultCredentials = false;
            client.Credentials = credential;

            MailMessage email = new MailMessage();
            email.Subject = message.Subject;
            email.Body = message.Body;
            email.IsBodyHtml = true;
            email.From = new MailAddress(adminEmail);
            email.To.Add(new MailAddress(message.Destination));
            await client.SendMailAsync(email);
        }
    }
}