using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    public partial class RedisHelper
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
            return Execute(db => db.StringSet(key, val, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkey = keyValues.Select(k => new KeyValuePair<RedisKey, RedisValue>(k.Key, k.Value)).ToList();
            return Execute(db => db.StringSet(newkey.ToArray()));
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
            return Execute(db => db.StringSet(key, value.ToRedisValue(_serializeType), expiry));
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string StringGet(string key)
        {
            return Execute(db => db.StringGet(key));
        }
        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T StringGet<T>(string key)
        {
            return Execute<T>(db => db.StringGet(key));
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public static double StringIncrement(string key, double val = 1)
        {
            return Execute(db => db.StringIncrement(key, val));
        }
        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public static double StringDecrement(string key, double val = 1)
        {
            return Execute(db => db.StringDecrement(key, val));
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
            return await ExecuteAsync(db => db.StringSetAsync(key, val, expiry));
        }
        /// <summary>
        /// 异步保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            var newkey = keyValues.Select(k => new KeyValuePair<RedisKey, RedisValue>(k.Key, k.Value)).ToList();
            return await ExecuteAsync(db => db.StringSetAsync(newkey.ToArray()));
        }

        /// <summary>
        /// 异步保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            return await ExecuteAsync(db => db.StringSetAsync(key, value.ToRedisValue(_serializeType), expiry));
        }

        /// <summary>
        /// 异步获取单个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<string> StringGetAsync(string key)
        {
            return await ExecuteAsync(db => db.StringGetAsync(key));
        }

        /// <summary>
        /// 异步获取单个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> StringGetAsync<T>(string key)
        {
            return await ExecuteAsync<T>(db => db.StringGetAsync(key));
        }

        /// <summary>
        /// 异步为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public static async Task<double> StringIncrementAsync(string key, double val = 1)
        {
            return await ExecuteAsync(db => db.StringIncrementAsync(key, val));
        }
        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负数</param>
        /// <returns>增长后的值</returns>
        public static async Task<double> StringDecrementAsync(string key, double val = 1)
        {
            return await ExecuteAsync(db => db.StringDecrementAsync(key, val));
        }
        #endregion
        #endregion
    }
}
