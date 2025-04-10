using DragonBallAPI.Application.DTOs;
using DragonBallAPI.Application.Interfaces;
using DragonBallAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonBallAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authService.AuthenticateAsync(userDto);

            if (token == null)
            {
                return Unauthorized("Credenciales inválidas");
            }

            return Ok(token);
        }
    }
}
