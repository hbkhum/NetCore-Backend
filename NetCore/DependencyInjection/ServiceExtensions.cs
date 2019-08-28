using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetCore.Infrastructure.Interfaces;
using NetCore.Repositories;
using NetCore.Services;
using NetCore.Services.Interfaces;

namespace NetCore.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static  IServiceCollection RegisterRepositories(this IServiceCollection repositories)
        {

            repositories.TryAddTransient<ICarRepository, CarRepository>();
            repositories.TryAddTransient<ITruckRepository, TruckRepository>();

            // Add all other services here.
            return repositories;
        }

        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {
            services.TryAddTransient<ICarService, CarService>();
            services.TryAddTransient<ITruckService, TruckService>();

            //services.TryAddTransient<IStationGroupService, StationGroupService>();

            // Add all other services here.
            return services;
        }

        public static IServiceCollection RegisterDataServices(
            this IServiceCollection services)
        {
            services.TryAddTransient<IDataServices, DataServices>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Add all other services here.
            return services;
        }

        public static IServiceCollection RegisterDataRepositories(
            this IServiceCollection services)
        {
            services.TryAddTransient<IDataRepositories, DataRepositories>();

            // Add all other services here.
            return services;
        }
    }
}
