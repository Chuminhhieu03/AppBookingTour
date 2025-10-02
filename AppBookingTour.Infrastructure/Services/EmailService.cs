using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using AppBookingTour.Application.IServices;
using AppBookingTour.Share.Configurations;

namespace AppBookingTour.Infrastructure.Services
{
    public class EmailService(IOptions<SmtpSettings> settings) : IEmailService
    {
        private readonly SmtpSettings _settings = settings.Value;

        public async Task SendEmailConfirmAsync(string email, string title, string htmlContent)
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.User, _settings.Pass),
                EnableSsl = _settings.EnableSsl
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.User, _settings.FromName),
                Subject = title,
                Body = htmlContent,
                IsBodyHtml = true
            };

            mail.To.Add(email);

            await client.SendMailAsync(mail);
        }
    }
}
