using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BT.SERVICES.KeyGenerator
{
    public class ApiKeyGenerator
    {

        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public ApiKeyGenerator(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.configuration = configuration;
            _userManager = userManager;
        }


        public async Task<string> GenerateJwtToken(IdentityUser user, int expiresIn)
        {

            var jwt_key = Environment.GetEnvironmentVariable("JWT_KEY");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwt_key ?? throw new Exception("Jwt key is missing"));

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Email, user.Email!),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiresIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}