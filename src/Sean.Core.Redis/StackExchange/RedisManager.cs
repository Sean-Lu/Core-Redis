using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Sean.Utility.Contracts;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    internal class RedisManager
    {
        /// <summary>
        /// 初始化标志
        /// </summary>
        public static bool InitFlag { get; set; }

        private static ISimpleLogger _logger;
        private static readonly object locker = new object();
        private static ConnectionMultiplexer _instance;
        private static ConfigurationOptions _option;

        /// <summary>
        /// redis连接超时时间
        /// </summary>
        private static int? _connectTimeout;

        static RedisManager()
        {
            Init(null);
        }

        /// <summary>
        /// 通过线程安全的单例模式来获取redis连接实例
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null || InitFlag)
                {
                    lock (locker)
                    {
                        if (_instance == null || InitFlag)
                        {
                            if (_instance != null && InitFlag)
                            {
                                // 释放资源
                                _instance.Dispose();
                                _instance = null;
                                _logger?.LogInfo($"重新初始化Redis连接，释放之前的{nameof(ConnectionMultiplexer)}资源");
                            }

                            _instance = GetManager();

                            InitFlag = false;
                        }
                    }
                }
                return _instance;
            }
        }

        internal static void Init(List<string> endPoints, string password = null, ISimpleLogger logger = null)
        {
            _logger = logger;

            if (endPoints == null || !endPoints.Any())
            {
                if (_option == null)
                {
                    var endPoint = ConfigurationManager.AppSettings["RedisServer"];
                    var pwd = ConfigurationManager.AppSettings["RedisPwd"];

                    if (!string.IsNullOrWhiteSpace(endPoint))
                    {
                        _option = new ConfigurationOptions
                        {
                            EndPoints = { endPoint },
                            Password = pwd
                        };

                        if (_connectTimeout.HasValue)
                        {
                            _option.ConnectTimeout = _connectTimeout.Value;
                        }
                    }
                }
            }
            else
            {
                if (_option == null)
                {
                    _option = new ConfigurationOptions();
                }

                _option.EndPoints.Clear();
                endPoints.ForEach(c =>
                {
                    _option.EndPoints.Add(c);
                });
                _option.Password = password;
            }
        }

        /// <summary>
        /// 创建Redis连接
        /// </summary>
        /// <returns></returns>
        private static ConnectionMultiplexer GetManager()
        {
            if (_option == null)
            {
                return null;
            }

            var connect = ConnectionMultiplexer.Connect(_option);

            #region 注册事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;
            #endregion

            return connect;
        }

        #region Redis事件
        /// <summary>
        /// 内部异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            _logger?.LogError($"内部异常：{e.Exception?.Message}", e.Exception);
        }

        /// <summary>
        /// 集群更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            _logger?.LogInfo($"集群更改【新集群：{e.NewEndPoint}，旧集群：{e.OldEndPoint}】");
        }

        /// <summary>
        /// 配置更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            _logger?.LogInfo($"配置更改：{e.EndPoint}");
        }

        /// <summary>
        /// 错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            _logger?.LogError($"异常信息：{e.Message}");
        }

        /// <summary>
        /// 重连错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            _logger?.LogError($"重连错误：{e.EndPoint}", e.Exception);
        }

        /// <summary>
        /// 连接失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            _logger?.LogError($"连接失败：{e.EndPoint}，ConnectionFailureType：{e.FailureType}", e.Exception);
        }
        #endregion
    }
}
