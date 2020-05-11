using System;
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
        public TokenManager() { }

        // must be a better way to do this than Task<string>, we need to await the user's claims, so this needs to be async, and you can only return certain types from async methods, TODO!
        public static Task<string> GenerateToken(string[] roles, AppUser user, string jwtKey, string jwtIssuer, UserManager<AppUser> userManager)
        {

            return Task.Run(() =>
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
            });
        }
    }
}