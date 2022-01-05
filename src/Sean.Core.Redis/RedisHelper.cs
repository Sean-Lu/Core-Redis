using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Sean.Core.Redis.Extensions;
using StackExchange.Redis;
#if NETSTANDARD
using Microsoft.Extensions.Configuration;
#endif

namespace Sean.Core.Redis
{
    /// <summary>
    /// Helper for redis cache (based on StackExchange.Redis: https://github.com/StackExchange/StackExchange.Redis)
    /// </summary>
    public partial class RedisHelper
    {
        private static readonly ConcurrentDictionary<int, IDatabase> _dbCacheDic = new ConcurrentDictionary<int, IDatabase>();

        #region Synchronous execution
        /// <summary>
        /// Synchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="db">Redis database index</param>
        /// <returns></returns>
        public static T Execute<T>(Func<IDatabase, T> func, int db = -1)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (typeof(T).BaseType == typeof(Task))
                throw new InvalidOperationException($"For asynchronous operations, please use the {nameof(ExecuteAsync)} method.");

            return func(GetDatabase(db));
        }

        /// <summary>
        /// Synchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="serializeType">Serialization type</param>
        /// <param name="db">Redis database index</param>
        /// <returns></returns>
        public static T Execute<T>(Func<IDatabase, RedisValue> func, SerializeType? serializeType = null, int db = -1)
        {
            return Execute(func, db).ToModel<T>(serializeType ?? RedisManager.DefaultSerializeType); ;
        }

        /// <summary>
        /// Synchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="serializeType">Serialization type</param>
        /// <param name="db">Redis database index</param>
        /// <returns></returns>
        public static List<T> Execute<T>(Func<IDatabase, RedisValue[]> func, SerializeType? serializeType = null, int db = -1)
        {
            return Execute(func, db).ToModelList<T>(serializeType ?? RedisManager.DefaultSerializeType);
        }
        #endregion

        #region Asynchronous execution
        /// <summary>
        /// Asynchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="db">Redis database index</param>
        /// <returns></returns>
        public static async Task<T> ExecuteAsync<T>(Func<IDatabase, Task<T>> func, int db = -1)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            return await func(GetDatabase(db));
        }

        /// <summary>
        /// Asynchronous execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The delegate used to handle the cache operation</param>
        /// <param name="serializeType">Serialization type</param>
        /// <param name="db">Redis database index</param>
        /// <returns></returns>
        public static async Task<T> ExecuteAsync<T>(Func<IDatabase, Task<RedisValue>> func, SerializeType? serializeType = null, int db = -1)
        {
            return (await ExecuteAsync(func, db)).ToModel<T>(serializeType ?? RedisManager.DefaultSerializeType);
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
        public static async Task<List<T>> ExecuteAsync<T>(Func<IDatabase, Task<RedisValue[]>> func, SerializeType? serializeType = null, int db = -1)
        {
            return (await ExecuteAsync(func, db)).ToModelList<T>(serializeType ?? RedisManager.DefaultSerializeType); ;
        }
        #endregion

        /// <summary>
        /// Get the database of the specified index
        /// </summary>
        /// <param name="index">Redis database index</param>
        /// <returns></returns>
        public static IDatabase GetDatabase(int index = -1)
        {
            return _dbCacheDic.GetOrAdd(index, key => RedisManager.Instance.GetDatabase(key));
        }
    }
}
