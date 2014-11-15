using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Caliburn.Micro;

using Wud.Kiosk.Socials.Mail;

namespace Wud.Kiosk.Client.ViewModels
{
    public class EmailViewModel : Screen
    {
        private const string Pattern = @"[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\.[a-zA-Z]{2,4}";

        private readonly string currentPicture;
        private readonly IMailService mailService;

        public EmailViewModel(string currentPicture, IMailService mailService)
        {
            this.currentPicture = currentPicture;
            this.mailService = mailService;

            MailList = new ObservableCollection<string>();
        }

        public bool IsMailValid { get; set; }

        public string MailTo { get; set; }

        public ObservableCollection<string> MailList { get; set; }

        public byte[] Picture
        {
            get
            {
                return File.ReadAllBytes(this.currentPicture);
            }
        }

        public void AddMail()
        {
            if (ValidateMail(MailTo) && !MailList.Contains(MailTo) && MailList.Count < 5)
            {
                MailList.Add(MailTo.Trim());
                MailTo = string.Empty;

                NotifyOfPropertyChange("MailTo");
                NotifyOfPropertyChange("MailList");
            }
        }

        public void RemoveMail(string mail)
        {
            MailList.Remove(mail);
            NotifyOfPropertyChange("MailList");
        }

        private bool ValidateMail(string input)
        {
            var rgx = new Regex(Pattern);
            Match match = rgx.Match(input);
            return match.Success;
        }

        public void SendMail()
        {
            var mail = new Mail
                           {
                               Subject = "WUD Silesia 2015",
                               Body = "Witaj,",
                               MailsTo = MailList.ToList(),
                               Attachments = new List<string> { this.currentPicture }
                           };

            var task = new Task(() => this.mailService.SendMail(mail));
            task.Start();

            TryClose();
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}
