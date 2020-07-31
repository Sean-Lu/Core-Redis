using System;
using System.Collections.Generic;
using System.Linq;
using Sean.Utility.Contracts;
using Sean.Utility.Format;
using Sean.Utility.Serialize;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    /// <summary>
    /// 缓存操作客户端基类
    /// </summary>
    public abstract class RedisClientBase
    {
        private static ConnectionMultiplexer _db;
        static RedisClientBase()
        {
            _db = RedisManager.Instance;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="endPoints">redis地址，示例：127.0.0.1:6379</param>
        /// <param name="password">redis密码，默认为null</param>
        /// <param name="logger">日志</param>
        protected static void Init(List<string> endPoints, string password = null, ILogger logger = null)
        {
            RedisManager.Init(endPoints, password, logger);

            if (endPoints != null && endPoints.Any())
            {
                RedisManager.InitFlag = true;
            }
            if (_db == null || RedisManager.InitFlag)
            {
                _db = RedisManager.Instance ?? throw new Exception("Init redis client fail.");
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">缓存操作的委托</param>
        /// <param name="db">数据库索引</param>
        /// <param name="exceptionAction">处理异常的委托</param>
        /// <returns></returns>
        protected static T Exec<T>(Func<IDatabase, T> func, int db = -1, Action<Exception> exceptionAction = null)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            try
            {
                return func(GetDatabase(db));
            }
            catch (Exception e)
            {
                exceptionAction?.Invoke(e);
                return default;
            }
        }

        /// <summary>
        /// 获取指定索引的数据库
        /// </summary>
        /// <param name="db">-1表示默认数据库</param>
        /// <returns></returns>
        protected static IDatabase GetDatabase(int db = -1)
        {
            return _db.GetDatabase(db);
        }

        #region 数据转换
        /// <summary>
        /// 对象转json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        protected internal static string ToJson<T>(T val)
        {
            return val as string ?? JsonHelper.Serialize(val);
        }
        /// <summary>
        /// 值转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="serializeType"></param>
        /// <returns></returns>
        protected internal static T ToModel<T>(RedisValue val, SerializeType serializeType = SerializeType.Json)
        {
            //return JsonHelper.Deserialize<T>(val);

            if (!val.HasValue)
                return default(T);
            if (typeof(T) == typeof(string))
                return (T)Convert.ChangeType(val, typeof(string));

            switch (serializeType)
            {
                case SerializeType.Binary:
                    return BinarySerializer.Deserialize<T>(val);
                case SerializeType.Json:
                    return JsonHelper.Deserialize<T>(val);
                default:
                    throw new NotSupportedException($"not supported for serialize type [{serializeType}]");
            }
        }

        /// <summary>
        /// 集合值转集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        protected internal static List<T> ToModelList<T>(RedisValue[] val)
        {
            return val?.Select(item => ToModel<T>(item)).ToList();
        }
        #endregion
    }
}
