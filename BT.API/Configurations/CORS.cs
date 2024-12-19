using Microsoft.AspNetCore.HttpOverrides;

namespace BT.API.Configurations
{
    public class CORS
    {
        internal static void AddCorsPolicy(WebApplicationBuilder builder)
        {
            try
            {
                var corsOptions = new CorsOptions();
                builder.Configuration.GetSection("Cors").Bind(corsOptions);
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("default", policy => policy
                        .WithOrigins(corsOptions.AllowedOrigins?.ToArray() ?? [])
                        .AllowAnyHeader()
                        .AllowAnyMethod());
                });
            }
            catch (Exception e)
            {
                throw new Exception($"Error Occured at AddCorsPolicy : {e.GetBaseException().Message}");
            }
        }

        internal static void ConfigureCors(WebApplication app)
        {
            try
            {
                app.UseCors("default");
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }
            catch (Exception e)
            {
                throw new Exception($"Error Occured at ConfigureCors : {e.GetBaseException().Message}");
            }
        }


        public class CorsOptions
        {
            public List<string>? AllowedOrigins { get; set; }
        }
    }
}