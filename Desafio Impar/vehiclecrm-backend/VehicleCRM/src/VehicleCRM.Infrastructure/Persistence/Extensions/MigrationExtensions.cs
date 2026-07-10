using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VehicleCRM.Infrastructure.Persistence.Contexts;

namespace VehicleCRM.Infrastructure.Persistence.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigrationsAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var logger = scope.ServiceProvider
                .GetRequiredService<ILogger<VehicleCrmDbContext>>();

            try
            {
                var context = scope.ServiceProvider.GetRequiredService<VehicleCrmDbContext>();

                logger.LogInformation("Aplicando migrations...");

                await context.Database.MigrateAsync();

                logger.LogInformation("Migrations aplicadas com sucesso.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao aplicar migrations.");
                throw;
            }
        }
    }
}