using System.Windows;
using System.Windows.Input;

namespace Choose_a_Student
{
    /// <summary>
    /// 大字号点名结果窗口，点击任意处或按 Esc 关闭。
    /// </summary>
    public partial class ResultWindow : Window
    {
        /// <summary>构造函数：初始化界面组件。</summary>
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

        /// <summary>鼠标左键点击窗口任意处时隐藏窗口。</summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }

        /// <summary>按下 Esc 键时隐藏窗口。</summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Hide();
            }
        }
    }
}
