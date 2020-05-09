
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Guests.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Guests.Helpers
{
    public class TokenManager
    {
        public TokenManager()
        {
        }

        // must be a better way to do this than Task<string>, we need to await the user's claims, so this needs to be async, and you can only return certain types from async methods, TODO!
        public static async Task<string> GenerateToken(string[] roles, AppUser user, string jwtKey, string jwtIssuer, UserManager<AppUser> userManager)
        {
            
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
            
            var claims = new List<Claim>();
            foreach (string role in roles)
            {
                claims.Add(new Claim("roles", role));
            }
            
            var Token = new JwtSecurityToken(
               issuer: jwtIssuer,
               expires: DateTime.UtcNow.AddDays(1),
               claims: claims,
               signingCredentials: new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(Token);

        }
    }
}
