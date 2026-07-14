using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Enums;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Infrastructure.Persistence.Contexts;

namespace VehicleCRM.Infrastructure.Persistence.Extensions
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<VehicleCrmDbContext>();

            if (await context.Vehicles.AnyAsync() || 
                await context.Customers.AnyAsync() || 
                await context.SaleOpportunities.AnyAsync())
            {
                return;
            }

            await SeedVehiclesAsync(context);
            await SeedCustomersAsync(context);
            await SeedSaleOpportunitiesAsync(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedVehiclesAsync(VehicleCrmDbContext context)
        {
            var vehicles = new[]
            {
                Vehicle.Create("Toyota", "Corolla", 2023, 125000.00m, "Prata", 15000),
                Vehicle.Create("Honda", "Civic", 2022, 135000.00m, "Preto", 25000),
                Vehicle.Create("Volkswagen", "Gol", 2024, 75000.00m, "Branco", 5000)
            };

            await context.Vehicles.AddRangeAsync(vehicles);
        }

        private static async Task SeedCustomersAsync(VehicleCrmDbContext context)
        {
            var customers = new[]
            {
                Customer.Create("João Silva", "joao.silva@email.com", "(11) 98765-4321", CustomerMainInterest.Sedan),
                Customer.Create("Maria Santos", "maria.santos@email.com", "(21) 97654-3210", CustomerMainInterest.Suv),
                Customer.Create("Pedro Oliveira", "pedro.oliveira@email.com", "(31) 96543-2109", CustomerMainInterest.Hatch)
            };

            await context.Customers.AddRangeAsync(customers);
        }

        private static async Task SeedSaleOpportunitiesAsync(VehicleCrmDbContext context)
        {
            await context.SaveChangesAsync();

            var vehicles = await context.Vehicles.ToListAsync();
            var customers = await context.Customers.ToListAsync();

            var saleOpportunities = new[]
            {
                SaleOpportunity.Create(customers[0].Id, vehicles[0].Id, 120000.00m, "Cliente interessado em financiamento"),
                SaleOpportunity.Create(customers[1].Id, vehicles[1].Id, 130000.00m, "Cliente quer test drive na próxima semana"),
                SaleOpportunity.Create(customers[2].Id, vehicles[2].Id, 72000.00m, "Primeira compra do cliente")
            };

            await context.SaleOpportunities.AddRangeAsync(saleOpportunities);
        }
    }
}
