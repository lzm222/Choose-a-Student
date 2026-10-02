using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Choose_a_Student.Models;
using Choose_a_Student.Services;

namespace Choose_a_Student
{
    /// <summary>
    /// 主窗口：选择名单、控制悬浮球。
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ConfigService _configService;
        private readonly RosterService _rosterService = new();
        private readonly PickerService _pickerService = new();
        private readonly AppConfig _config;

        private List<string> _roster = new();
        private FloatingBallWindow? _floatingBall;
        private ResultWindow? _resultWindow;

        /// <summary>初始化期间抑制控件事件回写配置。</summary>
        private bool _restoring = false;

        public MainWindow(ConfigService configService)
        {
            _configService = configService;
            _config = configService.Load();

            InitializeComponent();
            RestoreState();
        }

        private void RestoreState()
        {
            _restoring = true;

            if (!string.IsNullOrEmpty(_config.RosterFilePath) && File.Exists(_config.RosterFilePath))
            {
                _roster = _rosterService.Load(_config.RosterFilePath);
                UpdateRosterInfo(_config.RosterFilePath);
            }
            else
            {
                UpdateRosterInfo(null);
            }

            ShowBallCheckBox.IsChecked = _config.ShowFloatingBall;
            _restoring = false;

            if (_config.ShowFloatingBall)
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
                LoadRoster(dialog.FileName);
            }
        }

        private void LoadRoster(string path)
        {
            try
            {
                _roster = _rosterService.Load(path);
                _config.RosterFilePath = path;
                UpdateRosterInfo(path);
                _configService.Save(_config);
            }
            catch (Exception ex)
            {
                _roster = new List<string>();
                UpdateRosterInfo(null);
                MessageBox.Show(this, "读取名单失败：" + ex.Message, "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateRosterInfo(string? path)
        {
            FilePathText.Text = path is null ? "名单文件：未选择" : "名单文件：" + path;
            CountText.Text = "人数：" + _roster.Count;
        }

        private void ShowBallCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (_restoring)
            {
                return;
            }

            bool show = ShowBallCheckBox.IsChecked == true;
            SetFloatingBallVisible(show);

            _config.ShowFloatingBall = show;
            _configService.Save(_config);
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
            string? name = _pickerService.Pick(_roster);
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
