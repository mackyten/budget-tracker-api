using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.APPLICATION;
using BT.APPLICATION.Auth.Models;
using BT.SERVICES.KeyGenerator;
using MediatR;
using Microsoft.AspNetCore.Identity;



namespace Project.APPLICATION.Auth.Commands
{
    public class SignIn : IRequest<Response>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }


    public class SignInHandler : IRequestHandler<SignIn, Response>
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApiKeyGenerator _apiKeyGenerator;

        public SignInHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApiKeyGenerator apiKeyGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _apiKeyGenerator = apiKeyGenerator;
        }
        public async Task<Response> Handle(SignIn command, CancellationToken cancellationToken)
        {
            try
            {
                var expiresIn = (int)TimeSpan.FromHours(8).TotalMinutes; //Minutes
                var result = await _signInManager.PasswordSignInAsync(command.Email, command.Password, isPersistent: false, lockoutOnFailure: false);

                if (!result.Succeeded) throw new Exception("Invalid login attempt");

                var user = await _userManager.FindByNameAsync(command.Email);
                if (user == null) throw new Exception("User not found");

                var response = new SignInResponseModel
                {
                    TokenType = "Bearer",
                    AccessToken = await _apiKeyGenerator.GenerateJwtToken(user, expiresIn),
                    RefreshToken = "",
                    ExpiresIn = expiresIn
                };

                return new SuccessResponse<SignInResponseModel>(response);
            }
            catch (Exception ex)
            {
                return new BadRequestResponse(ex.GetBaseException().Message);
            }

        }
    }

}