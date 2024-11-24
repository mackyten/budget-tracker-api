using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.PERSISTENCE.Context;
using Microsoft.EntityFrameworkCore;

namespace BT.API.Configurations
{
    public class Database
    {
        internal static void RegisterEntityFramework(WebApplicationBuilder builder)
        {
            var a = builder.Configuration.GetConnectionString("DefaultSQL");
            builder.Services.AddDbContext<BTDbContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQL"), sqlServerOptions => sqlServerOptions.CommandTimeout(60));
                //opts.UseNpgsql(builder.Configuration.GetConnectionString("Default")!, sqlServerOptions => sqlServerOptions.CommandTimeout(60));
            });
        }


        internal static void ConfigureDatabaseMigrations(BTDbContext context)
        {
            //Check if there are any pending migrations
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

        }


        /// UNCOMMENT IF BUILDING INITIAL DATA IS NEEDED
        // internal static async Task ConfigureInitialData(BTDbContext context, IInitialData initialData)
        // {
        //     //Check for initial data buildup. Mainly used for large initial data lists
        // }  
    }
}