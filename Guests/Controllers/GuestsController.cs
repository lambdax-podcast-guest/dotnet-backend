using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guests.Entities;
using Guests.Helpers;
using Guests.Models;
using Guests.Models.Customizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guests.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class GuestsController : Controller
    {
        private readonly AppUserContext Context;
        private UserManager<AppUser> _userManager;

        public GuestsController(AppUserContext context, UserManager<AppUser> userManager)
        {
            Context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetGuests()
        {
            var guests = await _userManager.GetUsersInRoleAsync(Role.Guest);
            // If there are no guests don't bother normalizing the output
            if (guests.Count == 0) return Ok("Sorry, there are no guests...");
            // Populate roles for all users
            await Task.WhenAll(guests.Select(guest => _userManager.PopulateRolesAsync(guest)));
            // Return the matching guest
            return Ok(guests);
        }

        // Doesn't really do anything. Just an example of how this could look
        // Begs the question of whether we will have another role
        [AuthorizeId(Role.Host, Role.Guest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetGuests(string id)
        {
            // Find user matching id
            AppUser guest = await _userManager.FindByIdAsync(id);
            // Get roles for user
            await _userManager.PopulateRolesAsync(guest);
            // Boolean indicating whether this user has a guest role
            bool isGuest = Array.Exists(guest.Roles, role => role == Role.Guest);
            // If user does not exist or does not have a guest role, return 400
            if (guest == null || !isGuest) return NotFound("No such guest found, please check again...");
            // Return the user
            return Ok(guest);
        }

    }
}
