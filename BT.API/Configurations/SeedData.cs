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
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    await SuperAdminSeeder.SeedSuperAdmin(services, builder);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error Occured at SeedAsync : {e.GetBaseException().Message}");
            }
        }
    }
}