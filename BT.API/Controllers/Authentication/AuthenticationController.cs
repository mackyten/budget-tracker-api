using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BT.PERSISTENCE.Security;
using BT.SERVICES.SupabaseService;
using Microsoft.AspNetCore.Mvc;

namespace BT.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : BaseController
    {

        private readonly SupabaseService _supabaseService;

        public AuthenticationController(SupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var response = await _supabaseService.GetClient().Auth.SignUp(request.Email, request.Password);

            if (response?.User != null)
            {
                // Create claims for the new user
                var claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.UserId, response.User.Id!), // Store Supabase User ID
                new Claim(CustomClaimTypes.Email, request.Email),
                // Add any additional claims as necessary
            };

                var identity = new ClaimsIdentity(claims, "Custom");
                var principal = new ClaimsPrincipal(identity);
                Thread.CurrentPrincipal = principal; // Set the current principal

                return Ok(new { message = "User created successfully!" });
            }

            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _supabaseService.GetClient().Auth.SignIn(request.Email, request.Password);

            if (response?.User != null)
            {
                // Create claims for the logged-in user
                var claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.UserId, response.User.Id!), // Store Supabase User ID
                new Claim(CustomClaimTypes.Email, request.Email),
                // Add any additional claims as necessary
            };

                var identity = new ClaimsIdentity(claims, "Custom");
                var principal = new ClaimsPrincipal(identity);
                Thread.CurrentPrincipal = principal; // Set the current principal

                return Ok(new { message = "Login successful!", data = response });
            }

            return BadRequest(response);
        }
    }


    public class SignUpRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}