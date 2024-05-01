using ApiApplication.Caching.Extensions;
using ApiApplication.Domain.Extensions;
using ApiApplication.Middleware;
using ApiApplication.Repository.Extensions;
using ApiApplication.Client.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            _ = services
                    .AddUseCases()
                    .AddCaching(builder => builder.Configuration(redisConfig.GetValue<string>("Configuration"))
                        .Instance(redisConfig.GetValue<string>("Instance"))
                        .Build())
                    .AddRepository()
                    .AddMoviesApiClient()
                    .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            _ = app.UseMiddleware<ErrorHandlingMiddleware>();

            if (env.IsDevelopment()) {
                _ = app.UseDeveloperExceptionPage();
            }

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
