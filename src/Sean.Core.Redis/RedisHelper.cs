using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;
using StackExchange.Redis;

namespace Sean.Core.Redis
{
    /// <summary>
    /// Helper for operating redis cache (based on StackExchange.Redis: https://github.com/StackExchange/StackExchange.Redis)
    /// </summary>
    public partial class RedisHelper
    {
        // Common redis data types: String, List, Set, SortedSet, Hash

        /// <summary>
        /// Default serialization type
        /// </summary>
        public static SerializeType DefaultSerializeType => _defaultSerializeType;
        /// <summary>
        /// Handle exception
        /// </summary>
        public static event EventHandler<Exception> OnException;

        private static ConnectionMultiplexer _conn;
        private static ConcurrentDictionary<int, IDatabase> _dbDic;
        private static SerializeType _defaultSerializeType;

        #region Initialization
        /// <summary>
        /// Initialization
        /// </summary>
        public static void Init()
        {
            Init(options => { });
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="config">Redis configuration</param>
        public static void Init(Action<RedisConfigOptions> config)
        {
            var options = new RedisConfigOptions();
            config?.Invoke(options);
            Init(options);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="config">redis configuration</param>
        public static void Init(RedisConfigOptions config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            RedisManager.Init(config);

            _conn = RedisManager.Instance ?? throw new Exception("Init redis client fail.");
            _dbDic = new ConcurrentDictionary<int, IDatabase>();
            _defaultSerializeType = config.DefaultSerializeType;
        }
        #endregion

        #region Synchronous execution
        /// <summary>
        /// Synchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="db">Redis database index</param>
        /// <param name="exceptionAction">The delegate used to handle exception</param>
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
        /// Synchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="serializeType">Serialization type</param>
        /// <param name="db">Redis database index</param>
        /// <param name="exceptionAction">The delegate used to handle exception</param>
        /// <returns></returns>
        public static T Execute<T>(Func<IDatabase, RedisValue> func, SerializeType? serializeType = null, int db = -1, Action<Exception> exceptionAction = null)
        {
            return Execute(func, db, exceptionAction).ToModel<T>(serializeType ?? _defaultSerializeType); ;
        }

        /// <summary>
        /// Synchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="serializeType">Serialization type</param>
        /// <param name="db">Redis database index</param>
        /// <param name="exceptionAction">The delegate used to handle exception</param>
        /// <returns></returns>
        public static List<T> Execute<T>(Func<IDatabase, RedisValue[]> func, SerializeType? serializeType = null, int db = -1, Action<Exception> exceptionAction = null)
        {
            return Execute(func, db, exceptionAction).ToModelList<T>(serializeType ?? _defaultSerializeType);
        }
        #endregion

        #region Asynchronous execution
        /// <summary>
        /// Asynchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="db">Redis database index</param>
        /// <param name="exceptionAction">The delegate used to handle exception</param>
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
        /// Asynchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="serializeType">Serialization type</param>
        /// <param name="db">Redis database index</param>
        /// <param name="exceptionAction">The delegate used to handle exception</param>
        /// <returns></returns>
        public static async Task<T> ExecuteAsync<T>(Func<IDatabase, Task<RedisValue>> func, SerializeType? serializeType = null, int db = -1, Action<Exception> exceptionAction = null)
        {
            return (await ExecuteAsync(func, db, exceptionAction)).ToModel<T>(serializeType ?? _defaultSerializeType);
        }

        /// <summary>
        /// Asynchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="serializeType">Serialization type</param>
        /// <param name="db">Redis database index</param>
        /// <param name="exceptionAction">The delegate used to handle exception</param>
        /// <returns></returns>
        public static async Task<List<T>> ExecuteAsync<T>(Func<IDatabase, Task<RedisValue[]>> func, SerializeType? serializeType = null, int db = -1, Action<Exception> exceptionAction = null)
        {
            return (await ExecuteAsync(func, db, exceptionAction)).ToModelList<T>(serializeType ?? _defaultSerializeType); ;
        }
        #endregion

        /// <summary>
        /// Get the database of the specified index
        /// </summary>
        /// <param name="index">Redis database index</param>
        /// <returns></returns>
        public static IDatabase GetDatabase(int index = -1)
        {
            return _dbDic.GetOrAdd(index, key => _conn.GetDatabase(key));
        }
    }
}
