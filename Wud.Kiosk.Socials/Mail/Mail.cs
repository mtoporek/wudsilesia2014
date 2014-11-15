using System.Collections.Generic;

namespace Wud.Kiosk.Socials.Mail
{
    public class Mail
    {
        public IList<string> MailsTo { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public IList<string> Attachments { get; set; }
    }
}
