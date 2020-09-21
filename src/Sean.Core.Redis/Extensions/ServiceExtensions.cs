#if NETSTANDARD
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sean.Core.Redis.StackExchange;

namespace Sean.Core.Redis.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, Action<RedisConfigOptions> config)
        {
            RedisHelper.Init(config);
            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration, Action<RedisConfigOptions> config = null)
        {
            RedisHelper.Init(options =>
            {
                GetRedisConfig(configuration, options);
                config?.Invoke(options);
            });
            return services;
        }

        private static void GetRedisConfig(IConfiguration configuration, RedisConfigOptions config)
        {
            var endPoint = configuration.GetValue("Redis:endPoints", string.Empty);
            var pwd = configuration.GetValue("Redis:pwd", string.Empty);

            config.EndPoints = endPoint;
            config.Password = pwd;
        }
    }
}
#endif