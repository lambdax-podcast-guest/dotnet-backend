using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guests.Helpers;
using Guests.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guests.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PodcastsController : ControllerBase
    {
        private readonly AppUserContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PodcastsController(AppUserContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Podcast>>> GetPodcasts()
        {
            // for async
            await Task.Delay(500);
            // get user claim containing user's id
            string userId = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            // get podcasts whose host(s)/guest(s) has user's id
            var podcasts = _context.Podcasts.Where(pc => pc.PodcastHosts.Any(pch => pch.HostId == userId) || pc.PodcastGuests.Any(pcg => pcg.GuestId == userId));
            if (podcasts.Count() == 0) { return NotFound("There are no podcasts assigned to you"); }
            return Ok(podcasts);
        }
    }
}
