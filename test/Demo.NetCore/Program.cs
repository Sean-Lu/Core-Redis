using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sean.Core.Ioc;
using Sean.Core.Redis.Extensions;
using Sean.Core.Redis.StackExchange;
using Sean.Utility.Contracts;
using Sean.Utility.Impls.Log;

namespace Demo.NetCore
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
            //    options.Logger = new SimpleLocalLogger<Program>();
            //});
            #endregion

            #region 1. Redis初始化：使用DI注入（推荐）
            ServiceManager.ConfigureServices(services =>
            {
                services.AddTransient(typeof(ISimpleLogger<>), typeof(SimpleLocalLogger<>));
            });
            ServiceManager.ConfigureServices(services =>
            {
                var configuration = ServiceManager.GetService<IConfiguration>();
                var logger = ServiceManager.GetService<ISimpleLogger<Program>>();
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
            var cacheKey = "test";
            RedisHelper.StringSet(cacheKey, "123456", TimeSpan.FromSeconds(10));

            Console.WriteLine(RedisHelper.StringGet<string>(cacheKey));// 同步获取
            Console.WriteLine(RedisHelper.StringGetAsync<string>(cacheKey).Result);// 异步获取
            #endregion

            Console.ReadLine();
        }
    }
}
