using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Caliburn.Micro;

using Wud.Kiosk.Camera;
using Wud.Kiosk.Socials.FlickrGallery;
using Wud.Kiosk.Socials.Mail;

namespace Wud.Kiosk.Client.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private readonly BackgroundWorker worker;
        private readonly PictureProvider pictureProvider;
        private readonly IFlickrService flickrService;
        private readonly IMailService mailService;

        private readonly IWindowManager windowManager;
        private readonly string pictureDirectory;

        private IList<string> fileNames;
        private string currentPicture;

        public ShellViewModel()
        {
            this.worker = new BackgroundWorker { WorkerSupportsCancellation = true };
            this.worker.DoWork += WorkerDoWork;

            this.pictureProvider = new PictureProvider();
            this.pictureDirectory = ConfigurationManager.AppSettings["PicturesDirectory"];
            this.flickrService = new FlickrService();
            this.windowManager = new WindowManager();

            this.mailService = new MailService();

            if (!this.worker.IsBusy)
            {
                this.worker.RunWorkerAsync(DispatcherSynchronizationContext.Current);
            }
        }

        public byte[] Picture { get; set; }

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            this.fileNames = this.pictureProvider.GetFileNames(this.pictureDirectory);
            this.currentPicture = this.fileNames.LastOrDefault();

            var context = e.Argument as DispatcherSynchronizationContext;
            context.Send(UpdatePicture, this.currentPicture);

            while (true)
            {
                int currentCount = this.pictureProvider.GetFileNames(this.pictureDirectory).Count();

                if (currentCount != fileNames.Count())
                {
                    this.fileNames = this.pictureProvider.GetFileNames(this.pictureDirectory);
                    this.currentPicture = this.fileNames.LastOrDefault();
                    this.flickrService.Upload(null, currentPicture, "WUD Test", "Test", "WUD");

                    context.Send(UpdatePicture, this.currentPicture);
                }

                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
        }

        private void UpdatePicture(object parameter)
        {
            var fileName = parameter as string;

            if (fileName != null)
            {
                Picture = File.ReadAllBytes(fileName);
                NotifyOfPropertyChange("Picture");
            }
        }

        public void NextPicture()
        {
            int id = this.fileNames.IndexOf(this.currentPicture);

            if (id < this.fileNames.Count - 1)
            {
                string fileName = fileNames[++id];
                this.currentPicture = fileName;
                UpdatePicture(fileName);
            }
        }

        public void PreviousPicture()
        {
            int id = this.fileNames.IndexOf(this.currentPicture);

            if (id > 0)
            {
                string fileName = fileNames[--id];
                this.currentPicture = fileName;
                UpdatePicture(fileName);
            }
        }

        public void OpenMailWindow()
        {
            var emailViewModel = new EmailViewModel(this.currentPicture, this.mailService);
            this.windowManager.ShowWindow(emailViewModel);
        }

        private void ConfigurationClick(object sender, MouseButtonEventArgs e)
        {
            var configuration = new ConfigurationWindow(this.flickrService, this.mailService);
            configuration.ShowDialog();
        }
    }
}
