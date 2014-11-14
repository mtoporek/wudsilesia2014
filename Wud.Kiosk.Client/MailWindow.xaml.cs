using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public MailWindow(string currentPicture, IMailService mailService, string fileName)
        {
            DataContext = this;
            this.currentPicture = currentPicture;
            this.mailService = mailService;

            InitializeComponent();

            imgPreview.Source = new BitmapImage(new Uri(fileName));

            MailList = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> MailList { get; set; }

        public bool IsMailValid { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SendMail(object sender, RoutedEventArgs e)
        {
            var mail = new Mail
                           {
                               Subject = "WUD Silesia 2015",
                               Body = "Witaj,",
                               MailsTo = this.MailList.ToList(),
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

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            IsMailValid = ValidateMail(txtMailTo.Text) && this.MailList.Count < 5;
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
            MailList.Add(txtMailTo.Text.Trim());
            txtMailTo.Clear();

            OnPropertyChanged("MailList");
        }

        private void RemoveMail(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
