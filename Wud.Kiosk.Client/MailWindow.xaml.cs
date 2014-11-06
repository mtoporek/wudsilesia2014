using System.Collections.Generic;
using System.Windows;

using Wud.Kiosk.Socials.Mail;

namespace Wud.Kiosk.Client
{
    public partial class MailWindow
    {
        private readonly string currentPicture;
        private readonly IMailService mailService;

        public MailWindow(string currentPicture, IMailService mailService)
        {
            this.currentPicture = currentPicture;
            this.mailService = mailService;

            InitializeComponent();
        }

        private void SendMail(object sender, RoutedEventArgs e)
        {
            // todo: add validatoin
            string mailTo = txtMailTo.Text;

            var mail = new Mail
                           {
                               Subject = "WUD Silesia 2015",
                               Body = "Witaj,",
                               MailTo = mailTo,
                               Attachments = new List<string> { this.currentPicture }
                           };

            this.mailService.SendMail(mail);
        }
    }
}
