using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sean.Core.Redis.StackExchange;

namespace Demo.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var endPoint = "127.0.0.1:6379";
            RedisHelper.Init(new RedisConfigOptions
            {
                EndPoints = endPoint
            });

            var cacheKey = "test";
            RedisHelper.StringSet(cacheKey, "123456", TimeSpan.FromSeconds(10));

            Console.WriteLine(RedisHelper.StringGet<string>(cacheKey));
            Console.WriteLine(RedisHelper.StringGetAsync<string>(cacheKey).Result);

            Console.ReadLine();
        }
    }
}
