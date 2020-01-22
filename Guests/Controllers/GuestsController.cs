using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guests.Controllers{
    [Route("api/[controller]")]
    public class GuestsController : Controller
    {

        private readonly GuestsContext Context;

        public GuestsController(GuestsContext context)
            => Context = context;

        // GET: api/guests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guest>>> GetGuests()
        {

            var guests = await Context.Guests.ToListAsync();

            if(guests.Count == 0){
                return NotFound("Sorry to say but you have no guests...");
            }
            return Ok(guests);
        }

        // GET: api/guests/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Guest>>> GetGuests(int id)
        {

            var guest = await Context.Guests.FindAsync(id);

            if(guest == null){
                return NotFound("No such guest found, please check again...");
            }
            return Ok(guest);
        }

        // Put: api/guests/2
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuest(int id, [FromBody] Guest input){

            var guest = await Context.Guests.FindAsync(id);

            if(guest == null){
                return NotFound("The guest you are looking for does not appear to be...");
            }

            guest.Name = input.Name;
            guest.Email = input.Email;

            await Context.SaveChangesAsync();

            return Ok(guest);
        }

        // Post: api/guests
        [HttpPost]
        public async Task<IActionResult> CreateGuest([FromBody] Guest input){

            var guest = new Guest(){
                Name = input.Name,
                Email = input.Email
            };

            Context.Guests.Add(guest);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGuests), new { id = guest.Id }, guest);
        }

        // Delete: api/guests/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id){

            var guest = await Context.Guests.FindAsync(id);

            if(guest == null){
                return NotFound();
            }

            Context.Remove(guest);

            await Context.SaveChangesAsync();

            if(Context.Guests == null){
                return NotFound("No guests found");
            }
            return Ok(Context.Guests);
        }
    }
}