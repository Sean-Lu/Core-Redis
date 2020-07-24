using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sean.Core.Redis.Contracts;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    /// <summary>
    /// Redis缓存操作（基于StackExchange.Redis）
    /// </summary>
    public partial class RedisHelper : ICache
    {
        #region String: 字符串
        #region 同步执行
        /// <summary>
        /// 单个保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static bool StringSet(string key, string val, TimeSpan? expiry = null)
        {
            return Exec(db => db.StringSet(key, val, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkey = keyValues.Select(k => new KeyValuePair<RedisKey, RedisValue>(k.Key, k.Value)).ToList();
            return Exec(db => db.StringSet(newkey.ToArray()));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool StringSet<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = ToJson(value);
            return Exec(db => db.StringSet(key, json, expiry));
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string StringGet(string key)
        {
            return Exec(db => db.StringGet(key));
        }
        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T StringGet<T>(string key)
        {
            var val = Exec(db => db.StringGet(key));
            return ToModel<T>(val);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public static double StringIncrement(string key, double val = 1)
        {
            return Exec(db => db.StringIncrement(key, val));
        }
        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public static double StringDecrement(string key, double val = 1)
        {
            return Exec(db => db.StringDecrement(key, val));
        }
        #endregion

        #region 异步执行
        /// <summary>
        /// 异步保存单个
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync(string key, string val, TimeSpan? expiry = null)
        {
            return await Exec(db => db.StringSetAsync(key, val, expiry));
        }
        /// <summary>
        /// 异步保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkey = keyValues.Select(k => new KeyValuePair<RedisKey, RedisValue>(k.Key, k.Value)).ToList();
            return await Exec(db => db.StringSetAsync(newkey.ToArray()));
        }

        /// <summary>
        /// 异步保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = null)
        {
            string json = ToJson(obj);
            return await Exec(db => db.StringSetAsync(key, json, expiry));
        }

        /// <summary>
        /// 异步获取单个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<string> StringGetAsync(string key)
        {
            return await Exec(db => db.StringGetAsync(key));
        }

        /// <summary>
        /// 异步获取单个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> StringGetAsync<T>(string key)
        {
            var val = await Exec(db => db.StringGetAsync(key));
            return ToModel<T>(val);
        }

        /// <summary>
        /// 异步为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public static async Task<double> StringIncrementAsync(string key, double val = 1)
        {
            return await Exec(db => db.StringIncrementAsync(key, val));
        }
        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public static async Task<double> StringDecrementAsync(string key, double val = 1)
        {
            return await Exec(db => db.StringDecrementAsync(key, val));
        }
        #endregion
        #endregion
    }
}
