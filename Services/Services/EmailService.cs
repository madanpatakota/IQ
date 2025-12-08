using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Misard.IQs.Application.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var smtp = _config.GetSection("SmtpSettings");

            using var client = new SmtpClient
            {
                Host = smtp["Host"],
                Port = int.Parse(smtp["Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    smtp["Username"],
                    smtp["Password"]
                )
            };

            var mail = new MailMessage
            {
                From = new MailAddress(smtp["Username"], "Misard IQs"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await client.SendMailAsync(mail);
        }
    }
}
