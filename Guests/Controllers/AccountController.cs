using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guests.Helpers;
using Guests.Models;
using Guests.Models.Customizations;
using Guests.Models.Inputs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        // UserManager and SignInManager come from Identity
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager, RoleManager<IdentityRole> roleManager, AppUserContext context)
        {
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
            _context = context;
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
                // Create a new AppUser instance
                AppUser user = new AppUser()
                {
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Email = input.Email,
                    UserName = input.Email
                };
                // Verify each role from input
                foreach (string role in input.Roles)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(role);
                    // If a role is invalid, return 400 with message
                    if (!roleExists) return BadRequest(new { error = "Invalid Role" });
                }
                // Create the user in the database and automatically hashes the password
                IdentityResult result = await _userManager.CreateAsync(user, input.Password);
                if (result.Succeeded)
                {
                    // Add each role from input to user
                    foreach (string role in input.Roles) await _userManager.AddToRoleAsync(user, role);
                    // Sign in the user (false indicates we won't persist a login cookie, because we are using jwt instead)
                    await _signManager.SignInAsync(user, false);
                    // Get user and their roles
                    Tuple<IList<string>, AppUser> userWithRoles = await TokenizeUser(input.Email);
                    // Generate token
                    string token = TokenManager.GenerateToken(userWithRoles);
                    // Return 201 with id and token
                    return CreatedAtAction(nameof(Register), new { id = user.Id }, new { id = user.Id, token = token });
                }
                // Add each error from result to model state
                foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Description);
                // Return the model state with a 400 status
                return ValidationProblem(ModelState);
            }

            // if the model state was never valid return the modelstate errors
            return UnprocessableEntity(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginInput input)
        {
            // Return 422 if Model inconsistent with AppUser
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            // Attempt to sign user in using password
            Microsoft.AspNetCore.Identity.SignInResult result = await _signManager.PasswordSignInAsync(input.Email, input.Password, false, false);
            if (result.Succeeded)
            {
                // Get user and their roles
                Tuple<IList<string>, AppUser> userWithRoles = await TokenizeUser(input.Email);
                // Generate token
                string token = TokenManager.GenerateToken(userWithRoles);
                // Return the token with 200 status
                return Ok(new { token = token });
            }
            // If there was an error signing in, return 400 with a message
            return ValidationProblem("The user/password combination was incorrect.");
        }

        [AuthorizeId]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUser input)
        {
            // Return 422 if Model inconsistent with AppUser
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            // Get user matching id
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
            // Save the changes to the database
            await _context.SaveChangesAsync();
            // Populate user with roles
            await _userManager.PopulateRolesAsync(user);
            // Return the saved user with 200 status
            return Ok(user);

        }

        [AuthorizeId]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            // Get user matching id
            AppUser user = await _userManager.FindByIdAsync(id);
            // Return 404 status if user not found
            if (user == null) return NotFound("The user with the specified ID was not found.");
            // Remove the user from the database
            _context.Remove(user);
            // Save the changes to the database
            await _context.SaveChangesAsync();
            // Return 404 if no other users are left in the database
            if (_context.Users == null) return NotFound("No users found.");
            // Return what users are left in the database
            AppUser[] users = _context.Users.ToArray();
            // Populate roles for all users
            await Task.WhenAll(users.Select(user => _userManager.PopulateRolesAsync(user)));
            // Return all users with 200 status
            return Ok(users);
        }
    }
}
