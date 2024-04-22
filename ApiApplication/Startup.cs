using ApiApplication.Database;
using ApiApplication.Database.Repositories;
using ApiApplication.Database.Repositories.Abstractions;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            _ = services.AddTransient<IShowtimesRepository, ShowtimesRepository>();
            _ = services.AddTransient<ITicketsRepository, TicketsRepository>();
            _ = services.AddTransient<IAuditoriumsRepository, AuditoriumsRepository>();

            _ = services.AddDbContext<CinemaContext>(options => {
                _ = options.UseInMemoryDatabase("CinemaDb")
                    .EnableSensitiveDataLogging()
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            _ = services.AddControllers();
            _ = services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                _ = app.UseDeveloperExceptionPage();
            }

            //_ = app.UseHttpsRedirection();

            _ = app.UseRouting();

            //_ = app.UseAuthorization();

            _ = app.UseEndpoints(endpoints => {
                _ = endpoints.MapControllers();
            });
        }
    }
}
