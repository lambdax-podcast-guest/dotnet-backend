using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Guests.Helpers;
using Microsoft.Extensions.Configuration;

namespace Guests.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase

    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration Configuration;
        // we need access to the userManager and signManager from identity, add them to the constructor so we have access to them
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
            Configuration = configuration;
        }
        // Custom InputModel so the client can use these field names
        public class InputModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string[] Roles { get; set; }
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
                    UserName = input.Email
                };
                foreach (string role in input.Roles)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(role);
                    if (!roleExists)
                    {
                        return BadRequest(new { error = "Invalid Role" });
                    }
                }
                

                // userManager is from the identity package, it comes with the CreateAsync method, when supplied two args it takes the second one as a password and hashes it. It's success or failure is stored in result
                var result = await _userManager.CreateAsync(user, input.Password);

                if (result.Succeeded)
                {
                    // add the roles to the user
                    
                    foreach (string role in input.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                    // on success login the user, false indicates we won't persist a login cookie, we want to use tokens. CreatedAtAction and BadRequest are from the ControllerBase class
                    await _signManager.SignInAsync(user, false);

                    Task<string> token = TokenManager.GenerateToken(input.Roles, user, Configuration["Guests:JwtKey"], Configuration["Guests:JwtIssuer"], _userManager);
                    return CreatedAtAction(nameof(Register), new { id = user.Id }, new { id = user.Id, token = token.Result });
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