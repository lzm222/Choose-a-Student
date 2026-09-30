using System.Collections.Generic;
using System.IO;

namespace Choose_a_Student.Services
{
    /// <summary>
    /// 名单文件读取服务。
    /// </summary>
    public class RosterService
    {
        /// <summary>
        /// 读取名单文件，每行一个姓名，忽略首尾空白与空行。
        /// </summary>
        public List<string> Load(string path)
        {
            var names = new List<string>();

            foreach (string line in File.ReadAllLines(path))
            {
                string name = line.Trim();
                if (name.Length > 0)
                {
                    names.Add(name);
                }
            }

            return names;
        }
    }
}
