using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guests.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase

    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;
        // we need access to the userManager and signManager from identity, add them to the constructor so we have access to them
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }
        // Custom InputModel so the client can use these field names
        public class InputModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
            public string UserName { get; set; }
            public InputModel() { }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] InputModel input)
        {

            if (ModelState.IsValid)
            {
                // if the input model is valid, as in all the fields were initialized with the correct types, create a new app user with that info
                var user = new AppUser() {
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Email = input.Email,
                    UserName = input.UserName
                };
                // userManager is from the identity package, it comes with the CreateAsync method, when supplied two args it takes the second one as a password and hashes it. It's success or failure is stored in result
                var result = await _userManager.CreateAsync(user, input.Password);

                if (result.Succeeded)
                {
                    // on success login the user, false indicates we won't persist a login cookie, we want to use tokens. CreatedAtAction and BadRequest are from the ControllerBase class
                    await _signManager.SignInAsync(user, false);
                    return CreatedAtAction("registered", new { id = user.Id }, user);
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
    }
}