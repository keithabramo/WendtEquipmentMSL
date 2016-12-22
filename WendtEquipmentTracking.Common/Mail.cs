using System.Net.Mail;
using System.Threading.Tasks;

namespace WendtEquipmentTracking.Common
{

    public class Mail
    {
        public static async Task Send(string to, string subject, string message)
        {
            await Send(to, "", subject, message);
        }

        public static async Task Send(string to, string cc, string subject, string message)
        {
            MailMessage Message = new MailMessage();
            Message.To.Add(new MailAddress(to));

            if (cc.Length > 0)
            {
                MailAddress CC = new MailAddress(cc);
                Message.CC.Add(CC);
            }
            Message.Subject = subject;
            Message.IsBodyHtml = true;
            Message.Body = string.Format(message);
            SmtpClient smtp = new SmtpClient();

            await smtp.SendMailAsync(Message);
        }

    }
}
