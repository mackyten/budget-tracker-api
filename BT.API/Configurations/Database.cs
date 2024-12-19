using BT.PERSISTENCE.Context;
using Microsoft.EntityFrameworkCore;

namespace BT.API.Configurations
{
    public class Database
    {
        internal static void RegisterDatabase(WebApplicationBuilder builder)
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Database>>();

            try
            {
                // Register Entity Framework Core with PostgreSQL database connection
                builder.Services.AddDbContext<BTDbContext>(options =>
                {
                    var connectionString = builder.Configuration.GetConnectionString("DEFAULT_SQL");
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        connectionString = Environment.GetEnvironmentVariable("DEFAULT_SQL");
                    }


                    // Configures PostgreSQL connection
                    // options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));

                    //Configures SQL connection

                    logger.LogInformation($"Using connection string: {connectionString}",
                        string.IsNullOrEmpty(builder.Configuration.GetConnectionString("DEFAULT_SQL")) ? "Environment Variable" : "appsettings.json");

                    options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(60));
                });
            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at RegisterDatabase : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at RegisterDatabase : {e.GetBaseException().Message}");
            }
        }

        internal static void ApplyPendingMigrations(WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<Database>>();
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    logger.LogInformation("Starting to apply pending migrations...");

                    var dbContext = scope.ServiceProvider.GetRequiredService<BTDbContext>();
                    dbContext.Database.Migrate();

                    logger.LogInformation("migrations applied.");

                }
            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at ApplyPendingMigrations : {e.GetBaseException().Message}");

                throw new Exception($"Error Occured at ApplyPendingMigrations : {e.GetBaseException().Message}");
            }
        }
    }
}