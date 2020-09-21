using System;
using System.Collections.Generic;
using System.Linq;
using Sean.Utility.Format;
using Sean.Utility.Serialize;
using StackExchange.Redis;

namespace Sean.Core.Redis.Extensions
{
    public static class RedisValueExtensions
    {
        private static BinarySerializer _binarySerializer = new BinarySerializer();

        /// <summary>
        /// 转换为<see cref="RedisValue"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="serializeType"></param>
        /// <returns></returns>
        public static RedisValue ToRedisValue<T>(this T model, SerializeType serializeType = SerializeType.Json)
        {
            if (model == null)
                return default;

            if (model is string)
                return model as string;

            switch (serializeType)
            {
                case SerializeType.Binary:
                    return _binarySerializer.Serialize(model);
                case SerializeType.Json:
                    return JsonHelper.Serialize(model);
                default:
                    throw new NotSupportedException($"not supported for serialize type [{serializeType}]");
            }
        }

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="serializeType"></param>
        /// <returns></returns>
        public static T ToModel<T>(this RedisValue value, SerializeType serializeType = SerializeType.Json)
        {
            if (!value.HasValue)
                return default;
            if (typeof(T) == typeof(string))
                return (T)Convert.ChangeType(value, typeof(string));

            switch (serializeType)
            {
                case SerializeType.Binary:
                    return _binarySerializer.Deserialize<T>(value);
                case SerializeType.Json:
                    return JsonHelper.Deserialize<T>(value);
                default:
                    throw new NotSupportedException($"not supported for serialize type [{serializeType}]");
            }
        }

        /// <summary>
        /// 转换为对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="serializeType"></param>
        /// <returns></returns>
        public static List<T> ToModelList<T>(this RedisValue[] value, SerializeType serializeType = SerializeType.Json)
        {
            return value?.Select(item => ToModel<T>(item, serializeType)).ToList();
        }
    }
}
