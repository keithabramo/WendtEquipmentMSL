using log4net;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace WendtEquipmentTracking.Common
{

    public class Mail
    {
        private static ILog logger = LogManager.GetLogger("File");

        public static void Send(string to, string subject, string body)
        {
            send(to, subject, body, null);
        }

        public static void Send(string to, string subject, string body, byte [] attachment)
        {
            send(to, subject, body, attachment);
        }

        private static void send(string to, string subject, string body, byte[] attachment)
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.To.Add(new MailAddress(to));
            message.Bcc.Add(new MailAddress("Keith.Abramo@gmail.com"));
            message.Subject = subject;
            message.Body = string.Format(body);

            if (attachment != null)
            {
                using (MemoryStream stream = new MemoryStream(attachment))
                {
                    Attachment emailAttachment = new Attachment(stream, new ContentType("text/csv"));
                    emailAttachment.Name = "test.csv";

                    message.Attachments.Add(emailAttachment);
                }
            }
            
            try
            {
                SmtpClient smtp = new SmtpClient();

                smtp.Send(message);

            } catch (Exception ex)
            {
                logger.Error("Email Error " + ActiveDirectoryHelper.CurrentUserUsername() + ": ", ex);
            }
        }

    }
}
