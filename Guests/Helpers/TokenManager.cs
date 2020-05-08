
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Guests.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

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
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

            // trying to figure out how to use the claims!
            // var claims = await userManager.GetClaimsAsync(user);
            // var Token = new JwtSecurityToken(
            //    issuer: jwtIssuer,
            //    expires: DateTime.Now.AddDays(1),
            //    claims: claims,
            //    signingCredentials: new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature));
            //return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
