using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VehicleCRM.Application.Common.Behaviors;
using VehicleCRM.Domain.Common.UnitOfWork;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Services;
using VehicleCRM.Domain.Vehicles.Repositories;
using VehicleCRM.Infrastructure.Persistence.Repositories;
using VehicleCRM.Infrastructure.Persistence.UnitOfWork;

namespace VehicleCRM.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddVehicleCrmServices(this IServiceCollection services)
        {
            services.AddApplicationServices();
            services.AddInfrastructureServices();
            services.AddDomainServices();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(ValidationBehavior<,>).Assembly;

            services.AddValidatorsFromAssembly(applicationAssembly);

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(applicationAssembly);
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<ISaleOpportunityRepository, SaleOpportunityRepository>();

            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ISaleOpportunityDomainService, SaleOpportunityDomainService>();

            return services;
        }
    }
}
