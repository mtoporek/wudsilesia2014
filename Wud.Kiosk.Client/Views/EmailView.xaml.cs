using System;
using System.Windows;

namespace Wud.Kiosk.Client.Views
{
    public partial class EmailView
    {
        public EmailView()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            TxtMailTo.Focus();
            base.OnActivated(e);
        }
    }
}
