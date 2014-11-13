using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Wud.Kiosk.Socials.Mail;

namespace Wud.Kiosk.Client
{
    public partial class MailWindow : INotifyPropertyChanged
    {
        private const string Pattern = @"[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\.[a-zA-Z]{2,4}";
        private readonly string currentPicture;
        private readonly IMailService mailService;
        private readonly string fileName;

        private ObservableCollection<string> mailList;

        public ObservableCollection<string> MailList
        {
            get
            {
                return this.mailList;
            }
            set
            {
                this.mailList = value;
            }
        }

        public MailWindow(string currentPicture, IMailService mailService, string fileName)
        {
            DataContext = this;
            this.currentPicture = currentPicture;
            this.mailService = mailService;

            InitializeComponent();

            imgPreview.Source = new BitmapImage(new Uri(fileName));

            this.mailList = new ObservableCollection<string>();
        }

        public bool IsMailValid { get; set; }

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

            var task = new Task(() => this.mailService.SendMail(mail));
            task.Start();

            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            IsMailValid = ValidateMail(txtMailTo.Text) && this.mailList.Count < 5;
            OnPropertyChanged("IsMailValid");
        }

        private bool ValidateMail(string input)
        {
            
            var rgx = new Regex(Pattern);
            Match match = rgx.Match(input);
            return match.Success;
        }

        private void AddMail(object sender, RoutedEventArgs e)
        {
            this.mailList.Add(txtMailTo.Text);
            txtMailTo.Clear();

            OnPropertyChanged("MailList");
        }
    }
}
