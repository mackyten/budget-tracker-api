using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using BT.API.Configurations.Filters;

namespace BT.API.Configurations
{
    public class Endpoints
    {
        internal static void AddControllers(WebApplicationBuilder builder)
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Endpoint>>();
            logger.LogInformation("Adding Controllers...");

            try
            {

                builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.MaxDepth = 64; // Increase the depth if necessary
                });

                builder.Services.AddMvc(options =>
                    {
                        options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                        options.Filters.Add(typeof(ActionValidationFilterAttribute));
                    })
                    //.SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

                builder.Services.AddEndpointsApiExplorer();
                logger.LogInformation("Controllers added.");

            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at AddControllers : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at AddControllers : {e.GetBaseException().Message}");
            }
        }
    }
}