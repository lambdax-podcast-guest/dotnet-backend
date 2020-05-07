using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Guests.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase

    {
        private readonly GuestsContext Context;
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AppUser input)
        {
            var newUser = new AppUser()
            {
                Email = input.Email,
                PasswordHash = input.PasswordHash
            };

            Context.Users.Add(newUser);
            await Context.SaveChangesAsync();

            return CreatedAtAction("Register", new { id = newUser.Id }, newUser);
        }
    }
}