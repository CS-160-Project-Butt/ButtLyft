using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AASC.Partner.API.Utilities
{
    public class EmailServer
    {
        private string _domain;

        private string _account;

        private string _password;

        private string _adminEmail;

        private string _host;

        public EmailServer() {
            _domain = ConfigurationManager.AppSettings["emailService:Domain"];
            _account = ConfigurationManager.AppSettings["emailService:Account"];
            _password = ConfigurationManager.AppSettings["emailService:Password"];
            _adminEmail = ConfigurationManager.AppSettings["emailService:Email"];
            _host = ConfigurationManager.AppSettings["emailService:Host"];
        }

        //public EmailServer(string domain, string account, string password, string adminEmail, string host)
        //{
        //    _domain = domain;
        //    _account = account;
        //    _password = password;
        //    _adminEmail = adminEmail;
        //    _host = host;
        //}

        public async Task Send(string subject, string body, List<string> tos)
        {
            SmtpClient client = new SmtpClient();

            NetworkCredential credential = new NetworkCredential(_account, _password, _domain);

            client.Host = _host;
            client.UseDefaultCredentials = false;
            client.Credentials = credential;

            MailMessage email = new MailMessage();
            email.Subject = subject;
            email.IsBodyHtml = true;
            email.Body = body;
            email.From = new MailAddress(_adminEmail);
            foreach (var t in tos)
            {
                email.To.Add(new MailAddress(t));
            }
            await client.SendMailAsync(email);
        }
    }

}