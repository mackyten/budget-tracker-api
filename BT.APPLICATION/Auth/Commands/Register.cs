using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.APPLICATION;
using BT.PERSISTENCE.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Project.DOMAIN.Enums.Accounts;

namespace Project.APPLICATION.Auth.Commands
{
    public class Register : IRequest<Response>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }


    public class RegisterHandler(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : IRequestHandler<Register, Response>
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<Response> Handle(Register command, CancellationToken cancellationToken)
        {
            try
            {
                var user = new IdentityUser { UserName = command.Email, Email = command.Email };
                var result = await _userManager.CreateAsync(user, command.Password);


                if (result.Succeeded)
                {
                    var superAdminRole = Roles.GetEnumDescription(UserRoles.SuperAdmin);
                    var adminRole = Roles.GetEnumDescription(UserRoles.SuperAdmin);
                    var superAdminExists = await _userManager.GetUsersInRoleAsync(superAdminRole);
                    var role = superAdminExists.Any() ? adminRole : superAdminRole;

                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }

                    await _userManager.AddToRoleAsync(user, role);
                    return new SuccessResponse<string>("User registered successfully");
                }
                throw new Exception(result.Errors?.FirstOrDefault()?.Description ?? "Registration failed");
            }
            catch (Exception ex)
            {
                return new BadRequestResponse(ex.GetBaseException().Message);
            }
        }
    }
}