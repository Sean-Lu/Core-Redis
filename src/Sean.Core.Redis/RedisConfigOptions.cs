using Sean.Utility.Contracts;

namespace Sean.Core.Redis
{
    /// <summary>
    /// Redis configuration
    /// </summary>
    public class RedisConfigOptions
    {
        /// <summary>
        /// Redis address (cluster addresses are separated by ","), example: 127.0.0.1:6379
        /// </summary>
        public string EndPoints { get; set; }
        /// <summary>
        /// Redis password (default value is null)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Redis connection timeout (unit: milliseconds, default: 5 seconds)
        /// </summary>
        public int? ConnectTimeout { get; set; }
        /// <summary>
        /// Redis synchronization operation timeout (unit: milliseconds, default: 5 seconds)
        /// </summary>
        public int? SyncTimeout { get; set; }
        /// <summary>
        /// Redis asynchronous operation timeout (unit: milliseconds, default: 5 seconds)
        /// </summary>
        public int? AsyncTimeout { get; set; }

        /// <summary>
        /// Default serialization type (default value is <see cref="DefaultSerializeType.Json"/>)
        /// </summary>
        public SerializeType DefaultSerializeType { get; set; } = SerializeType.Json;

        /// <summary>
        /// Log
        /// </summary>
        public ILogger Logger { get; set; }
    }
}
