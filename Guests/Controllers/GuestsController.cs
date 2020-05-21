using System.Collections.Generic;
using System.Threading.Tasks;
using Guests.Models;
using Guests.Models.Customizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;

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
            var guests = await _userManager.GetUsersInRoleAsync("Guest");
            // If there are no guests don't bother normalizing the output
            if (guests.Count == 0) return Ok("Sorry, there are no guests...");
            return Ok(guests);
        }

        [AuthorizeId]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetGuests(string id)
        {
            var guest = await _userManager.FindByIdAsync(id);

            var roles = await _userManager.GetRolesAsync(guest);

            bool isGuest = Array.Exists(roles.ToArray(), role => role == "Guest" || role == "guest");

            if (guest == null || !isGuest) return NotFound("No such guest found, please check again...");
            return Ok(guest);
        }

    }
}
