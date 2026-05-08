using Microsoft.Extensions.DependencyInjection;

namespace AppInvoice
{
    // Entry point of this app and load AppShell.xaml as the main window
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}