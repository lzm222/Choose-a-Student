using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Choose_a_Student.Models;

namespace Choose_a_Student.Services
{
    /// <summary>
    /// 配置与名单的统一维护者：持有唯一的 <see cref="AppConfig"/> 与已加载的名单，
    /// 并负责配置文件的读取与写入。
    /// </summary>
    public class ConfigService
    {
        /// <summary>配置序列化选项：缩进输出，便于手工查看。</summary>
        private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

        private readonly RosterService _rosterService = new();
        private readonly AppConfig _config;

        /// <summary>
        /// 构造函数：确定配置文件路径，载入配置并预载名单。
        /// </summary>
        public ConfigService()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Choose-a-Student");
            ConfigPath = Path.Combine(dir, "config.json");

            _config = LoadAppConfig();
            InitializeRoster();
        }

        /// <summary>配置文件完整路径。</summary>
        public string ConfigPath { get; }

        /// <summary>当前名单文件路径，未设置时为空字符串。</summary>
        public string RosterFilePath => _config.RosterFilePath;

        /// <summary>已加载的名单，未加载时为空集合。</summary>
        public IReadOnlyList<string> Roster { get; private set; } = Array.Empty<string>();

        /// <summary>是否显示悬浮球，设置后立即持久化。</summary>
        public bool ShowFloatingBall
        {
            get => _config.ShowFloatingBall;
            set
            {
                _config.ShowFloatingBall = value;
                Save();
            }
        }

        /// <summary>
        /// 设置名单文件：校验并加载，成功则写入配置并持久化。
        /// 失败时返回 false，并通过 <paramref name="error"/> 给出原因。
        /// </summary>
        /// <param name="path">名单文件路径。</param>
        /// <param name="error">失败原因；成功时为 null。</param>
        /// <returns>加载并保存成功时返回 true，否则返回 false。</returns>
        public bool TrySetRosterFile(string path, out string? error)
        {
            try
            {
                Roster = _rosterService.Load(path);
                _config.RosterFilePath = path;
                Save();
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 按已保存的名单路径预载名单；路径无效或读取失败时名单保持为空。
        /// </summary>
        private void InitializeRoster()
        {
            if (!_rosterService.IsValid(_config.RosterFilePath))
            {
                return;
            }

            try
            {
                Roster = _rosterService.Load(_config.RosterFilePath);
            }
            catch
            {
                Roster = Array.Empty<string>();
            }
        }

        /// <summary>从磁盘读取配置；文件缺失或损坏时回退为默认配置。</summary>
        /// <returns>读取到的配置，异常情况下为默认配置。</returns>
        private AppConfig LoadAppConfig()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                {
                    return new AppConfig();
                }

                string json = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            catch
            {
                return new AppConfig();
            }
        }

        /// <summary>把当前配置写回磁盘（必要时创建目录），失败时静默忽略。</summary>
        private void Save()
        {
            try
            {
                string? dir = Path.GetDirectoryName(ConfigPath);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(_config, SerializerOptions));
            }
            catch
            {
                // 原型阶段忽略写盘失败。
            }
        }
    }
}
