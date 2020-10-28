using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace Sean.Core.Redis.Extensions
{
    /// <summary>
    /// Extension of <see cref="RedisKey"/>
    /// </summary>
    public static class RedisKeyExtensions
    {
        /// <summary>
        /// Convert to <see cref="RedisKey"/> array
        /// </summary>
        /// <param name="keys">Redis keys</param>
        /// <returns></returns>
        public static RedisKey[] ToRedisKeyArray(this IList<string> keys)
        {
            return keys?.Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => (RedisKey)c).ToArray();
        }
    }
}
