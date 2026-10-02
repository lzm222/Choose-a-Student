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
        private readonly ConfigService _configService;
        private readonly PickerService _pickerService = new();

        private FloatingBallWindow? _floatingBall;
        private ResultWindow? _resultWindow;

        /// <summary>初始化期间抑制控件事件回写配置。</summary>
        private bool _restoring = false;

        public MainWindow(ConfigService configService)
        {
            _configService = configService;

            InitializeComponent();
            RestoreState();
        }

        private void RestoreState()
        {
            RefreshFromConfig();

            if (_configService.ShowFloatingBall)
            {
                SetFloatingBallVisible(true);
            }
        }

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
