using System.Net;
using System.Net.Mail;

namespace Wud.Kiosk.Socials.Mail
{
    public class MailService : IMailService
    {
        private string smtpServer;
        private string mailFrom;
        private string password;
        private int portNumber;
        private bool enableSSL;

        public MailService()
        {
            this.smtpServer = "smtp.mail.yahoo.com";
            this.portNumber = 587;
            this.enableSSL = true;
            this.mailFrom = "wudsilesia@yahoo.com";
            this.password = "123WUDSilesia";
        }

        public void Configure(string mailFrom, int portNumber, string password, bool enableSSL, string smtpServer)
        {
            this.mailFrom = mailFrom;
            this.portNumber = portNumber;
            this.password = password;
            this.enableSSL = enableSSL;
            this.smtpServer = smtpServer;
        }

        public void SendMail(Mail mail)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(this.mailFrom);

                foreach (var mailTo in mail.MailsTo)
                {
                    mailMessage.To.Add(mailTo);
                }

                mailMessage.Subject = mail.Subject;
                mailMessage.Body = mail.Body;
                mailMessage.IsBodyHtml = false;

                foreach (string attachment in mail.Attachments)
                {
                    mailMessage.Attachments.Add(new Attachment(attachment));
                }

                using (var smtp = new SmtpClient(this.smtpServer, this.portNumber))
                {
                    smtp.Credentials = new NetworkCredential(this.mailFrom, this.password);
                    smtp.EnableSsl = this.enableSSL;
                    smtp.Send(mailMessage);
                }
            }
        }
    }
}
