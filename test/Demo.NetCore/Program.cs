using System;
using System.Collections.Generic;
using Sean.Core.Redis.StackExchange;

namespace Demo.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var endPoint = "127.0.0.1:6379";
            RedisHelper.Init(new List<string> { endPoint });

            var cacheKey = "test";
            RedisHelper.StringSet(cacheKey, "123456", TimeSpan.FromSeconds(10));

            var cacheVal = RedisHelper.StringGet<string>(cacheKey);
            Console.WriteLine(cacheVal);

            Console.ReadLine();
        }
    }
}
