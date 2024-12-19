using BT.PERSISTENCE.Context;
using Microsoft.EntityFrameworkCore;

namespace BT.API.Configurations
{
    public class Database
    {
        internal static void RegisterDatabase(WebApplicationBuilder builder)
        {
            // Register Entity Framework Core with PostgreSQL database connection
            builder.Services.AddDbContext<BTDbContext>(options =>
            {
                Console.WriteLine($"Envirinment: ${builder.Environment.EnvironmentName}");
                var connectionString = Environment.GetEnvironmentVariable("DEFAULT_SQL");
                // Configures PostgreSQL connection
                // options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));

                //Configures SQL connection
                options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(60));
            });
        }

        internal static void ApplyPendingMigrations(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BTDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}