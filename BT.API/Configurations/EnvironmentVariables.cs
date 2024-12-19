using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.API.Configurations
{
    public class EnvirontmentVariables
    {
        internal static void GetEnvVariables(WebApplicationBuilder builder)
        {
            // builder.Configuration.GetConnectionString("Default");
            // Environment.GetEnvironmentVariable("Jwt:Key");
        }
    }
}