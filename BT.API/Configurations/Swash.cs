using Microsoft.OpenApi.Models;

namespace BT.API.Configurations
{
    public static class Swash
    {
        internal static void RegisterSwash(WebApplicationBuilder builder)
        {
            var aspEnv = builder.Configuration.GetSection("ASPNETCORE_ENVIRONMENT")?.Value;
            var clinetEnv = builder.Configuration.GetSection("Client_Environment")?.Value;

            // if (clinetEnv == "Local" || aspEnv == "Development" || aspEnv == "Production" || aspEnv == "Test")
            // {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"BUDGET-TRACKER.API {aspEnv}",
                    Description = $"RESTFul Api for BUDGET-TRACKER Version: {builder.Configuration["buildVersion"]}"
                });

                // Add the JWT security definition
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer ' followed by your token"
                });

                // Add a security requirement to use the defined security scheme
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"  // Ensure this matches the security definition
                                }
                            },
                            new string[] {}
                        }
                });

                options.CustomSchemaIds(i => i.FullName);
            });
            // }
        }

        internal static void ConfigureSwash(WebApplication app, WebApplicationBuilder builder)
        {
            var aspEnv = builder.Configuration.GetSection("ASPNETCORE_ENVIRONMENT")?.Value;

            // if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || aspEnv == "Local" || aspEnv == "Test")
            // {
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            //}
        }
    }
}
