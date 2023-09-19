using System;
using System.Configuration;
using System.Linq;
using StackExchange.Redis;
#if NETSTANDARD
using Microsoft.Extensions.Configuration;
#endif

namespace Sean.Core.Redis;

public class RedisManager
{
    /// <summary>
    /// Obtain a redis connection instance (thread safety)
    /// </summary>
    public static ConnectionMultiplexer Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = Connect(_options);
                    }
                }
            }
            return _instance;
        }
    }

    public static ConfigurationOptions Options => _options;

    /// <summary>
    /// Default serialization type
    /// </summary>
    public static SerializeType DefaultSerializeType => _defaultSerializeType;

    private static ConnectionMultiplexer _instance;
    private static ConfigurationOptions _options;
    private static SerializeType _defaultSerializeType;
    private static readonly object _syncLock = new object();

    #region Initialization
#if NETSTANDARD
    /// <summary>
    /// 通过配置文件初始化
    /// </summary>
    /// <param name="configuration"></param>
    public static void Initialize(IConfiguration configuration)
    {
        var endPoints = configuration.GetValue("Redis:EndPoints", string.Empty);
        var pwd = configuration.GetValue("Redis:Password", string.Empty);
        var defaultSerializeType = configuration.GetValue("Redis:DefaultSerializeType", SerializeType.Json);
        var redisConfig = new RedisClientOptions
        {
            EndPoints = endPoints,
            Password = pwd,
            DefaultSerializeType = defaultSerializeType
        };
        Initialize(redisConfig);
    }
#else
    /// <summary>
    /// 通过配置文件初始化
    /// </summary>
    public static void Initialize()
    {
        var endPoints = ConfigurationManager.AppSettings["RedisEndPoints"];
        var pwd = ConfigurationManager.AppSettings["RedisPassword"];
        Enum.TryParse<SerializeType>(ConfigurationManager.AppSettings["RedisDefaultSerializeType"], out var defaultSerializeType);
        var redisConfig = new RedisClientOptions
        {
            EndPoints = endPoints,
            Password = pwd,
            DefaultSerializeType = defaultSerializeType
        };
        Initialize(redisConfig);
    }
#endif

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="redisConfig"></param>
    public static void Initialize(RedisClientOptions redisConfig)
    {
        if (redisConfig == null) throw new ArgumentNullException(nameof(redisConfig));

        Initialize(options =>
        {
            redisConfig.EndPoints?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(c =>
            {
                var hostAndPort = c?.Trim();
                if (!string.IsNullOrWhiteSpace(hostAndPort))
                {
                    options.EndPoints.Add(hostAndPort);
                }
            });
            options.Password = redisConfig.Password;
        }, redisConfig.DefaultSerializeType);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="options"></param>
    /// <param name="defaultSerializeType"></param>
    public static void Initialize(Action<ConfigurationOptions> options, SerializeType defaultSerializeType = SerializeType.Json)
    {
        _options = new ConfigurationOptions();
        _options.EndPoints.Clear();
        options?.Invoke(_options);
        _defaultSerializeType = defaultSerializeType;
    }
    #endregion

    public static ConnectionMultiplexer Connect(ConfigurationOptions options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

        var connect = ConnectionMultiplexer.Connect(options);
        //connect.InternalError += InternalError;
        //connect.ErrorMessage += ErrorMessage;
        //connect.ConnectionFailed += ConnectionFailed;
        //connect.ConnectionRestored += ConnectionRestored;
        //connect.ConfigurationChanged += ConfigurationChanged;
        //connect.HashSlotMoved += HashSlotMoved;
        return connect;
    }
}