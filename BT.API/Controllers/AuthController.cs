using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BT.APPLICATION;
using BT.APPLICATION.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Command = Project.APPLICATION.Auth.Commands;

namespace BT.API.Controllers
{
    [Route("api/auth")]
    public class AuthController(SignInManager<IdentityUser> signInManager) : BaseController
    {

        private readonly SignInManager<IdentityUser> _signInManager = signInManager;


        [HttpPost("register")]
        // [Authorize(Roles = "SuperAdmin")]
        [Description("Creates new admin")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] Command.Register command)
        {

            var result = await Mediator.Send(command);

            if (result is BadRequestResponse)
            {
                return BadRequest(result.Message);
            }

            var data = ((SuccessResponse<string>)result).Data;
            return Ok(data);
        }


        [HttpPost("login")]
        [Description("Logs in a user")]
        [ProducesResponseType(typeof(SignInResponseModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Login([FromBody] Command.SignIn command)
        {

            var result = await Mediator.Send(command);

            if (result is BadRequestResponse)
            {
                return BadRequest(result.Message);
            }

            var data = ((SuccessResponse<SignInResponseModel>)result).Data;
            return Ok(data);
        }

        [HttpPost("logout")]
        [Authorize]
        [Description("Logs out the current user")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "User logged out successfully" });
        }
    }
}