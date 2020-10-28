using System;
using System.Configuration;
using System.Linq;
using Sean.Utility.Contracts;
using StackExchange.Redis;

namespace Sean.Core.Redis
{
    internal class RedisManager
    {
        private static ILogger _logger;
        private static readonly object _syncLock = new object();
        private static ConnectionMultiplexer _instance;
        private static ConfigurationOptions _option;

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
                            _instance = Connect();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="options">redis configuration</param>
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
        /// Create Redis connection
        /// </summary>
        /// <returns></returns>
        private static ConnectionMultiplexer Connect()
        {
            if (_option == null)
            {
                return null;
            }

            var connect = ConnectionMultiplexer.Connect(_option);

            connect.InternalError += InternalError;
            connect.ErrorMessage += ErrorMessage;
            connect.ConnectionFailed += ConnectionFailed;
            connect.ConnectionRestored += ConnectionRestored;
            connect.ConfigurationChanged += ConfigurationChanged;
            connect.HashSlotMoved += HashSlotMoved;

            return connect;
        }

        #region Redis EventHandler
        private static void InternalError(object sender, InternalErrorEventArgs e)
        {
            _logger?.LogError($"InternalError: {e.Exception?.Message}", e.Exception);
        }

        private static void HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            _logger?.LogInfo($"HashSlotMoved => NewEndPoint: {e.NewEndPoint}, OldEndPoint: {e.OldEndPoint}");
        }

        private static void ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            _logger?.LogInfo($"ConfigurationChanged: {e.EndPoint}");
        }

        private static void ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            _logger?.LogError($"ErrorMessage: {e.Message}");
        }

        private static void ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            _logger?.LogError($"ConnectionRestored: {e.EndPoint}", e.Exception);
        }

        private static void ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            _logger?.LogError($"ConnectionFailed: {e.EndPoint}, FailureType: {e.FailureType}", e.Exception);
        }
        #endregion
    }
}
