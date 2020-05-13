using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Guests.Models;
using Guests.Models.Inputs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Guests.Helpers;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Guests.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase

    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signManager;
        private RoleManager<IdentityRole> _roleManager;
        // we need access to the userManager and signManager from identity, add them to the constructor so we have access to them
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterInput input)
        {

            if (ModelState.IsValid)
            {
                // if the input model is valid, as in all the fields were initialized with the correct types, create a new app user with that info
                AppUser user = new AppUser()
                {
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Email = input.Email,
                    UserName = input.Email
                };
                // verify the roles on the body exist
                foreach (string role in input.Roles)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(role);
                    if (!roleExists)
                    {
                        return BadRequest(new { error = "Invalid Role" });
                    }
                }

                // userManager is from the identity package, it comes with the CreateAsync method, when supplied two args it takes the second one as a password and hashes it. It's success or failure is stored in result
                IdentityResult result = await _userManager.CreateAsync(user, input.Password);

                if (result.Succeeded)
                {
                    // add the roles to the user
                    foreach (string role in input.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }

                    // on success login the user, false indicates we won't persist a login cookie, we want to use tokens. CreatedAtAction and BadRequest are from the ControllerBase class
                    await _signManager.SignInAsync(user, false);

                    /* Might encapsulate the following logic to a function later:

                        1. Get the user from the database
                    */
                    AppUser dbUser = _userManager.Users.Where(u => u.Email == input.Email).First();
                    /*  
                        2. Get the roles associated with the user from the database 
                    */
                    IList<string> roles = await _userManager.GetRolesAsync(dbUser);
                    string token = TokenManager.GenerateToken(roles, dbUser);

                    return CreatedAtAction(nameof(Register), new { id = user.Id }, new { id = user.Id, token = token });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }

            // if the model state was never valid return the modelstate errors
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginInput input)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(input.Email, input.Password, false, false);

                if (result.Succeeded)
                {
                    AppUser dbUser = _userManager.Users.Where(u => u.Email == input.Email).First();
                    IList<string> roles = await _userManager.GetRolesAsync(dbUser);
                    var token = TokenManager.GenerateToken(roles, dbUser);
                    return Ok(new { token = token });
                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest(ModelState);
        }
    }
}