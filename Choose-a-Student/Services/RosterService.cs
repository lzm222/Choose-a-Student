using System.Collections.Generic;
using System.IO;

namespace Choose_a_Student.Services
{
    /// <summary>
    /// 名单文件读取服务。
    /// </summary>
    public class RosterService
    {
        /// <summary>判断名单文件路径是否有效（非空白且文件存在）。</summary>
        public bool IsValid(string? path)
        {
            return !string.IsNullOrWhiteSpace(path) && File.Exists(path);
        }

        /// <summary>
        /// 读取名单文件，每行一个姓名，忽略首尾空白与空行。
        /// 路径无效或名单为空时抛出 <see cref="InvalidDataException"/>。
        /// </summary>
        public List<string> Load(string path)
        {
            if (!IsValid(path))
            {
                throw new InvalidDataException($"名单文件不存在：{path}");
            }

            var names = new List<string>();

            foreach (string line in File.ReadAllLines(path))
            {
                string name = line.Trim();
                if (name.Length > 0)
                {
                    names.Add(name);
                }
            }

            if (names.Count == 0)
            {
                throw new InvalidDataException("名单文件为空。");
            }

            return names;
        }
    }
}
