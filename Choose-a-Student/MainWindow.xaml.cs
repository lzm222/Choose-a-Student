using System.Windows;
using Microsoft.Win32;
using Choose_a_Student.Services;

namespace Choose_a_Student
{
    /// <summary>
    /// 主窗口：选择名单、控制悬浮球。
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>配置与名单的统一访问入口。</summary>
        private readonly ConfigService _configService;

        /// <summary>随机点名服务。</summary>
        private readonly PickerService _pickerService = new();

        /// <summary>悬浮球窗口；未显示时为 null。</summary>
        private FloatingBallWindow? _floatingBall;

        /// <summary>结果窗口；未创建时为 null。</summary>
        private ResultWindow? _resultWindow;

        /// <summary>初始化期间抑制控件事件回写配置。</summary>
        private bool _restoring = false;

        /// <summary>构造函数：记录配置服务并初始化界面。</summary>
        public MainWindow(ConfigService configService)
        {
            _configService = configService;

            InitializeComponent();
            RestoreState();
        }

        /// <summary>启动时按配置恢复界面：刷新显示，并在需要时显示悬浮球。</summary>
        private void RestoreState()
        {
            RefreshFromConfig();

            if (_configService.ShowFloatingBall)
            {
                SetFloatingBallVisible(true);
            }
        }

        /// <summary>「选择名单文件」按钮点击：弹出文件对话框，成功后加载名单并刷新界面。</summary>
        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "选择名单文件",
                Filter = "文本文件|*.txt|所有文件|*.*",
            };

            if (dialog.ShowDialog(this) == true)
            {
                if (_configService.TrySetRosterFile(dialog.FileName, out string? error))
                {
                    RefreshFromConfig();
                }
                else
                {
                    MessageBox.Show(this, "读取名单失败：" + error, "错误",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>读取当前配置并刷新界面显示。</summary>
        private void RefreshFromConfig()
        {
            _restoring = true;

            string path = _configService.RosterFilePath;
            FilePathText.Text = string.IsNullOrEmpty(path) ? "名单文件：未选择" : "名单文件：" + path;
            CountText.Text = "人数：" + _configService.Roster.Count;
            ShowBallCheckBox.IsChecked = _configService.ShowFloatingBall;

            _restoring = false;
        }

        /// <summary>悬浮球开关变化：显示或隐藏悬浮球，并把开关状态写入配置。</summary>
        private void ShowBallCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (_restoring)
            {
                return;
            }

            bool show = ShowBallCheckBox.IsChecked == true;
            SetFloatingBallVisible(show);

            _configService.ShowFloatingBall = show;
        }

        /// <summary>按需创建并显示悬浮球窗口（接好点名事件）；隐藏时关闭并释放引用。</summary>
        private void SetFloatingBallVisible(bool visible)
        {
            if (visible)
            {
                if (_floatingBall is null)
                {
                    _floatingBall = new FloatingBallWindow();
                    _floatingBall.PickRequested += OnPickRequested;
                    _floatingBall.Closed += (_, _) => _floatingBall = null;
                }

                _floatingBall.Show();
                _floatingBall.Activate();
            }
            else
            {
                _floatingBall?.Close();
                _floatingBall = null;
            }
        }

        /// <summary>悬浮球请求点名：随机取一名并显示到结果窗口；名单为空时给出提示。</summary>
        private void OnPickRequested()
        {
            string? name = _pickerService.Pick(_configService.Roster);
            if (name is null)
            {
                MessageBox.Show(this, "名单为空，请先选择名单文件。", "提示",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_resultWindow is null || !_resultWindow.IsLoaded)
            {
                _resultWindow = new ResultWindow { Owner = this };
                _resultWindow.Closed += (_, _) => _resultWindow = null;
            }

            _resultWindow.ShowName(name);
        }
    }
}
