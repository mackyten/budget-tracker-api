using System;
using System.Text;
using BT.API.Configurations.Filters;
using BT.DOMAIN.Entities.BudgetTracker;
using BT.PERSISTENCE.Context;
using BT.SERVICES.AccountsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BT.API.Configurations
{
    public static class Security
    {

        public static void RegisterJwtAuthentication(WebApplicationBuilder builder)
        {
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,

                       ValidIssuer = "https://etfsndmewtdjyxjsqwbe.supabase.co/auth/v1", // Supabase issuer URL
                       ValidAudience = "authenticated", // Audience from the token
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4oQEYHxChIcNakfCvbVaq9LL2lcwYReG6Fgb862Ma88/VpZ56Lv8PK2vCX4NqFapsnuOIKBwwfu4DjyLn/24Xw=="))
                   };
               });

        }
        public static void RegisterIdentityServer(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<UserAccount, IdentityRole>(o =>
            {
                o.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<BTDbContext>()
            .AddDefaultTokenProviders();

            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var accountsOption = scope.ServiceProvider.GetRequiredService<IOptions<AccountsOptions>>().Value;

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(options =>
            //     {
            //         options.Authority = accountsOption.ApiAuthorityUrl;
            //         options.RequireHttpsMetadata = accountsOption.RequiresHttpsMeta;
            //         options.Audience = accountsOption.HRPApiName;
            //     });
            RegisterJwtAuthentication(builder);
            builder.Services.AddSingleton<IStartupFilter, OptionValidationStartupFilter>();
            builder.Services.AddDataProtection();
        }
    }
}
