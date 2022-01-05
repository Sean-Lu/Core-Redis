#if NETSTANDARD
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sean.Core.Redis.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            RedisManager.Initialize(configuration);
            return services;
        }
    }
}
#endif