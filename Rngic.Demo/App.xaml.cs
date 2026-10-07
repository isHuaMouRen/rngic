using Rngic.Demo.Windows;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Rngic.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var win = new WindowMain();
            this.MainWindow = win;
            win.Show();
        }
    }

}
