using Unit_Testing_xUnit_ASP_NET_Core.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Repositories
{
    public class MailRepository
    {
        private readonly MailSettings _mailSettings;
        public MailRepository(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                /*var time24 = DateTime.Now.ToString("HH:mm:ss");

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_mailSettings.Host, _mailSettings.Port);
                smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                NetworkCredential nc = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                smtp.Credentials = nc;
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_mailSettings.Mail, "Forgot Password");
                mailMessage.To.Add(new MailAddress(mailRequest.ToEmail));
                mailMessage.Subject = mailRequest.Subject;
                mailMessage.IsBodyHtml = false;
                mailMessage.Body = mailRequest.Body;
                smtp.Send(mailMessage);
                return 1;*/
            }
            catch (Exception e)
            {
                //return 0;
            }
        }
    }
}
