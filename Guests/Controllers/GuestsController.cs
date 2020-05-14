using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Identity;

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

        public class GuestOutput
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public List<GuestTopic> Topics { get; set; }
            // //if the guest is also a host, it would be nice to be able to show their podcasts in this view
            public List<PodcastHost> Podcasts { get; set; }
            public string AvatarUrl { get; set; }
            public string Headline { get; set; }
            public string Bio { get; set; }

            public GuestOutput(AppUser guest)
            {
                Id = guest.Id;
                FirstName = guest.FirstName;
                LastName = guest.LastName;
                Email = guest.Email;
                Topics = guest.GuestTopics;
                Podcasts = guest.PodcastHosts;
                AvatarUrl = guest.AvatarUrl;
                Headline = guest.HeadLine;
                Bio = guest.Bio;
            }
        }

        // GET: api/guests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuestOutput>>> GetGuests()
        {

            var guests = await _userManager.GetUsersInRoleAsync("Guest");
            // If there are no guests don't bother normalizing the output
            if (guests.Count == 0)
            {
                return Ok("Sorry, there are no guests...");
            }

            List<GuestOutput> guestOutput = new List<GuestOutput>();
            foreach (AppUser guest in guests)
            {
                guestOutput.Add(new GuestOutput(guest));
            }

            return Ok(guestOutput);
        }

        // GET: api/guests/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GuestOutput>>> GetGuests(string id)
        {

            var guest = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(guest);
            bool isGuest = Array.Exists(roles.ToArray(), role => role == "Guest" || role == "guest");

            if (guest == null || !isGuest)
            {
                return NotFound("No such guest found, please check again...");
            }
            GuestOutput guestOutput = new GuestOutput(guest);
            return Ok(guestOutput);
        }

    }
}