using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.NetCore
{
    public static class IocContainer
    {
        private static IConfiguration _configuration;
        private static IServiceCollection _services;
        private static IServiceProvider _serviceProvider;

        public static void ConfigureServices(Action<IServiceCollection, IConfiguration> configServices)
        {
            if (_services != null)
            {
                configServices(_services, _configuration);
                _serviceProvider = _services.BuildServiceProvider();
                return;
            }

            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            _services = new ServiceCollection();
            _services.AddSingleton<IConfiguration>(_configuration);
            configServices(_services, _configuration);

            _serviceProvider = _services.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
