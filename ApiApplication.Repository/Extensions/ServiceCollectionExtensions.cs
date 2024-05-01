using ApiApplication.Domain.Repositories;
using ApiApplication.Repository.Context;
using ApiApplication.Repository.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ApiApplication.Repository.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddRepository(this IServiceCollection services) {
            _ = services.AddTransient<IShowtimesRepository, ShowtimesRepository>()
                    .AddTransient<ITicketsRepository, TicketsRepository>()
                    .AddTransient<IAuditoriumsRepository, AuditoriumsRepository>()
                    .AddTransient<IMoviesRepository, MoviesRepository>()
                    .AddDbContext<CinemaContext>(options => options.UseInMemoryDatabase("CinemaDb")
                    .EnableSensitiveDataLogging()
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            return services;
        }
    }
}
