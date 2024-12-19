using static BT.API.Configurations.Endpoints;
using static BT.API.Configurations.Swash;
using static BT.API.Configurations.Database;
using static BT.API.Configurations.Mediator;
using static BT.API.Configurations.Identity;
using static BT.API.Configurations.CORS;
using static BT.API.Configurations.Services;
using static BT.API.Configurations.SeedData;
using static BT.API.Configurations.EnvirontmentVariables;


var builder = WebApplication.CreateBuilder(args);


GetEnvVariables(builder);
AddControllers(builder);
RegisterSwagger(builder);
RegisterDatabase(builder);
RegisterMediatr(builder);
RegisterAutoMapper(builder);
RegisterIdentity(builder);
AddCorsPolicy(builder);
RegisterServices(builder);
AddAuthentication(builder);


var app = builder.Build();


UseSwagger(app);
ApplyPendingMigrations(app);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
ConfigureCors(app);
await SeedAsync(app, builder);


app.Run();