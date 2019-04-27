using System;

namespace Wechat.Backend.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 获取当前本地时间戳
        /// </summary>
        /// <returns></returns>      
        public static long GetTimestamp(this DateTime dt)
        {
            var diff = dt.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var t = (long) diff.TotalSeconds;

            return t;
        }
    }
}