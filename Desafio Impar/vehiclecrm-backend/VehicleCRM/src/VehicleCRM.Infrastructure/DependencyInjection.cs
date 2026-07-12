using Microsoft.Extensions.DependencyInjection;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Repositories;
using VehicleCRM.Infrastructure.Persistence.Repositories;

namespace VehicleCRM.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<ISaleOpportunityRepository, SaleOpportunityRepository>();

            return services;
        }
    }
}
