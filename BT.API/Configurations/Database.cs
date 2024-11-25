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

                var connStr = "Server=tcp:bt-server.database.windows.net,1433;Initial Catalog=BTDb;Persist Security Info=False;User ID=BtAdmin;Password=BtTracker!234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

                opts.UseSqlServer(connStr, sqlServerOptions => sqlServerOptions.CommandTimeout(60));

                // opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQL"), sqlServerOptions => sqlServerOptions.CommandTimeout(60));
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