using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

using Wud.Kiosk.Socials.FlickrGallery;
using Wud.Kiosk.Socials.Mail;

namespace Wud.Kiosk.Client
{
    public partial class ConfigurationWindow
    {
        private readonly IFlickrService flickrService;
        private readonly IMailService mailService;

        public ConfigurationWindow(IFlickrService flickrService, IMailService mailService)
        {
            this.flickrService = flickrService;
            this.mailService = mailService;

            InitializeComponent();
        }

        private void StartAuthentication(object sender, RoutedEventArgs e)
        {
            this.flickrService.Authenticate();
        }

        private void CompleteAuthentication(object sender, RoutedEventArgs e)
        {
            this.flickrService.CompleteAuthentication(txtFlickrCode.Text);
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
           Close();
        }

        private void SendTestMail(object sender, RoutedEventArgs e)
        {
            var mail = new Mail { Subject = "Test mail", Body = "Test", MailsTo = new List<string> { txtMailTo.Text } };
            var task = new Task(
                () => this.mailService.SendMail(mail));
            task.Start();
        }

        private void SaveMailConfiguration(object sender, RoutedEventArgs e)
        {
            this.mailService.Configure(txtMailFrom.Text, 587, mailPassword.Password, true, txtSmtpServer.Text);
        }
    }
}
