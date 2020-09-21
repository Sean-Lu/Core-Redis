using System.Collections.Generic;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;

namespace Sean.Core.Redis.StackExchange
{
    public partial class RedisHelper
    {
        #region Sorted Set: 有序集合
        #region 同步执行
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool SortedSetExists<T>(string key, T val)
        {
            var rank = Execute(db => db.SortedSetRank(key, val.ToRedisValue(_serializeType)));
            return rank.HasValue && rank.Value >= 0;
        }

        /// <summary>
        /// 无序添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd<T>(string key, T val, double score)
        {
            return Execute(db => db.SortedSetAdd(key, val.ToRedisValue(_serializeType), score));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool SortedSetRemove<T>(string key, T val)
        {
            return Execute(db => db.SortedSetRemove(key, val.ToRedisValue(_serializeType)));
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static List<T> SortedSetRangeByRank<T>(string key, long start = 0, long stop = -1)
        {
            return Execute<T>(db => db.SortedSetRangeByRank(key, start, stop));
        }

        /// <summary>
        ///  获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SortedSetLength(string key)
        {
            return Execute(db => db.SortedSetLength(key));

        }
        #endregion

        #region 异步执行
        /// <summary>
        /// 异步判断是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<bool> SortedSetExistsAsync<T>(string key, T val)
        {
            var rank = await ExecuteAsync(db => db.SortedSetRankAsync(key, val.ToRedisValue(_serializeType)));
            return rank.HasValue && rank.Value >= 0;
        }

        /// <summary>
        /// 异步无序添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async Task<bool> SortedSetAddAsync<T>(string key, T val, double score)
        {
            return await ExecuteAsync(db => db.SortedSetAddAsync(key, val.ToRedisValue(_serializeType), score));
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<bool> SortedSetRemoveAsync<T>(string key, T val)
        {
            return await ExecuteAsync(db => db.SortedSetRemoveAsync(key, val.ToRedisValue(_serializeType)));
        }

        /// <summary>
        /// 异步获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static async Task<List<T>> SortedSetRangeByRankAsync<T>(string key, long start = 0, long stop = -1)
        {
            return await ExecuteAsync<T>(db => db.SortedSetRangeByRankAsync(key, start, stop));
        }

        /// <summary>
        ///  异步获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> SortedSetLengthAsync(string key)
        {
            return await ExecuteAsync(db => db.SortedSetLengthAsync(key));

        }
        #endregion
        #endregion
    }
}
