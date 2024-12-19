using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.APPLICATION.Auth.Models
{
    public class SignInResponseModel
    {
        public required string TokenType { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required int ExpiresIn { get; set; }
    }
}