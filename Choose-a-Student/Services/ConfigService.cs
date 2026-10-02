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
        private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

        private readonly RosterService _rosterService = new();
        private readonly AppConfig _config;

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
