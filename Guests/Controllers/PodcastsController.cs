using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guests.Entities;
using Guests.Helpers;
using Guests.Models;
using Guests.Models.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                // for async
                await Task.Delay(500);
                // get user claim containing user's id
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                // get podcasts whose host(s)/guest(s) has user's id
                IQueryable<Podcast> podcasts = _context.PodcastHosts.Where(ph => ph.HostId == userId).Select(p => p.Podcast);
                // if no podcasts exist for user, 
                if (podcasts.Count() == 0) { return NotFound("There are no podcasts assigned to you"); }
                return Ok(podcasts);
            }
            catch (Exception ex) { return StatusCode(500, ex); }
        }

        [Authorize(Roles = Role.Host)]
        [HttpPost]
        public async Task<ActionResult<Podcast>> PostPodcast([FromBody] PodcastInput input)
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            try
            {
                // Create new podcast with input information, not including hosts or guests
                Podcast newPodcast = new Podcast() { Name = input.Name, Description = input.Description, HeadLine = input.HeadLine, PodcastTopics = new List<PodcastTopic>(), PodcastHosts = new List<PodcastHost>() };
                foreach (string topic in input.Topics)
                {
                    // check for existing topic
                    bool topicExists = _context.Topics.Any(t => t.Name == topic);
                    // either create new topic or find existing topic
                    Topic topicMatch = topicExists ? _context.Topics.First(t => t.Name == topic) : new Topic() { Name = topic };
                    // create new relationship
                    PodcastTopic relationship = new PodcastTopic() { PodcastId = newPodcast.Id, TopicId = topicMatch.Id };
                    // add topic to db
                    if (!topicExists) await _context.Topics.AddAsync(topicMatch);
                    newPodcast.PodcastTopics.Add(relationship);
                }
                // Get id for user who is making new podcast
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                // Create new host
                PodcastHost host = new PodcastHost() { HostId = userId, PodcastId = newPodcast.Id };
                // add host to podcast
                newPodcast.PodcastHosts.Add(host);
                // add podcast to db
                await _context.Podcasts.AddAsync(newPodcast);
                // save changes to db
                await _context.SaveChangesAsync();
                // return created if succeeded
                return Created(newPodcast.Id.ToString(), newPodcast);
            }
            // return 500 if error occurred
            catch (Exception ex) { return StatusCode(500, ex); }
        }
    }
}
