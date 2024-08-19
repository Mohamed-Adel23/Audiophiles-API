using Audiophiles_API.IServices;
using System.Net.Mail;
using System.Net;

namespace Audiophiles_API.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fMail = "mohamed.adel.fci@outlook.com";
            var fPassword = "Mo23.@aic#4";

            var msg = new MailMessage();
            msg.From = new MailAddress(fMail);
            msg.Subject = subject;
            msg.To.Add(email);
            msg.Body = $"<html><body>{htmlMessage}</body></html>";
            msg.IsBodyHtml = true;

            var smtp = new SmtpClient("smtp-mail.outlook.com")
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fMail, fPassword),
                Port = 587
            };

            smtp.Send(msg);
        }
    }
}
