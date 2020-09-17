using System;
using System.Collections.Generic;
using System.Linq;
using Sean.Utility.Format;
using Sean.Utility.Serialize;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    public abstract class RedisClientBase
    {
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
