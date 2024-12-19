using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.API.Configurations
{
    public class SeedData
    {
        internal static async Task SeedAsync(WebApplication app, WebApplicationBuilder builder)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //await SuperAdminSeeder.SeedSuperAdmin(services, builder);
            }
        }
    }
}