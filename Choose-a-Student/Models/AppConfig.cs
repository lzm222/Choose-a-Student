namespace Choose_a_Student.Models
{
    /// <summary>
    /// 应用配置，持久化到 %AppData%\Choose-a-Student\config.json。
    /// </summary>
    public class AppConfig
    {
        /// <summary>名单文件路径。</summary>
        public string RosterFilePath { get; set; } = string.Empty;

        /// <summary>是否显示悬浮球。</summary>
        public bool ShowFloatingBall { get; set; }

        /// <summary>悬浮球左侧位置；null 表示未记录，使用默认位置。</summary>
        public double? FloatingBallLeft { get; set; }

        /// <summary>悬浮球顶部位置；null 表示未记录，使用默认位置。</summary>
        public double? FloatingBallTop { get; set; }
    }
}
