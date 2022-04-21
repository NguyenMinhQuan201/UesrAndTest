using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAdmin.Models;

namespace UserAdmin.Services
{
    public interface ISendMailService
    {
        Task SendMail(MailContent mailContent);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
    public class EmailSenderServices : ISendMailService
    {
        private readonly MailSettings mailSettings;
        private readonly MailUtils mail;

        private readonly ILogger<EmailSenderServices> logger;


        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public EmailSenderServices(IOptions<MailSettings> _mailSettings, ILogger<EmailSenderServices> _logger, MailUtils _mail)
        {
            mailSettings = _mailSettings.Value;
            logger = _logger;
            mail = _mail;
            logger.LogInformation("Create SendMailService");
        }

        // Gửi email, theo nội dung trong mailContent
        public async Task SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                logger.LogInformation("Lỗi gửi mail");
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            logger.LogInformation("send mail to " + mailContent.To);

        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendMail(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }
    }
}
