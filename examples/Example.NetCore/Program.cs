using System;
using System.Collections.Generic;
using Example.NetCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sean.Core.Ioc;
using Sean.Core.Redis;
using Sean.Core.Redis.Extensions;
using Sean.Utility.Contracts;
using Sean.Utility.Format;
using Sean.Utility.Impls.Log;

namespace Example.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 1. Redis初始化：不使用DI注入
            //var endPoint = "127.0.0.1:6379";
            //RedisHelper.Init(options =>
            //{
            //    options.EndPoints = endPoint;
            //    options.ConnectTimeout = 10000;
            //    options.SyncTimeout = 15000;
            //    options.AsyncTimeout = 15000;
            //    options.Logger = new SimpleLocalLogger<RedisHelper>();
            //});
            #endregion

            #region 1. Redis初始化：使用DI注入（推荐）
            IocContainer.Instance.ConfigureServices(services =>
            {
                services.AddTransient(typeof(ISimpleLogger<>), typeof(SimpleLocalLogger<>));
            });
            IocContainer.Instance.ConfigureServices(services =>
            {
                var configuration = IocContainer.Instance.GetService<IConfiguration>();
                var logger = IocContainer.Instance.GetService<ISimpleLogger<RedisHelper>>();
                services.AddRedis(configuration, options =>
                {
                    options.ConnectTimeout = 10000;
                    options.SyncTimeout = 15000;
                    options.AsyncTimeout = 15000;
                    options.Logger = logger;
                });
            });
            #endregion

            #region 2. Redis使用示例

            #region string => 最简单的用法
            var cacheKey = "test";
            Console.WriteLine($"添加string缓存：{RedisHelper.StringSet(cacheKey, new TestModel { Id = 1001, Name = "Sean" }, TimeSpan.FromSeconds(20))}");
            Console.WriteLine($"同步获取缓存：{JsonHelper.Serialize(RedisHelper.StringGet<TestModel>(cacheKey))}");
            Console.WriteLine($"异步获取缓存：{JsonHelper.Serialize(RedisHelper.StringGetAsync<TestModel>(cacheKey).Result)}");
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
            Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<TestModel>(cacheKeyListQueue))}");
            Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<TestModel>(cacheKeyListQueue))}");
            Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<TestModel>(cacheKeyListQueue))}");
            #endregion

            #region list => 实现堆栈（先进后出）
            var cacheKeyListStack = "testListStack";
            Console.WriteLine($"添加list缓存：{RedisHelper.ListLeftPush(cacheKeyListStack, list)}");
            Console.WriteLine($"设置缓存超时时间：{RedisHelper.KeyExpire(cacheKeyListStack, TimeSpan.FromSeconds(20))}");
            Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<TestModel>(cacheKeyListStack))}");
            Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<TestModel>(cacheKeyListStack))}");
            Console.WriteLine($"获取缓存：{JsonHelper.Serialize(RedisHelper.ListLeftPop<TestModel>(cacheKeyListStack))}");
            #endregion

            #endregion

            Console.ReadLine();
        }
    }
}
