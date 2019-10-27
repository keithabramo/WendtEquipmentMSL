using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;

namespace WendtEquipmentTracking.Common
{

    public class Mail
    {
        private static ILog logger = LogManager.GetLogger("File");

        //public static bool Send(string to, string subject, string body)
        //{
        //    return send(to, subject, body, null, string.Empty);
        //}

        public static bool Send(string to, string subject, string body, byte [] attachment, string attachmentName, string contentType)
        {
            return send(to, subject, body, attachment, attachmentName, contentType);
        }

        private static bool send(string to, string subject, string body, byte[] attachment, string attachmentName, string contentType)
        {
            var success = true;

            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.To.Add(new MailAddress(to));
            message.Bcc.Add(new MailAddress("Keith.Abramo@gmail.com"));
            message.Subject = subject;
            message.Body = string.Format(body);

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                SmtpClient smtp = new SmtpClient();


                if (attachment != null)
                {
                    using (MemoryStream stream = new MemoryStream(attachment))
                    {
                        Attachment emailAttachment = new Attachment(stream, new ContentType(contentType));
                        emailAttachment.Name = attachmentName;
                        emailAttachment.ContentId = attachmentName;

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
