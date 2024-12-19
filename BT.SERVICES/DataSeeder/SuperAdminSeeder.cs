using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BT.SERVICES.DataSeeder
{
    public class SuperAdminSeeder
    {
        public static async Task SeedSuperAdmin(IServiceProvider serviceProvider, WebApplicationBuilder builder)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var superAdminRole = "SuperAdmin";
            var superAdminEmail = "superadmin@mailinator.com";//builder.Configuration["SuperAdmin:Email"] ?? throw new Exception("SuperAdmin email not found");
            var superAdminPassword = "P@ssword1";//builder.Configuration["SuperAdmin:Password"] ?? throw new Exception("SuperAdmin password not found");

            // Check if the SuperAdmin role exists, and create it if it does not
            if (!await roleManager.RoleExistsAsync(superAdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(superAdminRole));
            }

            // Check if a SuperAdmin user exists, and create one if it does not
            var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
            if (superAdminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, superAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, superAdminRole);
                }
            }
        }
    }
}