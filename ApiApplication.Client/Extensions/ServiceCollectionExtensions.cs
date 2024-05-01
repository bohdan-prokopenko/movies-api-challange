using ApiApplication.Client.Http;
using ApiApplication.Domain.Api;

using Microsoft.Extensions.DependencyInjection;

using System;

using static ApiApplication.Client.Http.MoviesClientApi;

namespace ApiApplication.Client.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddMoviesApiClient(this IServiceCollection services) {
            _ = services.AddTransient<IMoviesClientApi, MoviesClientApi>()
                .AddHttpClient(ClientName, client => {
                    client.BaseAddress = new Uri("http://api");
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Add("X-Apikey", "68e5fbda-9ec9-4858-97b2-4a8349764c63");
                });
            return services;
        }
    }
}
