using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    public partial class RedisHelper
    {
        #region List: 列表

        #region 同步执行
        /// <summary>
        /// 移除List内部指定的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static long ListRemove<T>(string key, T val)
        {
            return Exec(db => db.ListRemove(key, ToJson(val)));
        }

        /// <summary>
        /// 获取指定Key的List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static List<T> ListRange<T>(string key, long start = 0, long stop = -1)
        {
            return Exec(db =>
            {
                var val = db.ListRange(key, start, stop);
                return ToModelList<T>(val);
            });
        }

        /// <summary>
        /// 插入（入队）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static long ListRightPush<T>(string key, T val)
        {
            return Exec(db => db.ListRightPush(key, ToJson(val)));
        }
        /// <summary>
        /// 批量插入（入队）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static long ListRightPush<T>(string key, IList<T> val)
        {
            return Exec(db => db.ListRightPush(key, val.Select(c => RedisValue.Unbox(ToJson(c))).ToArray()));
        }

        /// <summary>
        /// 取出（出队）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListRightPop<T>(string key)
        {
            return Exec(db =>
            {
                var val = db.ListRightPop(key);
                return ToModel<T>(val);
            });
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void ListLeftPush<T>(string key, T val)
        {
            Exec(db => db.ListLeftPush(key, ToJson(val)));
        }
        /// <summary>
        /// 批量入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static long ListLeftPush<T>(string key, IList<T> val)
        {
            return Exec(db => db.ListLeftPush(key, val.Select(c => RedisValue.Unbox(ToJson(c))).ToArray()));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListLeftPop<T>(string key)
        {
            return Exec(db =>
            {
                var val = db.ListLeftPop(key);
                return ToModel<T>(val);
            });
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetListLength(string key)
        {
            return Exec(db => db.ListLength(key));
        }
        #endregion

        #region 异步执行
        /// <summary>
        /// 异步移除List内部指定的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static async Task<long> ListRemoveAsync<T>(string key, T val)
        {
            return await Exec(db => db.ListRemoveAsync(key, ToJson(val)));
        }

        /// <summary>
        /// 异步获取指定Key的List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> ListRangeAsync<T>(string key)
        {
            var val = await Exec(db => db.ListRangeAsync(key));
            return ToModelList<T>(val);
        }

        /// <summary>
        /// 异步插入（入队）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static async Task<long> ListRightPushAsync<T>(string key, T val)
        {
            return await Exec(db => db.ListRightPushAsync(key, ToJson(val)));
        }
        /// <summary>
        /// 异步批量插入（入队）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static async Task<long> ListRightPushAsync<T>(string key, IList<T> val)
        {
            return await Exec(db => db.ListRightPushAsync(key, val.Select(c => RedisValue.Unbox(ToJson(c))).ToArray()));
        }

        /// <summary>
        /// 异步取出（出队）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> ListRightPopAsync<T>(string key)
        {
            var val = await Exec(db => db.ListRightPopAsync(key));
            return ToModel<T>(val);
        }

        /// <summary>
        /// 异步入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static async Task<long> ListLeftPushAsync<T>(string key, T val)
        {
            return await Exec(db => db.ListLeftPushAsync(key, ToJson(val)));
        }
        /// <summary>
        /// 异步批量入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static async Task<long> ListLeftPushAsync<T>(string key, IList<T> val)
        {
            return await Exec(db => db.ListLeftPushAsync(key, val.Select(c => RedisValue.Unbox(ToJson(c))).ToArray()));
        }

        /// <summary>
        /// 异步出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> ListLeftPopAsync<T>(string key)
        {
            var val = await Exec(db => db.ListLeftPopAsync(key));
            return ToModel<T>(val);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> GetListLengthAsync(string key)
        {
            return await Exec(db => db.ListLengthAsync(key));
        }
        #endregion

        #endregion
    }
}
