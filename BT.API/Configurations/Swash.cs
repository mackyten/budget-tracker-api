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
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Swash>>();

            try
            {
                logger.LogInformation("Registering Swagger...");
                var aspEnv = builder.Configuration.GetSection("ASPNETCORE_ENVIRONMENT")?.Value;
                if (aspEnv == "Local" || aspEnv == "Development" || aspEnv == "Production")
                {
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
                }
                logger.LogInformation("Swagger Registered.");

            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at RegisterSwagger : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at RegisterSwagger : {e.GetBaseException().Message}");
            }
        }


        internal static void ConfigureSwash(WebApplication app, WebApplicationBuilder builder)
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Swash>>();
            try
            {
                logger.LogInformation("Configuring Swash...");

                var aspEnv = builder.Configuration.GetSection("ASPNETCORE_ENVIRONMENT")?.Value;
                if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || aspEnv == "Local" || aspEnv == "Test")
                {
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
                }
                logger.LogInformation("Swash Configured.");

            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at ConfigureSwash : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at ConfigureSwash : {e.GetBaseException().Message}");
            }
        }



        internal static void UseSwagger(WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<Swash>>();
            try
            {
                logger.LogInformation("Using Swagger...");
                if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                logger.LogInformation("Swagger used.");

            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at ConfigureSwash : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at ConfigureSwash : {e.GetBaseException().Message}");
            }
        }
    }
}