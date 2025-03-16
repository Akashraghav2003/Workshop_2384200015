using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Net;
using System.Net.Mail;

namespace RepositoryLayer.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailModel emailModel)
        {
            try
            {
                using var client = new SmtpClient(_config["SMTP:Host"], int.Parse(_config["SMTP:Port"]))
                {
                    Credentials = new NetworkCredential(_config["SMTP:Username"], _config["SMTP:Password"]),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["SMTP:Username"]),
                    Subject = emailModel.Subject,
                    Body =emailModel.Body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(new MailAddress(emailModel.To));

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email: {ex.Message}", ex);
            }
        }
    }
}
