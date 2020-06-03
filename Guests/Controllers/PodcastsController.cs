using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guests.Entities;
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
            var podcasts = _context.Podcasts.Where(
                pc =>
                pc.PodcastHosts.Any(pch => pch.HostId == userId) ||
                pc.PodcastGuests.Any(pcg => pcg.GuestId == userId)
            );
            // if no podcasts exist for user, 
            if (podcasts.Count() == 0) { return NotFound("There are no podcasts assigned to you"); }
            return Ok(podcasts);
        }

        [Authorize(Roles = Role.Host)]
        [HttpPost]
        public async Task<ActionResult> PostPodcast([FromBody] Podcast input)
        {
            try
            {
                // Create new podcast with input information, not including hosts or guests
                Podcast newPodcast = new Podcast() { Name = input.Name, Description = input.Description, HeadLine = input.HeadLine };
                // Get id for user who is making new podcast
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                // Create new host
                PodcastHost host = new PodcastHost() { HostId = userId, PodcastId = newPodcast.Id };
                // Populate host list with user
                List<PodcastHost> hostList = new List<PodcastHost>() { host };
                // Save host to db
                await _context.PodcastHosts.AddAsync(host);
                // add hosts to podcast
                newPodcast.PodcastHosts = hostList;
                // initiate guest list
                List<PodcastGuest> guestList = new List<PodcastGuest>();
                // add each guest from input to guest list
                foreach (string id in input.PodcastGuests.Select(g => g.GuestId))
                {
                    // Create new guest
                    PodcastGuest guest = new PodcastGuest() { GuestId = id, PodcastId = newPodcast.Id }
                    // Populate host list with guest
                    guestList.Add(guest);
                    // Save guest to db
                    await _context.PodcastGuests.AddAsync(guest);
                }
                // add guests to podcast
                newPodcast.PodcastGuests = guestList;
                // add podcast to db
                await _context.Podcasts.AddAsync(newPodcast);
                // save changes to db
                await _context.SaveChangesAsync();
                // return created if succeeded
                return Created(newPodcast.Id.ToString(), newPodcast);
            }
            // return 500 if error occurred
            catch (Exception) { return StatusCode(500); }
        }
    }
}
