using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Sean.Utility.Contracts;
using StackExchange.Redis;

namespace Sean.Core.Redis.StackExchange
{
    internal class RedisManager
    {
        private static ILogger _logger;
        private static readonly object locker = new object();
        private static ConnectionMultiplexer _instance;
        private static ConfigurationOptions _option;

        /// <summary>
        /// 获取redis连接实例（线程安全）
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                        {
                            _instance = GetManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public static void Init(RedisConfigOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _option = new ConfigurationOptions();

            if (string.IsNullOrWhiteSpace(options.EndPoints))
            {
                var endPoints = ConfigurationManager.AppSettings["RedisServer"];
                var pwd = ConfigurationManager.AppSettings["RedisPwd"];

                if (!string.IsNullOrWhiteSpace(endPoints))
                {
                    options.EndPoints = endPoints;
                    options.Password = pwd;
                }
            }

            _option.EndPoints.Clear();
            options.EndPoints?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToList().ForEach(c =>
            {
                if (!string.IsNullOrWhiteSpace(c))
                {
                    _option.EndPoints.Add(c);
                }
            });
            _option.Password = options.Password;

            if (options.ConnectTimeout.HasValue)
            {
                _option.ConnectTimeout = options.ConnectTimeout.Value;
            }
            if (options.SyncTimeout.HasValue)
            {
                _option.SyncTimeout = options.SyncTimeout.Value;
            }
            if (options.AsyncTimeout.HasValue)
            {
                _option.AsyncTimeout = options.AsyncTimeout.Value;
            }

            _logger = options.Logger;
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
