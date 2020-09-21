using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    public partial class RedisHelper
    {
        #region Key
        #region 同步执行
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyExists(string key)
        {
            return Execute(db => db.KeyExists(key));
        }

        /// <summary>
        /// 删除单个Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyDelete(string key)
        {
            return Execute(db => db.KeyDelete(key));
        }
        /// <summary>
        /// 删除多个Key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static long KeyDelete(IList<string> keys)
        {
            if (keys == null)
            {
                return -1;
            }

            return Execute(db => db.KeyDelete(keys.Select(c => (RedisKey)c).ToArray()));
        }

        /// <summary>
        /// 重命名Key
        /// </summary>
        /// <param name="key">old key name</param>
        /// <param name="newKey">new key name</param>
        /// <returns></returns>
        public static bool KeyRename(string key, string newKey)
        {
            return Execute(db => db.KeyRename(key, newKey));
        }

        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool KeyExpire(string key, DateTime? expiry)
        {
            return Execute(db => db.KeyExpire(key, expiry));
        }
        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool KeyExpire(string key, TimeSpan? expiry)
        {
            return Execute(db => db.KeyExpire(key, expiry));
        }

        /// <summary>
        /// 返回给定 key 的剩余生存时间(TTL, time to live)。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TimeSpan? KeyTimeToLive(string key)
        {
            // Redis TTL 命令 - 以秒为单位，返回给定 key 的剩余生存时间(TTL, time to live)。
            // 当 key 存在但没有设置剩余生存时间时，返回 -1 。
            return Execute(db => db.KeyTimeToLive(key));
        }
        #endregion

        #region 异步执行
        /// <summary>
        /// 异步判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> KeyExistsAsync(string key)
        {
            return await ExecuteAsync(db => db.KeyExistsAsync(key));
        }

        /// <summary>
        /// 异步删除单个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> KeyDeleteAsync(string key)
        {
            return await ExecuteAsync(db => db.KeyDeleteAsync(key));
        }

        /// <summary>
        /// 异步删除多个Key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static async Task<long> KeyDeleteAsync(List<string> keys)
        {
            return await ExecuteAsync(db => db.KeyDeleteAsync(keys.Select(c => (RedisKey)c).ToArray()));
        }

        /// <summary>
        ///  异步重命名Key
        /// </summary>
        /// <param name="key">old key name</param>
        /// <param name="newKey">new key name</param>
        /// <returns></returns>
        public static async Task<bool> KeyRenameAsync(string key, string newKey)
        {
            return await ExecuteAsync(db => db.KeyRenameAsync(key, newKey));
        }

        /// <summary>
        /// 异步设置Key的时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> KeyExpireAsync(string key, DateTime? expiry)
        {
            return await ExecuteAsync(db => db.KeyExpireAsync(key, expiry));
        }
        /// <summary>
        /// 异步设置Key的时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> KeyExpireAsync(string key, TimeSpan? expiry)
        {
            return await ExecuteAsync(db => db.KeyExpireAsync(key, expiry));
        }

        /// <summary>
        /// 异步返回给定 key 的剩余生存时间(TTL, time to live)。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<TimeSpan?> KeyTimeToLiveAsync(string key)
        {
            return await ExecuteAsync(db => db.KeyTimeToLiveAsync(key));
        }
        #endregion
        #endregion
    }
}
