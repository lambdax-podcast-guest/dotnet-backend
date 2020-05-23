using System.Linq;
using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Identity;

namespace Guests.Helpers
{
    // A class containing methods that can be used by specific types
    // i.e. _userManager.PopulateRolesAsync(guest)
    public static class ExtensionMethods
    {
        /// <summary>
        /// Populates an AppUser with its respective roles
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Async AppUser with roles</returns>
        public static async Task<AppUser> PopulateRolesAsync(this UserManager<AppUser> userManager, AppUser user)
        {
            // Await roles from UserManager
            var roles = await userManager.GetRolesAsync(user);
            // Add roles to AppUser
            user.Roles = roles.ToArray();
            // Return the user with its roles
            return user;
        }
    }
}
