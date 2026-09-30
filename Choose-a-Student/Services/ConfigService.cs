using System;
using System.IO;
using System.Text.Json;
using Choose_a_Student.Models;

namespace Choose_a_Student.Services
{
    /// <summary>
    /// 负责配置的读取与写入。
    /// </summary>
    public class ConfigService
    {
        private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

        public ConfigService()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Choose-a-Student");
            ConfigPath = Path.Combine(dir, "config.json");
        }

        /// <summary>配置文件完整路径。</summary>
        public string ConfigPath { get; }

        /// <summary>读取配置，文件缺失或损坏时返回默认配置。</summary>
        public AppConfig Load()
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

        /// <summary>写入配置，失败时静默忽略（原型阶段）。</summary>
        public void Save(AppConfig config)
        {
            try
            {
                string? dir = Path.GetDirectoryName(ConfigPath);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(config, SerializerOptions));
            }
            catch
            {
                // 原型阶段忽略写盘失败。
            }
        }
    }
}
