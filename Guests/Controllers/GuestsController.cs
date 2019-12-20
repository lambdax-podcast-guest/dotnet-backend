using System.Linq;
using Guests.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guests.Controllers{
    [Route("api/[controller]")]
    public class GuestsController : Controller
    {

        readonly GuestsContext Context;
        public GuestsController(GuestsContext context)
            => Context = context;

        [HttpGet]
        public IActionResult GetGuest(){
            
            var guests = Context.Guests.ToList();

            return Ok(guests);
        }

        [HttpPost]
        public IActionResult CreateGuest(){
            var guest = new Guest(){
                Name = "Charlie Rogers",
                Email = "charles.rogers2024@gmail.com"
            };

            Context.Add(guest);
            Context.SaveChanges();

            return Ok("Successfully created a new guest...");
        }
    }
}