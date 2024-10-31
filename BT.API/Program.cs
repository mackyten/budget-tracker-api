using static BT.API.Configurations.Database;
using static BT.API.Configurations.Swash;
using static BT.API.Configurations.Mediator;
using static BT.API.Configurations.Services;
using static BT.API.Configurations.Endpoints;
using static BT.API.Configurations.CORS;
using static BT.API.Configurations.Security;

using BT.PERSISTENCE.Context;
using BT.PERSISTENCE.Security;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IdentityHelper>();


RegisterEntityFramework(builder);
RegisterSwash(builder);
RegisterMediatr(builder);
RegisterAutoMapper(builder);
RegisterServices(builder);
RegisterEndpoints(builder);
AddFluentValidation(builder);
AddCorsPolicy(builder);
RegisterIdentityServer(builder);

//===========================================================================

var app = builder.Build();
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<BTDbContext>();
//var initialData = scope.ServiceProvider.GetRequiredService<IInitialData>();

ConfigureCors(app);
ConfigureEndpoints(app);
ConfigureSwash(app, builder);
ConfigureDatabaseMigrations(context);
//ConfigureMulticast(app, builder);
//await ConfigureInitialData(context, initialData);




app.Run();