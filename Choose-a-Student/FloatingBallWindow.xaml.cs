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
        /// <summary>构造函数：初始化界面组件。</summary>
        public FloatingBallWindow()
        {
            InitializeComponent();
        }

        /// <summary>左半区被点击时触发，由宿主负责抽取并展示结果。</summary>
        public event Action? PickRequested;

        /// <summary>拖动结束后触发，携带窗口最终的左、上坐标，由宿主负责持久化。</summary>
        public event Action<double, double>? PositionChanged;

        /// <summary>
        /// 恢复悬浮球位置：两个坐标都有效时才应用，随后把窗口钳制到可见区域内。
        /// 由宿主在显示窗口前用配置调用；未记录位置时保持 XAML 中的默认位置。
        /// </summary>
        /// <param name="left">已记录的左侧位置；null 表示无记录。</param>
        /// <param name="top">已记录的顶部位置；null 表示无记录。</param>
        public void RestorePosition(double? left, double? top)
        {
            if (left is not double l || top is not double t)
            {
                return;
            }

            Left = l;
            Top = t;
            ClampToScreen();
        }

        /// <summary>
        /// 鼠标左键按下：落在左半区则触发点名，落在右半区则拖拽移动窗口。
        /// </summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).X < Width / 2)
            {
                PickRequested?.Invoke();
            }
            else
            {
                DragMove();
                PositionChanged?.Invoke(Left, Top);
            }
        }

        /// <summary>
        /// 把窗口限制在虚拟屏幕范围内，避免换显示器或改分辨率后悬浮球留在屏幕外无法找回。
        /// </summary>
        private void ClampToScreen()
        {
            double minLeft = SystemParameters.VirtualScreenLeft;
            double minTop = SystemParameters.VirtualScreenTop;
            double maxLeft = minLeft + SystemParameters.VirtualScreenWidth - Width;
            double maxTop = minTop + SystemParameters.VirtualScreenHeight - Height;

            Left = Math.Clamp(Left, minLeft, Math.Max(minLeft, maxLeft));
            Top = Math.Clamp(Top, minTop, Math.Max(minTop, maxTop));
        }
    }
}
