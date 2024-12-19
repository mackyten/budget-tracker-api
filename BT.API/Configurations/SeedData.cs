using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.SERVICES.DataSeeder;

namespace BT.API.Configurations
{
    public class SeedData
    {
        internal static async Task SeedAsync(WebApplication app, WebApplicationBuilder builder)
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<SeedData>>();

            try
            {
                logger.LogInformation("Seeding Data...");
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    await SuperAdminSeeder.SeedSuperAdmin(services, builder);
                }
                logger.LogInformation("Data Seeded...");
            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at SeedAsync : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at SeedAsync : {e.GetBaseException().Message}");
            }
        }
    }
}