using ApiApplication.Caching.Configuration;
using ApiApplication.Caching.Repositories;
using ApiApplication.Domain.Repositories;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ApiApplication.Caching.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddCaching(this IServiceCollection services, Func<CacheConfigurationBuilder, ICacheConfiguration> builder) {
            return services.AddStackExchangeRedisCache(options => {
                ICacheConfiguration config = builder(new CacheConfigurationBuilder());
                options.Configuration = config.Configuration;
                options.InstanceName = config.Instance;
            }).AddTransient<ICacheRepository, RedisCacheRepository>();
        }
    }
}
