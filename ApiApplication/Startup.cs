using ApiApplication.Database;
using ApiApplication.Database.Repositories;
using ApiApplication.Domain.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
            _ = services.AddTransient<IShowtimesRepository, ShowtimesRepository>()
                .AddTransient<ITicketsRepository, TicketsRepository>()
                .AddTransient<IAuditoriumsRepository, AuditoriumsRepository>()
                .AddHttpClient()
                .AddDbContext<CinemaContext>(options => {
                    _ = options.UseInMemoryDatabase("CinemaDb")
                        .EnableSensitiveDataLogging()
                        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                }).AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                _ = app.UseDeveloperExceptionPage();
            }

            _ = app.UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints => {
                    _ = endpoints.MapControllers();
                });
        }
    }
}
