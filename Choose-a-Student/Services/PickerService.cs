using System;
using System.Collections.Generic;

namespace Choose_a_Student.Services
{
    /// <summary>
    /// 随机点名服务。
    /// </summary>
    public class PickerService
    {
        /// <summary>从名单中随机取一名。</summary>
        /// <param name="names">候选姓名列表。</param>
        /// <returns>随机选中的姓名；名单为空时返回 null。</returns>
        public string? Pick(IReadOnlyList<string> names)
        {
            if (names.Count == 0)
            {
                return null;
            }

            return names[Random.Shared.Next(names.Count)];
        }
    }
}
