using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Shortha.Application.Interfaces.Services;
using Shortha.Domain.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Shortha.Application.Services
{
    public class JwtTokenService(ISecretService secretService) : ITokenService
    {
        public string GenerateToken(string userId, string email, string role, int expireMinutes = 60)
        {
            // Claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var JwtSecret = secretService.GetSecret("JwtSecret");
            var Audience = secretService.GetSecret("JwtAudience");

            // Key & credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token object
            var token = new JwtSecurityToken(
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            // Return string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}