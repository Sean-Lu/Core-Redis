using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sean.Core.Redis.Contracts;
using Sean.Core.Redis.Extensions;
using Sean.Utility.Contracts;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    /// <summary>
    /// Redis缓存操作（基于StackExchange.Redis）
    /// </summary>
    public partial class RedisHelper
    {
        /// <summary>
        /// Serialize type
        /// </summary>
        public static SerializeType SerializeType => _serializeType;
        /// <summary>
        /// 处理异常
        /// </summary>
        public static event EventHandler<Exception> OnException;

        private static ConnectionMultiplexer _conn;
        private static ConcurrentDictionary<int, IDatabase> _dbDic;
        private static SerializeType _serializeType;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            Init(options => { });
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config">redis configuration</param>
        public static void Init(Action<RedisConfigOptions> config)
        {
            var options = new RedisConfigOptions();
            config?.Invoke(options);
            Init(options);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config">redis configuration</param>
        public static void Init(RedisConfigOptions config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            RedisManager.Init(config);

            _conn = RedisManager.Instance ?? throw new Exception("Init redis client fail.");
            _dbDic = new ConcurrentDictionary<int, IDatabase>();
            _serializeType = config.SerializeType;
        }

        /// <summary>
        /// 获取指定索引的数据库
        /// </summary>
        /// <param name="index">-1表示默认数据库</param>
        /// <returns></returns>
        public static IDatabase GetDatabase(int index = -1)
        {
            return _dbDic.GetOrAdd(index, key => _conn.GetDatabase(key));
        }

        #region Execute【执行】
        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">缓存操作的委托</param>
        /// <param name="db">数据库索引</param>
        /// <param name="exceptionAction">处理异常的委托</param>
        /// <returns></returns>
        public static T Execute<T>(Func<IDatabase, T> func, int db = -1, Action<Exception> exceptionAction = null)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (typeof(T).BaseType == typeof(Task))
                throw new InvalidOperationException($"For asynchronous operations, please use the {nameof(ExecuteAsync)} method.");

            try
            {
                return func(GetDatabase(db));
            }
            catch (Exception ex)
            {
                exceptionAction?.Invoke(ex);
                OnException?.Invoke(null, ex);
                return default;
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">缓存操作的委托</param>
        /// <param name="serializeType"></param>
        /// <param name="db">数据库索引</param>
        /// <param name="exceptionAction">处理异常的委托</param>
        /// <returns></returns>
        public static T Execute<T>(Func<IDatabase, RedisValue> func, SerializeType? serializeType = null, int db = -1, Action<Exception> exceptionAction = null)
        {
            return Execute(func, db, exceptionAction).ToModel<T>(serializeType ?? _serializeType); ;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">缓存操作的委托</param>
        /// <param name="serializeType"></param>
        /// <param name="db">数据库索引</param>
        /// <param name="exceptionAction">处理异常的委托</param>
        /// <returns></returns>
        public static List<T> Execute<T>(Func<IDatabase, RedisValue[]> func, SerializeType? serializeType = null, int db = -1, Action<Exception> exceptionAction = null)
        {
            return Execute(func, db, exceptionAction).ToModelList<T>(serializeType ?? _serializeType);
        }
        #endregion

        #region ExecuteAsync【异步执行】
        /// <summary>
        /// 执行（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">缓存操作的委托</param>
        /// <param name="db">数据库索引</param>
        /// <param name="exceptionAction">处理异常的委托</param>
        /// <returns></returns>
        public static async Task<T> ExecuteAsync<T>(Func<IDatabase, Task<T>> func, int db = -1, Action<Exception> exceptionAction = null)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            try
            {
                return await func(GetDatabase(db));
            }
            catch (Exception ex)
            {
                exceptionAction?.Invoke(ex);
                OnException?.Invoke(null, ex);
                return default;
            }
        }

        /// <summary>
        /// 执行（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">缓存操作的委托</param>
        /// <param name="serializeType"></param>
        /// <param name="db">数据库索引</param>
        /// <param name="exceptionAction">处理异常的委托</param>
        /// <returns></returns>
        public static async Task<T> ExecuteAsync<T>(Func<IDatabase, Task<RedisValue>> func, SerializeType? serializeType = null, int db = -1, Action<Exception> exceptionAction = null)
        {
            return (await ExecuteAsync(func, db, exceptionAction)).ToModel<T>(serializeType ?? _serializeType);
        }

        /// <summary>
        /// 执行（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">缓存操作的委托</param>
        /// <param name="serializeType"></param>
        /// <param name="db">数据库索引</param>
        /// <param name="exceptionAction">处理异常的委托</param>
        /// <returns></returns>
        public static async Task<List<T>> ExecuteAsync<T>(Func<IDatabase, Task<RedisValue[]>> func, SerializeType? serializeType = null, int db = -1, Action<Exception> exceptionAction = null)
        {
            return (await ExecuteAsync(func, db, exceptionAction)).ToModelList<T>(serializeType ?? _serializeType); ;
        }
        #endregion

        #region Lock（分布式锁）：由于 Redis 是单线程模型，命令操作原子性，所以利用这个特性可以很容易的实现分布式锁。
        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="key">锁的名称</param>
        /// <param name="value">用来标识谁拥有该锁并用来释放锁</param>
        /// <param name="expiry">表示该锁的有效时间，避免死锁。</param>
        /// <returns></returns>
        public static bool LockTake(string key, string value, TimeSpan expiry)
        {
            return Execute(db => db.LockTake(key, value, expiry));
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的名称</param>
        /// <param name="value">用来标识谁拥有该锁并用来释放锁</param>
        /// <returns></returns>
        public static bool LockRelease(string key, string value)
        {
            return Execute(db => db.LockRelease(key, value));
        }

        /// <summary>
        /// 异步获取锁
        /// </summary>
        /// <param name="key">锁的名称</param>
        /// <param name="value">用来标识谁拥有该锁并用来释放锁</param>
        /// <param name="expiry">表示该锁的有效时间，避免死锁。</param>
        /// <returns></returns>
        public static async Task<bool> LockTakeAsync(string key, string value, TimeSpan expiry)
        {
            return await ExecuteAsync(db => db.LockTakeAsync(key, value, expiry));
        }
        /// <summary>
        /// 异步释放锁
        /// </summary>
        /// <param name="key">锁的名称</param>
        /// <param name="value">用来标识谁拥有该锁并用来释放锁</param>
        /// <returns></returns>
        public static async Task<bool> LockReleaseAsync(string key, string value)
        {
            return await ExecuteAsync(db => db.LockReleaseAsync(key, value));
        }
        #endregion
    }
}
