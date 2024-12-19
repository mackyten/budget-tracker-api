using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.APPLICATION.Auth.Models
{
    public class RegisterModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}