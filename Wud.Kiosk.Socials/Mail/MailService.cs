using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wud.Kiosk.Socials.Mail
{
    public class MailService : IMailService
    {
        private string smtpServer;
        private int portNumber;
        private bool enableSSL;

        private string mailFrom;
        // todo: hash!
        private string password;

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
                mailMessage.To.Add(mail.MailTo);
                mailMessage.Subject = mail.Subject;
                mailMessage.Body = mail.Body;
                mailMessage.IsBodyHtml = false;
                // Can set to false, if you are sending pure text.

                //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

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
