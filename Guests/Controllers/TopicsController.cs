using System.Collections.Generic;
using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


// We won't need this controller, I am just using it to test if my automatic timestamps are working. Which they are not.
namespace Guests.Controllers
{
    [Route("api/[controller]")]
    public class TopicsController : Controller
    {

        private readonly GuestsContext Context;

        public TopicsController(GuestsContext context)
            => Context = context;



        // Post: api/topics
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Topic input)
        {

            var topic = new Topic()
            {
                Name = input.Name,
            };

            Context.Topics.Add(topic);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = topic.Id }, topic);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopic(int id, [FromBody] Topic input)
        {

            var topic = await Context.Topics.FindAsync(id);

            if (topic == null)
            {
                return NotFound("The topic you are looking for does not appear to be...");
            }

            topic.Name = input.Name;

            await Context.SaveChangesAsync();

            return Ok(topic);
        }

    }
}