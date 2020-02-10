using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;


namespace Guests.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController:ControllerBase{
        [HttpGet]
        public IActionResult Get(){
            return new JsonResult(from c in User.Claims select new {c.Type,c.Value});
        }
    }
}