using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guests.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InviteController : ControllerBase
    {
        private UserManager<AppUser> userManager;
        private AppUserContext context;
        public InviteController(UserManager<AppUser> _userManager, AppUserContext _userContext)
        {
            userManager = _userManager;
            context = _userContext;
        }

        [HttpGet]
        private async Task<IActionResult> GetAllInvitations()
        {
            // grab current user's id
            string userId = User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                // find invitations with user's id as the subject
                IList<Invitation> matches = await context.Invitations.Where(i => i.GuestId == userId).ToListAsync();
                // return matching invites
                return Ok(matches);
            }
            // if none found, handle exception
            catch (Exception ex) { return StatusCode(500, ex); }
        }
    }
}
