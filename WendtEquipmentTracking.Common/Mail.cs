using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using WendtEquipmentTracking.Common.DTO;

namespace WendtEquipmentTracking.Common
{

    public class Mail
    {
        private static ILog logger = LogManager.GetLogger("File");

        public static bool Send(EmailDTO emailDTO)
        {
            var success = true;

            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.To.Add(new MailAddress(emailDTO.To));
            message.Bcc.Add(new MailAddress("Keith.Abramo@gmail.com"));
            message.Subject = emailDTO.Subject;
            message.Body = emailDTO.Body;

            if(!string.IsNullOrWhiteSpace(emailDTO.From))
            {
                message.From = new MailAddress(emailDTO.From);
            }

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                SmtpClient smtp = new SmtpClient();

                if (emailDTO.Attachment != null)
                {
                    using (MemoryStream stream = new MemoryStream(emailDTO.Attachment.File))
                    {
                        Attachment emailAttachment = new Attachment(stream, new ContentType(emailDTO.Attachment.ContentType));
                        emailAttachment.Name = emailDTO.Attachment.FileName;
                        emailAttachment.ContentId = emailDTO.Attachment.FileName;

                        message.Attachments.Add(emailAttachment);

                        smtp.Send(message);
                    }
                } else
                {
                    smtp.Send(message);
                }
            } catch (Exception ex)
            {
                success = false;
                logger.Error("Email Error " + ActiveDirectoryHelper.CurrentUserUsername() + ": ", ex);
            }

            return success;
        }

        private static bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}
