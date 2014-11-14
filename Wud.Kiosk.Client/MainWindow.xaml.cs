using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Wud.Kiosk.Camera;
using Wud.Kiosk.Socials.FlickrGallery;
using Wud.Kiosk.Socials.Mail;

namespace Wud.Kiosk.Client
{
    public partial class MainWindow
    {
        private readonly BackgroundWorker worker;
        private readonly PictureProvider pictureProvider;
        private readonly IFlickrService flickrService;
        private readonly IMailService mailService;
        private readonly string pictureDirectory;

        private IList<string> fileNames;
        private string currentPicture;


        public MainWindow()
        {
            InitializeComponent();

            this.worker = new BackgroundWorker { WorkerSupportsCancellation = true };
            this.worker.DoWork += WorkerDoWork;

            this.pictureProvider = new PictureProvider();
            this.pictureDirectory = ConfigurationManager.AppSettings["PicturesDirectory"];
            this.flickrService = new FlickrService();

            this.mailService = new MailService();

            if (!this.worker.IsBusy)
            {
                this.worker.RunWorkerAsync(DispatcherSynchronizationContext.Current);
            }
        }

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
            string fileName = parameter as string;

            if (fileName != null)
            {
                img.Source = new BitmapImage(new Uri(fileName));
            }
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            int id = this.fileNames.IndexOf(this.currentPicture);

            if (id < this.fileNames.Count - 1)
            {
                string fileName = fileNames[++id];
                this.currentPicture = fileName;
                UpdatePicture(fileName);
            }
        }

        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            int id = this.fileNames.IndexOf(this.currentPicture);

            if (id > 0)
            {
                string fileName = fileNames[--id];
                this.currentPicture = fileName;
                UpdatePicture(fileName);
            }
        }

        private void ConfigurationClick(object sender, MouseButtonEventArgs e)
        {
            var configuration = new ConfigurationWindow(this.flickrService, this.mailService);
            configuration.ShowDialog();
        }

        private void MailClick(object sender, RoutedEventArgs e)
        {
            var mailWindow = new MailWindow(this.currentPicture, this.mailService, this.currentPicture);
            mailWindow.ShowDialog();
        }
    }
}
