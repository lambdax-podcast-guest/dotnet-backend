using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Guests.Helpers
{
    public class TokenManager
    {
        private static IConfiguration config;
        public TokenManager(IConfiguration configuration)
        {
            config = configuration;
        }
        public static string GenerateToken(string[] roles, AppUser user)
        {
            var claimsIdentity = new ClaimsIdentity(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var handler = new JwtSecurityTokenHandler();

            var securityToken = new JwtSecurityToken
            (
                issuer: Startup.Configuration["Guests:JwtIssuer"],
                claims: claimsIdentity.Claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Startup.Configuration["Guests:JwtKey"])), SecurityAlgorithms.HmacSha256)
            );

            return handler.WriteToken(securityToken);
        }
    }
}