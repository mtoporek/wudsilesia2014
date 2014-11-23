using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Wud.Kiosk.Socials.FlickrGallery;
using Wud.Kiosk.Socials.Mail;

namespace Wud.Kiosk.Client.ViewModels
{
    public class ConfigurationViewModel : Screen
    {
        private readonly IFlickrService flickrService;

        private readonly IMailService mailService;

        public ConfigurationViewModel(IFlickrService flickrService, IMailService mailService)
        {
            this.flickrService = flickrService;
            this.mailService = mailService;
        }

        public void StartAuthentication()
        {
            this.flickrService.Authenticate();
        }

        public void CompleteAuthentication(string code)
        {
            this.flickrService.CompleteAuthentication(code);
        }

        public void Close()
        {
            TryClose();
        }

        public void SendTestMail(string mailTo)
        {
            var mail = new Mail { Subject = "Test mail", Body = "Test", MailsTo = new List<string> { mailTo } };
            var task = new Task(
                () => this.mailService.SendMail(mail));
            task.Start();
        }

        public void SaveMailConfiguration(string mailFrom, string smtpServer, string password)
        {
            this.mailService.Configure(mailFrom, 587, password, true, smtpServer);
        }
    }
}
