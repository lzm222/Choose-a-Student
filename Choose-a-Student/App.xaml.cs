using System.Windows;
using Choose_a_Student.Services;

namespace Choose_a_Student
{
    /// <summary>
    /// 应用程序入口与组合根：创建服务与各窗口，订阅并转发它们的回调，驱动全部业务逻辑。
    /// </summary>
    public partial class App : Application
    {
        /// <summary>配置与名单的统一访问入口。</summary>
        private readonly ConfigService _configService = new();

        /// <summary>随机点名服务。</summary>
        private readonly PickerService _pickerService = new();

        /// <summary>主窗口。</summary>
        private MainWindow? _mainWindow;

        /// <summary>悬浮球窗口。</summary>
        private FloatingBallWindow? _floatingBall;

        /// <summary>结果窗口；按需创建。</summary>
        private ResultWindow? _resultWindow;

        /// <summary>
        /// 启动时创建并装配各窗口与服务，然后用配置初始化界面。
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _mainWindow = new MainWindow();
            _floatingBall = new FloatingBallWindow();

            _mainWindow.RosterFileSelected += OnRosterFileSelected;
            _mainWindow.ShowFloatingBallToggled += OnShowFloatingBallToggled;
            _floatingBall.PickRequested += OnPickRequested;

            _mainWindow.SetRosterInfo(_configService.RosterFilePath, _configService.Roster.Count);
            _mainWindow.SetShowFloatingBall(_configService.ShowFloatingBall);
            ApplyFloatingBallVisibility();

            MainWindow = _mainWindow;
            MainWindow.Show();
        }

        /// <summary>响应主窗口选定名单文件：加载名单并刷新界面，失败则提示。</summary>
        /// <param name="path">名单文件完整路径。</param>
        private void OnRosterFileSelected(string path)
        {
            if (_configService.TrySetRosterFile(path, out string? error))
            {
                _mainWindow?.SetRosterInfo(path, _configService.Roster.Count);
            }
            else
            {
                _mainWindow?.ShowRosterError(error ?? "未知错误");
            }
        }

        /// <summary>响应悬浮球开关变化：写入配置并按需显示或隐藏悬浮球。</summary>
        /// <param name="show">是否显示悬浮球。</param>
        private void OnShowFloatingBallToggled(bool show)
        {
            _configService.ShowFloatingBall = show;
            ApplyFloatingBallVisibility();
        }

        /// <summary>响应悬浮球点名请求：随机取一名并显示结果，名单为空时提示。</summary>
        private void OnPickRequested()
        {
            string? name = _pickerService.Pick(_configService.Roster);
            if (name is null)
            {
                _mainWindow?.ShowNoRosterMessage();
                return;
            }

            ShowResult(name);
        }

        /// <summary>按配置显示或隐藏悬浮球窗口。</summary>
        private void ApplyFloatingBallVisibility()
        {
            if (_floatingBall is null)
            {
                return;
            }

            if (_configService.ShowFloatingBall)
            {
                _floatingBall.Show();
                _floatingBall.Activate();
            }
            else
            {
                _floatingBall.Hide();
            }
        }

        /// <summary>把姓名显示到结果窗口，必要时重新创建该窗口。</summary>
        /// <param name="name">被点到的姓名。</param>
        private void ShowResult(string name)
        {
            if (_resultWindow is null || !_resultWindow.IsLoaded)
            {
                _resultWindow = new ResultWindow
                {
                    Owner = _mainWindow,
                };
                _resultWindow.Closed += (_, _) => _resultWindow = null;
            }

            _resultWindow.ShowName(name);
        }
    }
}
