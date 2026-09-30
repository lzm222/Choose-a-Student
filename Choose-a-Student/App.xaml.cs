using System.Windows;
using Choose_a_Student.Services;

namespace Choose_a_Student
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configService = new ConfigService();
            MainWindow = new MainWindow(configService);
            MainWindow.Show();
        }
    }
}
