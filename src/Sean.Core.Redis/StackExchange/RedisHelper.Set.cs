using System.Threading.Tasks;

namespace Sean.Core.Redis.StackExchange
{
    public partial class RedisHelper
    {
        #region Set: 集合
        #region 同步执行
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool SetAdd(string key, string val)
        {
            return Execute(db => db.SetAdd(key, val));
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SetLength(string key)
        {
            return Execute(db => db.SetLength(key));
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool SetContains(string key, string val)
        {
            return Execute(db => db.SetContains(key, val));
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool SetRemove(string key, string val)
        {
            return Execute(db => db.SetRemove(key, val));
        }
        #endregion

        #region 异步执行
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<bool> SetAddAsync(string key, string val)
        {
            return await ExecuteAsync(db => db.SetAddAsync(key, val));
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> SetLengthAsync(string key)
        {
            return await ExecuteAsync(db => db.SetLengthAsync(key));
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<bool> SetContainsAsync(string key, string val)
        {
            return await ExecuteAsync(db => db.SetContainsAsync(key, val));
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<bool> SetRemoveAsync(string key, string val)
        {
            return await ExecuteAsync(db => db.SetRemoveAsync(key, val));
        }
        #endregion
        #endregion
    }
}
