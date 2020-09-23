using System.Collections.Generic;
using Sean.Utility.Contracts;

namespace Sean.Core.Redis.StackExchange
{
    /// <summary>
    /// redis配置
    /// </summary>
    public class RedisConfigOptions
    {
        /// <summary>
        /// redis地址（集群地址用","分隔），示例：127.0.0.1:6379
        /// </summary>
        public string EndPoints { get; set; }
        /// <summary>
        /// redis密码（默认：null）
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// redis连接超时时间（单位：毫秒，默认：5秒）
        /// </summary>
        public int? ConnectTimeout { get; set; }
        /// <summary>
        /// redis同步操作超时时间（单位：毫秒，默认：5秒）
        /// </summary>
        public int? SyncTimeout { get; set; }
        /// <summary>
        /// redis异步操作超时时间（单位：毫秒，默认：5秒）
        /// </summary>
        public int? AsyncTimeout { get; set; }

        /// <summary>
        /// 序列化类型（默认：<see cref="SerializeType.Json"/>）
        /// </summary>
        public SerializeType SerializeType { get; set; } = SerializeType.Json;

        /// <summary>
        /// 日志
        /// </summary>
        public ILogger Logger { get; set; }

    }
}
