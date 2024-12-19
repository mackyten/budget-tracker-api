using Microsoft.AspNetCore.HttpOverrides;

namespace BT.API.Configurations
{
    public class CORS
    {
        internal static void AddCorsPolicy(WebApplicationBuilder builder)
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<CORS>>();

            try
            {
                logger.LogInformation("Adding CORS policy...");

                var corsOptions = new CorsOptions();
                builder.Configuration.GetSection("Cors").Bind(corsOptions);
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("default", policy => policy
                        .WithOrigins(corsOptions.AllowedOrigins?.ToArray() ?? [])
                        .AllowAnyHeader()
                        .AllowAnyMethod());
                });

                logger.LogInformation("CORS policy applied.");

            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at AddCorsPolicy : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at AddCorsPolicy : {e.GetBaseException().Message}");
            }
        }

        internal static void ConfigureCors(WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<CORS>>();

            try
            {
                logger.LogInformation("Configuring CORS...");

                app.UseCors("default");
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });

                logger.LogInformation("CORS configured.");
            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at ConfigureCors : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at ConfigureCors : {e.GetBaseException().Message}");
            }
        }


        public class CorsOptions
        {
            public List<string>? AllowedOrigins { get; set; }
        }
    }
}