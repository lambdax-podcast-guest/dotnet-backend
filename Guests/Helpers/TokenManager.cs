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
        public TokenManager(IConfiguration configuration) { }
        public static string GenerateToken(IList<string> roles, AppUser user)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: Startup.Configuration["Guests:JwtIssuer"],
                // access the claims within the claims identity
                claims: claimsIdentity.Claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Startup.Configuration["Guests:JwtKey"])), SecurityAlgorithms.HmacSha256)
            );

            return handler.WriteToken(token);
        }
    }
}