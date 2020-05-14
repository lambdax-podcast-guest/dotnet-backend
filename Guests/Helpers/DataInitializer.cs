using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Guests.Models;

namespace Guests.Helpers
{
    public class DataInitializer
    {
        // Topics property will hold a list of topics to be seeded to the db
        public static readonly string[] Topics = new string[] { "Sports", "Literature" };
        public static void SeedData(RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Host").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Host";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Guest").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Guest";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
