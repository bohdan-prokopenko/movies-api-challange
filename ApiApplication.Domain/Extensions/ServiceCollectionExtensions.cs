using ApiApplication.Domain.UseCases;

using Microsoft.Extensions.DependencyInjection;

namespace ApiApplication.Domain.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddUseCases(this IServiceCollection services) {
            return services.AddScoped<ICreateShowTimeUseCase, CreateShowTimeUseCase>()
                .AddScoped<ICreateReservationUseCase, CreateReservationUseCase>();
        }
    }
}
