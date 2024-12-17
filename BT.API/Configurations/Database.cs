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
            var connStr = builder.Configuration.GetConnectionString("DefaultSQL");

            builder.Services.AddDbContext<BTDbContext>(opts =>
            {

                try
                {
                    opts.UseSqlServer(connStr, sqlServerOptions => sqlServerOptions.CommandTimeout(60));

                }
                catch (Exception e)
                {

                    throw new Exception($"{e.GetBaseException().Message} : ${connStr}");
                }
            });
        }


        internal static void ConfigureDatabaseMigrations(BTDbContext context, WebApplicationBuilder builder)
        {
            var connStr = builder.Configuration.GetConnectionString("DefaultSQL");

            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception e)
            {

                throw new Exception($"{e.GetBaseException().Message} : ${connStr}");
            }

        }


        /// UNCOMMENT IF BUILDING INITIAL DATA IS NEEDED
        // internal static async Task ConfigureInitialData(BTDbContext context, IInitialData initialData)
        // {
        //     //Check for initial data buildup. Mainly used for large initial data lists
        // }  
    }
}