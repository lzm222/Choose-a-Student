using System;
using System.Collections.Generic;

namespace Choose_a_Student.Services
{
    /// <summary>
    /// 随机点名服务。
    /// </summary>
    public class PickerService
    {
        /// <summary>
        /// 从名单中随机取一名，名单为空时返回 null。
        /// </summary>
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
