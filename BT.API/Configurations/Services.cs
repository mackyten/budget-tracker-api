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
            try
            {
                builder.Services.AddScoped<ApiKeyGenerator>();
                builder.Services.AddSingleton<IEmailSender<IdentityUser>, NoOpEmailSender<IdentityUser>>();
            }
            catch (Exception e)
            {
                throw new Exception($"Error Occured at RegisterServices : {e.GetBaseException().Message}");
            }
        }
    }
}