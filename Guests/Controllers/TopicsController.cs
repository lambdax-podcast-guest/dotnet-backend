using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guests.Entities;
using Guests.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guests.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly AppUserContext context;
        public TopicsController(AppUserContext _context) { context = _context; }

        [HttpGet]
        public async Task<ActionResult<List<Topic>>> GetAllTopics()
        {
            try
            {
                List<Topic> topics = await context.Topics
                    .Include(t => t.PodcastTopics)
                    .ToListAsync();
                return Ok(topics);
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetOneTopic(int id)
        {
            if (context.Topics.Find(id) == null) return NotFound();
            try
            {
                Topic match = await context.Topics
                    .Include(t => t.PodcastTopics)
                    .ThenInclude(p => p.Podcast)
                    .FirstAsync(t => t.Id == id);
                return Ok(match);
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> PostTopic([FromBody] Topic input)
        {
            try
            {
                Topic newTopic = new Topic() { Name = input.Name };
                await context.Topics.AddAsync(newTopic);
                await context.SaveChangesAsync();
                return Created(HttpContext.Request.Path, newTopic.Id);
            }
            // expose exception if fails
            catch (Exception ex) { return StatusCode(500, ex); }
        }
    }
}
