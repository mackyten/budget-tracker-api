using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BT.DOMAIN.Enums.Accounts;
using Microsoft.AspNetCore.Http;

namespace BT.PERSISTENCE.Security
{
   public class IdentityHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IdentityType IdentityType
        {
            get
            {
                var principal = _httpContextAccessor.HttpContext?.User;

                if (principal == null || !principal.Identity.IsAuthenticated)
                    return IdentityType.None;

                return Email != null ? IdentityType.User : IdentityType.None;
            }
        }

        private IEnumerable<Claim> Claims => _httpContextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>();

        public string UserId => Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.UserId)?.Value;

        public string Email => Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Email)?.Value;

        public List<string> Roles => Claims.Where(x => x.Type == CustomClaimTypes.Role)
                                           .Select(x => x.Value)
                                           .ToList();
    }
}