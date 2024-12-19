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
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
             .AddEntityFrameworkStores<BTDbContext>()
             .AddDefaultTokenProviders();
        }


        internal static void AddAuthentication(WebApplicationBuilder builder)
        {
            var jwt_key =Environment.GetEnvironmentVariable("JWT_KEY");
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
        }
    }
}