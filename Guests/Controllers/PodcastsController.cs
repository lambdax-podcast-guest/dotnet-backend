using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guests.Entities;
using Guests.Models;
using Guests.Models.Inputs;
using Microsoft.AspNetCore.Authorization;
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

        public PodcastsController(AppUserContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Podcast>>> GetPodcasts()
        {
            try
            {
                // get user claim containing user's id
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                // get all podcasts with user as host
                List<Podcast> podcasts = await _context.Podcasts
                    .Include(p => p.PodcastGuests)
                    .Include(p => p.PodcastHosts)
                    .Include(p => p.PodcastTopics)
                    .Where(p => p.PodcastHosts.Any(ph => ph.HostId == userId))
                    .ToListAsync();
                return Ok(podcasts);
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Podcast>> GetOnePodcast(int id)
        {
            // get user's id
            string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                // find podcast matching id
                Podcast match = await _context.Podcasts
                    .Include(p => p.PodcastGuests)
                    .Include(p => p.PodcastHosts)
                    .Include(p => p.PodcastTopics)
                    .FirstAsync(p => p.Id == id);
                // check to see if user's id is a host
                bool isHost = match.PodcastHosts.Where(ph => ph.PodcastId == match.Id).Any(p => p.HostId == userId);
                // if the user's id doesn't match a host, forbid
                if (!isHost) return Forbid();
                // populate and return match
                return Ok(match);
            }
            // expose exception if fails
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
                int id = 1;
                foreach (string topic in input.Topics)
                {
                    // check for existing topic
                    bool topicExists = _context.Topics.Any(t => t.Name == topic);
                    // either create new topic or find existing topic
                    Topic topicMatch = topicExists ? _context.Topics.First(t => t.Name == topic) : new Topic() { Id = _context.Topics.Count() + id, Name = topic };
                    // create new relationship
                    PodcastTopic relationship = new PodcastTopic() { PodcastId = newPodcast.Id, TopicId = topicMatch.Id };
                    // add topic to db if not exists
                    if (!topicExists)
                    {
                        await _context.Topics.AddAsync(topicMatch);
                        id++;
                    }
                    // add relationship to podcast
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
                return Created(HttpContext.Request.Path, newPodcast.Id);
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }
    }
}
