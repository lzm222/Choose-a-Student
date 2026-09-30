using System.Windows;
using System.Windows.Input;

namespace Choose_a_Student
{
    /// <summary>
    /// 大字号点名结果窗口，点击任意处或按 Esc 关闭。
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow()
        {
            InitializeComponent();
        }

        /// <summary>更新姓名并显示窗口。</summary>
        public void ShowName(string name)
        {
            NameText.Text = name;
            Show();
            Activate();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Hide();
            }
        }
    }
}
