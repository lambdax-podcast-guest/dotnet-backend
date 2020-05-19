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
        private AppUserContext _context;

        private IConfiguration Configuration;
        // we need access to the userManager and signManager from identity, add them to the constructor so we have access to them
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager, RoleManager<IdentityRole> roleManager, AppUserContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
            _context = context;
            Configuration = configuration;
        }

        /// <summary>Asynchronously gets a user's roles and user data from the database.</summary>
        private async Task<Tuple<IList<string>, AppUser>> TokenizeUser(string email)
        {
            // Get the user from the database
            AppUser dbUser = _userManager.Users.Where(u => u.Email == email).First();
            // Get the roles associated with the user from the database 
            IList<string> roles = await _userManager.GetRolesAsync(dbUser);
            return new Tuple<IList<string>, AppUser>(roles, dbUser);
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

                    Tuple<IList<string>, AppUser> userWithRoles = await TokenizeUser(input.Email);
                    string token = TokenManager.GenerateToken(userWithRoles, Configuration);

                    return CreatedAtAction(nameof(Register), new { id = user.Id }, new { id = user.Id, token = token });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return ValidationProblem(new ValidationProblemDetails(ModelState));
                }
            }

            // if the model state was never valid return the modelstate errors
            return UnprocessableEntity(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginInput input)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signManager.PasswordSignInAsync(input.Email, input.Password, false, false);

                if (result.Succeeded)
                {
                    Tuple<IList<string>, AppUser> userWithRoles = await TokenizeUser(input.Email);
                    string token = TokenManager.GenerateToken(userWithRoles, Configuration);
                    return Ok(new { token = token });
                }
                else
                {
                    return ValidationProblem(ModelState);
                }
            }
            return UnprocessableEntity(ModelState);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUser input)
        {
            if (ModelState.IsValid)
            {
                bool isUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) == id;

                if (isUserId)
                {
                    AppUser user = await _userManager.FindByIdAsync(id);

                    // Return 404 status if user not found
                    if (user == null) return NotFound("The user with the specified ID was not found.");

                    // Update only the non-identity fields
                    user.FirstName = input.FirstName;
                    user.LastName = input.LastName;
                    user.AvatarUrl = input.AvatarUrl;
                    user.HeadLine = input.HeadLine;
                    user.Location = input.Location;
                    user.Bio = input.Bio;
                    user.Languages = input.Languages;

                    await _context.SaveChangesAsync();

                    return Ok(user);
                }

                // Return 401 if not same user
                return Unauthorized();
            }

            // Return 422 if Model inconsistent with AppUser
            return UnprocessableEntity(ModelState);
        }

        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool isUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) == id;

            if (isUserId)
            {
                AppUser user = await _userManager.FindByIdAsync(id);

                if (user == null) return NotFound("The user with the specified ID was not found.");

                _context.Remove(user);

                await _context.SaveChangesAsync();

                if (_context.Users == null) return NotFound("No users found.");

                return Ok(_context.Users);
            }
            return Unauthorized();
        }
    }
}