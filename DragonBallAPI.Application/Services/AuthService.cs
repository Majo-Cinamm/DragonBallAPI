using DragonBallAPI.Application.DTOs;
using DragonBallAPI.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DragonBallAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TokenDto> AuthenticateAsync(UserDto userDto)
        {
            // En un caso real, verificarías las credenciales contra una base de datos
            // Para este ejemplo, simplemente verificamos un usuario y contraseña predefinidos
            if (userDto.Username != "admin" || userDto.Password != "DragonBallAPI2025!")
            {
                return null;
            }

            // Crear el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "DefaultSecretKey123456789012345678901234");

            var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");
            var expiration = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userDto.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = expiration,
                Issuer = _configuration["Jwt:Issuer"] ?? "DragonBallAPI",
                Audience = _configuration["Jwt:Audience"] ?? "DragonBallAPI.Client",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
