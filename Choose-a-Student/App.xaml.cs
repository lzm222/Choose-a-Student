using System.Windows;
using Choose_a_Student.Services;

namespace Choose_a_Student
{
    /// <summary>
    /// 应用程序入口：在启动时装配服务并显示主窗口。
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 启动时创建配置服务与主窗口，并把主窗口设为应用主窗口。
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configService = new ConfigService();
            MainWindow = new MainWindow(configService);
            MainWindow.Show();
        }
    }
}
