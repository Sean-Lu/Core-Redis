namespace Sean.Core.Redis;

/// <summary>
/// Redis configuration
/// </summary>
public class RedisClientOptions
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
    /// Default serialization type (default value is <see cref="SerializeType.Json"/>)
    /// </summary>
    public SerializeType DefaultSerializeType { get; set; } = SerializeType.Json;
}