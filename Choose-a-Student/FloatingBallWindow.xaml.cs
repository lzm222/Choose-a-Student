using System;
using System.Windows;
using System.Windows.Input;

namespace Choose_a_Student
{
    /// <summary>
    /// 矩形无边框置顶悬浮球：左半区点击触发点名，右半区拖拽移动窗口。
    /// </summary>
    public partial class FloatingBallWindow : Window
    {
        public FloatingBallWindow()
        {
            InitializeComponent();
        }

        /// <summary>左半区被点击时触发，由宿主负责抽取并展示结果。</summary>
        public event Action? PickRequested;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).X < Width / 2)
            {
                PickRequested?.Invoke();
            }
            else
            {
                DragMove();
            }
        }
    }
}
