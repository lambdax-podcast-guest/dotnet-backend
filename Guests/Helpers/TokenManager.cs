using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Guests.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Guests.Helpers
{
    public class TokenManager
    {
        public TokenManager(IConfiguration configuration) { }
        public static string GenerateToken(Tuple<IList<string>, AppUser> user)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(user.Item1.Select(role => new Claim(ClaimTypes.Role, role)));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Item2.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Item2.Id));

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