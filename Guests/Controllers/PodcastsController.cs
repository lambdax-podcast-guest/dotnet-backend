using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guests.Entities;
using Guests.Helpers;
using Guests.Models;
using Guests.Models.Customizations;
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
                List<PodcastHost> hosts = _context.PodcastHosts.Where(ph => ph.HostId == userId).ToList();
                // if no podcasts exist for user, return 404
                if (hosts.Count() == 0) { return NotFound("There are no podcasts assigned to you"); }
                // initiate list of podcasts
                List<Podcast> podcasts = new List<Podcast>();
                foreach (PodcastHost host in hosts)
                {
                    // get podcast that matches host
                    Podcast podcast = _context.Podcasts.First(p => p.Id == host.PodcastId);
                    // populate and add podcast to list
                    podcasts.Add(_context.PopulateRelationships(podcast));
                };
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
                Podcast match = await _context.Podcasts.FirstAsync(p => p.Id == id);
                // check to see if user's id is a host
                bool isHost = _context.PodcastHosts.Where(ph => ph.PodcastId == match.Id).Any(p => p.HostId == userId);
                // populate and return match
                if (isHost) return Ok(_context.PopulateRelationships(match));
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
            // if the user's id doesn't match a host, forbid
            return Forbid();
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
                    Topic topicMatch = topicExists ? _context.Topics.First(t => t.Name == topic) : new Topic() { Id = _context.Topics.Count() + 1, Name = topic };
                    // create new relationship
                    PodcastTopic relationship = new PodcastTopic() { PodcastId = newPodcast.Id, TopicId = topicMatch.Id };
                    // add topic to db if not exists
                    if (!topicExists) await _context.Topics.AddAsync(topicMatch);
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
                return Created(newPodcast.Id.ToString(), newPodcast.Id);
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }
    }
}
