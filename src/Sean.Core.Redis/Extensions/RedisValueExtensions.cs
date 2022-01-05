using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Sean.Core.Redis.Extensions
{
    /// <summary>
    /// Extension of <see cref="RedisValue"/>
    /// </summary>
    public static class RedisValueExtensions
    {
        private static readonly BinarySerializer BinarySerializer = new BinarySerializer();

        /// <summary>
        /// Convert to <see cref="RedisValue"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="serializeType">Serialization type</param>
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
                    return BinarySerializer.Serialize(model);
                case SerializeType.Json:
                    return JsonConvert.SerializeObject(model);//RedisValue.Unbox(xxx);
                default:
                    throw new NotSupportedException($"Not yet supported for serialize type [{serializeType}]");
            }
        }

        /// <summary>
        /// Convert to <see cref="RedisValue"/> array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="serializeType">Serialization type</param>
        /// <returns></returns>
        public static RedisValue[] ToRedisValueArray<T>(this IList<T> model, SerializeType serializeType = SerializeType.Json)
        {
            return model?.Select(c => c.ToRedisValue(serializeType)).ToArray();
        }

        /// <summary>
        /// Convert to model object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="serializeType">Serialization type</param>
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
                    return BinarySerializer.Deserialize<T>(value);
                case SerializeType.Json:
                    return JsonConvert.DeserializeObject<T>(value);
                default:
                    throw new NotSupportedException($"Not yet supported for serialize type [{serializeType}]");
            }
        }

        /// <summary>
        /// Convert to model object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="serializeType">Serialization type</param>
        /// <returns></returns>
        public static List<T> ToModelList<T>(this RedisValue[] value, SerializeType serializeType = SerializeType.Json)
        {
            return value?.Select(item => ToModel<T>(item, serializeType)).ToList();
        }
    }
}
