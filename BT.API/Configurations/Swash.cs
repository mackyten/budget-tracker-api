using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace BT.API.Configurations
{
    public class Swash
    {

        internal static void RegisterSwagger(WebApplicationBuilder builder)
        {
            var aspEnv = builder.Configuration.GetSection("ASPNETCORE_ENVIRONMENT")?.Value;
            // if (aspEnv == "Local" || aspEnv == "Development" || aspEnv == "Production")
            // {
                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = $"ProjectAPI {aspEnv}",
                    });
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                    });
                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                });
           // }
        }


        internal static void ConfigureSwash(WebApplication app, WebApplicationBuilder builder)
        {
            var aspEnv = builder.Configuration.GetSection("ASPNETCORE_ENVIRONMENT")?.Value;
            // if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || aspEnv == "Local" || aspEnv == "Test")
            // {
                app.UseDeveloperExceptionPage();

                app.UseSwagger(options =>
                {
                    options.SerializeAsV2 = true;
                });

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.DocumentTitle = "Project API";
                });
            //}
        }



        internal static void UseSwagger(WebApplication app)
        {
            // if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
            // {
                app.UseSwagger();
                app.UseSwaggerUI();
            //}
        }
    }
}