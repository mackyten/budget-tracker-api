

using BT.SERVICES.DateTimeService;
using BT.SERVICES.SupabaseService;
using Microsoft.Extensions.Options;


namespace BT.API.Configurations
{
    public static class Services
    {
        internal static void RegisterServices(WebApplicationBuilder builder)
        {
            //Date Time Service
            builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
            builder.Services.AddSingleton<SupabaseService>();
            builder.Services.AddHttpContextAccessor();
        }
    }
}
