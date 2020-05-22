using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guests.Entities;
using Guests.Helpers;
using Guests.Models;
using Guests.Models.Customizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guests.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class HostsController : ControllerBase
    {
        private readonly AppUserContext _context;
        private UserManager<AppUser> _userManager;
        public HostsController(AppUserContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET api/hosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetHosts()
        {
            // Get all hosts
            IList<AppUser> hosts = await _userManager.GetUsersInRoleAsync(Role.Host);
            // Return 200 if no users found
            if (hosts.Count == 0) return Ok("There are currently no hosts");
            // Populate each host with their respective roles
            await Task.WhenAll(hosts.Select(host => _userManager.PopulateRolesAsync(host)));
            // Return them
            return Ok(hosts);
        }

        // GET api/hosts/5
        [AuthorizeId]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetHostsById(string id)
        {
            // Get matching host
            AppUser host = await _userManager.FindByIdAsync(id);
            // Return 404 if not found
            if (host == null) return NotFound($"The user with id of \"{id}\" was not found");
            // Populate host with roles
            await _userManager.PopulateRolesAsync(host);
            // Return host
            return Ok(host);
        }
    }
}
