
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Guests.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Guests.Helpers
{
    public class TokenManager
    {
        public TokenManager(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            Configuration = configuration;
            _userManager = userManager;
        }
        private IConfiguration Configuration { get; }
        private UserManager<AppUser> _userManager;
        
        public string GenerateToken(AppUser user)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Guests:JwtKey"]));

            var myIssuer = Configuration["Guests:JwtIssuer"];

            var tokenHandler = new JwtSecurityTokenHandler();
            //var claims = _userManager.GetValidClaims(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = myIssuer,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
