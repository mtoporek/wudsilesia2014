using System.Windows;

using Caliburn.Micro;

using Wud.Kiosk.Client.ViewModels;

namespace Wud.Kiosk.Client
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
