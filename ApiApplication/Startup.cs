using ApiApplication.Caching.Extensions;
using ApiApplication.Client.Extensions;
using ApiApplication.Conventions;
using ApiApplication.Domain.Extensions;
using ApiApplication.Middleware;
using ApiApplication.Repository.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Net;

namespace ApiApplication {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration {
            get;
        }

        public void ConfigureServices(IServiceCollection services) {
            IConfigurationSection redisConfig = Configuration.GetSection("RedisCacheOptions");
            IConfigurationSection grpcConfig = Configuration.GetSection("gRPC");
            _ = services
                    .AddUseCases()
                    .AddCaching(builder => builder.Configuration(redisConfig.GetValue<string>("Configuration"))
                        .Instance(redisConfig.GetValue<string>("Instance"))
                        .Build())
            .AddRepository()
                    .AddMoviesGrpcApiClient(builder => builder.Address(grpcConfig.GetValue<string>("Address")).ApiKey(grpcConfig.GetValue<string>("ApiKey")).Build())
                    .AddControllersWithViews(options =>
                    {
                        options.Conventions.Add(new DynamicRouteConvention());
                    });
            ;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            _ = app.UseMiddleware<ErrorHandlingMiddleware>()
                .UseMiddleware<RequestTimeMiddleware>();

            _ = app.UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints => {
                    _ = endpoints.MapControllers();
                });

            _ = app.UseSampleData();
        }
    }
}
