using System;
using System.Windows;
using Microsoft.Win32;

namespace Choose_a_Student
{
    /// <summary>
    /// 主窗口：只负责界面渲染，并把用户操作以回调形式抛出，不包含任何业务逻辑。
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>设置控件状态期间，抑制事件外抛。</summary>
        private bool _restoring = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>用户选定名单文件后触发，携带文件完整路径。</summary>
        public event Action<string>? RosterFileSelected;

        /// <summary>「显示悬浮球」勾选状态变化时触发。</summary>
        public event Action<bool>? ShowFloatingBallToggled;

        /// <summary>更新名单信息显示。</summary>
        /// <param name="path">名单文件路径；null 或空表示未选择。</param>
        /// <param name="count">名单人数。</param>
        public void SetRosterInfo(string? path, int count)
        {
            FilePathText.Text = string.IsNullOrEmpty(path) ? "名单文件：未选择" : "名单文件：" + path;
            CountText.Text = "人数：" + count;
        }

        /// <summary>设置「显示悬浮球」勾选状态，且不回抛事件。</summary>
        /// <param name="show">是否勾选。</param>
        public void SetShowFloatingBall(bool show)
        {
            _restoring = true;
            ShowBallCheckBox.IsChecked = show;
            _restoring = false;
        }

        /// <summary>提示名单为空。</summary>
        public void ShowNoRosterMessage()
        {
            MessageBox.Show(this, "名单为空，请先选择名单文件。", "提示",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>提示读取名单失败。</summary>
        /// <param name="message">失败原因。</param>
        public void ShowRosterError(string message)
        {
            MessageBox.Show(this, "读取名单失败：" + message, "错误",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>「选择名单文件」按钮点击：弹出文件对话框，选定后抛出 <see cref="RosterFileSelected"/>。</summary>
        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "选择名单文件",
                Filter = "文本文件|*.txt|所有文件|*.*",
            };

            if (dialog.ShowDialog(this) == true)
            {
                RosterFileSelected?.Invoke(dialog.FileName);
            }
        }

        /// <summary>「显示悬浮球」勾选变化：抛出 <see cref="ShowFloatingBallToggled"/>。</summary>
        private void ShowBallCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (_restoring)
            {
                return;
            }

            ShowFloatingBallToggled?.Invoke(ShowBallCheckBox.IsChecked == true);
        }
    }
}
