using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wud.Kiosk.Socials.Mail
{
    public class Mail
    {
        public string MailTo { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public IList<string> Attachments { get; set; }
 

    }
}
