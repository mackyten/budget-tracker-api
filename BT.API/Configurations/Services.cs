using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.SERVICES.EmailSender;
using BT.SERVICES.KeyGenerator;
using Microsoft.AspNetCore.Identity;


namespace BT.API.Configurations
{
    public class Services
    {
        internal static void RegisterServices(WebApplicationBuilder builder)
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Services>>();
            try
            {
                logger.LogInformation("Registering Services...");

                builder.Services.AddScoped<ApiKeyGenerator>();
                builder.Services.AddSingleton<IEmailSender<IdentityUser>, NoOpEmailSender<IdentityUser>>();

                logger.LogInformation("Services registered.");
            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at RegisterServices : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at RegisterServices : {e.GetBaseException().Message}");
            }
        }
    }
}