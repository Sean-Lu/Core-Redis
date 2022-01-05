using System;
using System.Collections.Generic;
using Example.NetCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sean.Core.Ioc;
using Sean.Core.Redis;
using Sean.Core.Redis.Extensions;
using Sean.Utility.Contracts;
using Sean.Utility.Extensions;
using Sean.Utility.Format;
using Sean.Utility.Impls.Log;
using StackExchange.Redis;
using StackExchange.Redis.Maintenance;

namespace Example.NetCore
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            IocContainer.Instance.ConfigureServices((services, configuration) =>
            {
                services.AddTransient(typeof(ISimpleLogger<>), typeof(SimpleLocalLogger<>));// Logger 日志
            });

            SimpleLocalLoggerBase.DateTimeFormat = time => time.ToLongDateTime();

            _logger = IocContainer.Instance.GetService<ISimpleLogger<Program>>();//SimpleLocalLoggerManager.GetCurrentClassLogger();
            _logger.LogInfo("Redis使用示例");

            #region 1. Redis初始化

            #region 方式1：不使用依赖注入
            //// 示例1：
            //var configuration = IocContainer.Instance.GetService<IConfiguration>();
            //RedisManager.Initialize(configuration);

            //// 示例2：
            //RedisManager.Initialize(new RedisClientOptions
            //{
            //    EndPoints = "127.0.0.1:6379",
            //    Password = string.Empty,
            //    DefaultSerializeType = SerializeType.Json
            //});
            #endregion

            #region 方式2：使用依赖注入（推荐）
            IocContainer.Instance.ConfigureServices((services, configuration) =>
            {
                services.AddRedis(configuration);
            });
            #endregion

            //RedisManager.Options.ConnectTimeout = 5000;
            //RedisManager.Options.SyncTimeout = 5000;
            //RedisManager.Options.AsyncTimeout = 5000;

            #endregion

            #region 2. Redis使用示例

            #region Redis EventHandler
            RedisManager.Instance.InternalError += InternalError;
            RedisManager.Instance.ErrorMessage += ErrorMessage;
            RedisManager.Instance.ConnectionFailed += ConnectionFailed;
            RedisManager.Instance.ConnectionRestored += ConnectionRestored;
            RedisManager.Instance.ConfigurationChanged += ConfigurationChanged;
            RedisManager.Instance.ConfigurationChangedBroadcast += ConfigurationChangedBroadcast;
            RedisManager.Instance.HashSlotMoved += HashSlotMoved;
            RedisManager.Instance.ServerMaintenanceEvent += ServerMaintenanceEvent;
            #endregion

            #region string => 最简单的用法
            var cacheKey = "test";
            Console.WriteLine($"添加string缓存：{RedisHelper.StringSet(cacheKey, new TestModel { Id = 1001, Name = "Sean" }, TimeSpan.FromSeconds(20))}");
            Console.WriteLine($"同步获取缓存：{JsonConvert.SerializeObject(RedisHelper.StringGet<TestModel>(cacheKey))}");
            Console.WriteLine($"异步获取缓存：{JsonConvert.SerializeObject(RedisHelper.StringGetAsync<TestModel>(cacheKey).Result)}");
            Console.WriteLine($"手动删除缓存：{RedisHelper.KeyDelete(cacheKey)}");
            #endregion

            #region list => 实现队列（先进先出）
            var cacheKeyListQueue = "testListQueue";
            var list = new List<TestModel>
            {
                new TestModel { Id = 1002, Name = "aaa" },
                new TestModel { Id = 1003, Name = "bbb" },
                new TestModel { Id = 1004, Name = "ccc" },
            };
            Console.WriteLine($"添加list缓存：{RedisHelper.ListRightPush(cacheKeyListQueue, list)}");
            Console.WriteLine($"设置缓存超时时间：{RedisHelper.KeyExpire(cacheKeyListQueue, TimeSpan.FromSeconds(20))}");
            Console.WriteLine($"获取缓存：{JsonConvert.SerializeObject(RedisHelper.ListLeftPop<TestModel>(cacheKeyListQueue))}");
            Console.WriteLine($"获取缓存：{JsonConvert.SerializeObject(RedisHelper.ListLeftPop<TestModel>(cacheKeyListQueue))}");
            Console.WriteLine($"获取缓存：{JsonConvert.SerializeObject(RedisHelper.ListLeftPop<TestModel>(cacheKeyListQueue))}");
            #endregion

            #region list => 实现堆栈（先进后出）
            var cacheKeyListStack = "testListStack";
            Console.WriteLine($"添加list缓存：{RedisHelper.ListLeftPush(cacheKeyListStack, list)}");
            Console.WriteLine($"设置缓存超时时间：{RedisHelper.KeyExpire(cacheKeyListStack, TimeSpan.FromSeconds(20))}");
            Console.WriteLine($"获取缓存：{JsonConvert.SerializeObject(RedisHelper.ListLeftPop<TestModel>(cacheKeyListStack))}");
            Console.WriteLine($"获取缓存：{JsonConvert.SerializeObject(RedisHelper.ListLeftPop<TestModel>(cacheKeyListStack))}");
            Console.WriteLine($"获取缓存：{JsonConvert.SerializeObject(RedisHelper.ListLeftPop<TestModel>(cacheKeyListStack))}");
            #endregion

            #endregion

            Console.ReadLine();
        }

        #region Redis EventHandler
        private static void InternalError(object sender, InternalErrorEventArgs e)
        {
            _logger.LogError($"[Redis]InternalError: {e.Exception?.Message}", e.Exception);
        }

        private static void ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            _logger.LogError($"[Redis]ErrorMessage: {e.Message}");
        }

        private static void ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            _logger.LogError($"[Redis]ConnectionFailed: {e.EndPoint}, FailureType: {e.FailureType}", e.Exception);
        }

        private static void ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            _logger.LogInfo($"[Redis]ConnectionRestored: {e.EndPoint}", e.Exception);
        }

        private static void ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            _logger.LogInfo($"[Redis]ConfigurationChanged: {e.EndPoint}");
        }

        private static void ConfigurationChangedBroadcast(object? sender, EndPointEventArgs e)
        {
            _logger.LogInfo($"[Redis]ConfigurationChangedBroadcast: {e.EndPoint}");
        }

        private static void HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            _logger.LogInfo($"[Redis]HashSlotMoved => NewEndPoint: {e.NewEndPoint}, OldEndPoint: {e.OldEndPoint}");
        }

        private static void ServerMaintenanceEvent(object? sender, ServerMaintenanceEvent e)
        {
            _logger.LogInfo($"[Redis]ServerMaintenanceEvent => ReceivedTimeUtc: {e.ReceivedTimeUtc}, RawMessage: {e.RawMessage}");
        }
        #endregion
    }
}
