using System.Collections.Generic;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    public partial class RedisHelper
    {
        #region Hash: 散列
        #region 同步执行
        /// <summary>
        /// 是否被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool HashExists(string key, string dataKey)
        {
            return Execute(db => db.HashExists(key, dataKey));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool HashSet<T>(string key, string dataKey, T val)
        {
            return Execute(db => db.HashSet(key, dataKey, val.ToRedisValue(_serializeType)));
        }

        /// <summary>
        /// 从hash表中移除数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool HashDelete(string key, string dataKey)
        {
            return Execute(db => db.HashDelete(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static long HashRemove(string key, List<RedisValue> dataKey)
        {
            return Execute(db => db.HashDelete(key, dataKey.ToArray()));
        }

        /// <summary>
        /// 从hash表中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static T HashGet<T>(string key, string dataKey)
        {
            return Execute<T>(db => db.HashGet(key, dataKey));
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double HashIncrement(string key, string dataKey, double val = 1)
        {
            return Execute(db => db.HashIncrement(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double HashDecrement(string key, string dataKey, double val = 1)
        {
            return Execute(db => db.HashDecrement(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> HashKeys<T>(string key)
        {
            return Execute<T>(db => db.HashKeys(key));
        }
        #endregion

        #region 异步执行
        /// <summary>
        /// 异步是否被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<bool> HashExistsAsync(string key, string dataKey)
        {
            return await ExecuteAsync(db => db.HashExistsAsync(key, dataKey));
        }

        /// <summary>
        /// 异步存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<bool> HashSetAsync<T>(string key, string dataKey, T val)
        {
            return await ExecuteAsync(db => db.HashSetAsync(key, dataKey, val.ToRedisValue(_serializeType)));
        }

        /// <summary>
        /// 异步从hash表中移除数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<bool> HashDeleteAsync(string key, string dataKey)
        {
            return await ExecuteAsync(db => db.HashDeleteAsync(key, dataKey));
        }

        /// <summary>
        /// 异步移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<long> HashRemoveAsync(string key, List<RedisValue> dataKey)
        {
            return await ExecuteAsync(db => db.HashDeleteAsync(key, dataKey.ToArray()));
        }

        /// <summary>
        /// 从hash表中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<T> HashGetAsync<T>(string key, string dataKey)
        {
            return await ExecuteAsync<T>(db => db.HashGetAsync(key, dataKey));
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            return await ExecuteAsync(db => db.HashIncrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            return await ExecuteAsync(db => db.HashDecrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> HashKeysAsync<T>(string key)
        {
            return await ExecuteAsync<T>(db => db.HashKeysAsync(key));
        }
        #endregion
        #endregion
    }
}
