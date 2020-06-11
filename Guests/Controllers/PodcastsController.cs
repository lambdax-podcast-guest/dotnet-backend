using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guests.Entities;
using Guests.Models;
using Guests.Models.Customizations;
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

        private async Task<Podcast> AddTopics(Podcast p, string[] topics)
        {
            if (p.PodcastTopics == null) p.PodcastTopics = new List<PodcastTopic>();
            int id = 1;
            foreach (string topic in topics)
            {
                // check for existing topic
                bool topicExists = _context.Topics.Any(t => t.Name == topic);
                // either create new topic or find existing topic
                Topic topicMatch = topicExists ? _context.Topics.First(t => t.Name == topic) : new Topic() { Id = _context.Topics.Count() + id, Name = topic };
                // create new relationship
                PodcastTopic relationship = new PodcastTopic() { PodcastId = p.Id, TopicId = topicMatch.Id };
                // add topic to db if not exists
                if (!topicExists)
                {
                    await _context.Topics.AddAsync(topicMatch);
                    id++;
                }
                // add relationship to podcast if the podcast doesn't have it already
                if (!p.PodcastTopics.Any(pt => pt.TopicId == topicMatch.Id)) p.PodcastTopics.Add(relationship);
            }
            return p;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Podcast>>> GetPodcasts()
        {
            try
            {
                // get user claim containing user's id
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                // get all podcasts
                List<Podcast> podcasts = await _context.Podcasts
                    .Include(p => p.PodcastGuests)
                    .Include(p => p.PodcastHosts)
                    .Include(p => p.PodcastTopics)
                    // with user as host
                    .Where(p => p.PodcastHosts.Any(ph => ph.HostId == userId) ||
                        // or guest
                        p.PodcastGuests.Any(pg => pg.GuestId == userId))
                    .ToListAsync();
                return Ok(podcasts);
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }

        [AuthorizePodcast]
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
                // populate and return match
                return Ok(match);
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }

        [Authorize(Roles = Role.Host)]
        [HttpPost]
        public async Task<ActionResult<int>> PostPodcast([FromBody] PodcastInput input)
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            try
            {
                // Create new podcast with input information, not including hosts or guests
                Podcast newPodcast = new Podcast() { Name = input.Name, Description = input.Description, HeadLine = input.HeadLine, PodcastHosts = new List<PodcastHost>() };
                // add topics to podcast
                await AddTopics(newPodcast, input.Topics);
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

        [AuthorizePodcast]
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> UpdatePodcast(int id, [FromBody] PodcastInput input)
        {
            try
            {
                // get podcast matching id
                Podcast match = _context.Podcasts
                    .Include(p => p.PodcastTopics)
                    .First(p => p.Id == id);
                // update all fields
                match.HeadLine = input.HeadLine;
                match.Description = input.Description;
                match.Name = input.Name;
                // add topics to matching podcast
                await AddTopics(match, input.Topics);
                // save the changes
                await _context.SaveChangesAsync();
                // return the id of the matching podcast
                return Ok(match.Id);
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }

        [AuthorizePodcast]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePodcast(int id)
        {
            try
            {
                // get matching podcast
                Podcast match = _context.Podcasts.First(p => p.Id == id);
                // remove podcast
                _context.Podcasts.Remove(match);
                // save changes
                await _context.SaveChangesAsync();
                // return 200
                return Ok();
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }
    }
}
