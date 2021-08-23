using IdentityCore.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Service
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SMTPConfigModel> _smtpConfig;
        private const string templatePath = @"EmailTemplate/{0}.html";

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig;
        }

        public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{username}}, This is test email from HIMU identity", userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("TestEmail"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        public async Task SendEmailConfirmation(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{username}}, Confirm your email id", userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirmation"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        public async Task SendEmailForgotPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{username}}, Confirm your email id", userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mailMessage = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.Value.SenderAddress, _smtpConfig.Value.SenderDisplayName),
                IsBodyHtml = _smtpConfig.Value.IsBodyHTML
            };

            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mailMessage.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.Value.UserName, _smtpConfig.Value.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Value.Host,
                Port = _smtpConfig.Value.Port,
                EnableSsl = _smtpConfig.Value.EnableSSL,
                UseDefaultCredentials = _smtpConfig.Value.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mailMessage.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mailMessage);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!String.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var item in keyValuePairs)
                {
                    if (text.Contains(item.Key))
                    {
                        text = text.Replace(item.Key, item.Value);
                    }
                }
            }

            return text;
        }
    }
}
