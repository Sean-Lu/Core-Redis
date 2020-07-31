using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sean.Core.Redis.Contracts;
using Sean.Utility.Contracts;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    /// <summary>
    /// Redis缓存操作（基于StackExchange.Redis）
    /// </summary>
    public partial class RedisHelper : RedisClientBase, ICache
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="logger">日志</param>
        public static void Init(ILogger logger = null)
        {
            RedisClientBase.Init(null, null, logger);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="endPoints">redis地址，示例：127.0.0.1:6379</param>
        /// <param name="password">redis密码，默认为null</param>
        /// <param name="logger">日志</param>
        public new static void Init(List<string> endPoints, string password = null, ILogger logger = null)
        {
            RedisClientBase.Init(endPoints, password, logger);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">缓存操作的委托</param>
        /// <param name="db">数据库索引</param>
        /// <param name="exceptionAction">处理异常的委托</param>
        /// <returns></returns>
        public new static T Exec<T>(Func<IDatabase, T> func, int db = -1, Action<Exception> exceptionAction = null)
        {
            return RedisClientBase.Exec(func, db, exceptionAction);
        }

        /// <summary>
        /// 获取指定索引的数据库
        /// </summary>
        /// <param name="db">-1表示默认数据库</param>
        /// <returns></returns>
        public new static IDatabase GetDatabase(int db = -1)
        {
            return RedisClientBase.GetDatabase(db);
        }

        #region Lock（分布式锁）：由于 Redis 是单线程模型，命令操作原子性，所以利用这个特性可以很容易的实现分布式锁。
        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="key">锁的名称</param>
        /// <param name="token">用来标识谁拥有该锁并用来释放锁</param>
        /// <param name="expiry">表示该锁的有效时间，避免死锁。</param>
        /// <returns></returns>
        public static bool LockTake(string key, string token, TimeSpan expiry)
        {
            return Exec(db => db.LockTake(key, token, expiry));
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的名称</param>
        /// <param name="token">用来标识谁拥有该锁并用来释放锁</param>
        /// <returns></returns>
        public static bool LockRelease(string key, string token)
        {
            return Exec(db => db.LockRelease(key, token));
        }

        /// <summary>
        /// 异步获取锁
        /// </summary>
        /// <param name="key">锁的名称</param>
        /// <param name="token">用来标识谁拥有该锁并用来释放锁</param>
        /// <param name="expiry">表示该锁的有效时间，避免死锁。</param>
        /// <returns></returns>
        public static async Task<bool> LockTakeAsync(string key, string token, TimeSpan expiry)
        {
            return await Exec(db => db.LockTakeAsync(key, token, expiry));
        }
        /// <summary>
        /// 异步释放锁
        /// </summary>
        /// <param name="key">锁的名称</param>
        /// <param name="token">用来标识谁拥有该锁并用来释放锁</param>
        /// <returns></returns>
        public static async Task<bool> LockReleaseAsync(string key, string token)
        {
            return await Exec(db => db.LockReleaseAsync(key, token));
        }
        #endregion
    }
}
