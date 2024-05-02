using ApiApplication.Client.Configuration;
using ApiApplication.Client.Grpc;
using ApiApplication.Client.Http;
using ApiApplication.Domain.Api;

using Microsoft.Extensions.DependencyInjection;

using System;

using static ApiApplication.Client.Http.MoviesClientApi;

namespace ApiApplication.Client.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddMoviesHttpApiClient(this IServiceCollection services, Func<ApiClientConfigurationBuilder, IApiClientConfiguration> builder) {
            IApiClientConfiguration config = builder(new ApiClientConfigurationBuilder());
            _ = services.AddTransient<IMoviesClientApi, MoviesClientApi>()
                .AddHttpClient(ClientName, client => {
                    client.BaseAddress = new Uri(config.Address);
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Add("X-Apikey", config.ApiKey);
                });
            return services;
        }

        public static IServiceCollection AddMoviesGrpcApiClient(this IServiceCollection services, Func<ApiClientConfigurationBuilder, IApiClientConfiguration> builder) {
            return services.AddSingleton(builder(new ApiClientConfigurationBuilder()))
                .AddTransient<IMoviesClientApi, ApiClientGrpc>();
        }
    }
}
