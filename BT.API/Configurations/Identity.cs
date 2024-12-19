using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.PERSISTENCE.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BT.API.Configurations
{
    public class Identity
    {
        internal static void RegisterIdentity(WebApplicationBuilder builder)
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Identity>>();
            try
            {
                logger.LogInformation("Registering Identity...");
                builder.Services.AddIdentity<IdentityUser, IdentityRole>()
              .AddEntityFrameworkStores<BTDbContext>()
              .AddDefaultTokenProviders();

                logger.LogInformation("Identity Registered.");

            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at RegisterIdentity : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at RegisterIdentity : {e.GetBaseException().Message}");
            }
        }


        internal static void AddAuthentication(WebApplicationBuilder builder)
        {
            try
            {
                var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Identity>>();
                logger.LogInformation("Adding Authentication...");
                var jwt_key = Environment.GetEnvironmentVariable("JWT_KEY");
                var key = Encoding.ASCII.GetBytes(jwt_key ?? throw new Exception("Jwt:Key is missing in appsettings.json"));
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

                logger.LogInformation("Authentication Added.");

            }
            catch (Exception e)
            {
                throw new Exception($"Error Occured at AddAuthentication : {e.GetBaseException().Message}");
            }
        }
    }
}