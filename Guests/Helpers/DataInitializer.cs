using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Guests.Entities;

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
            if (!roleManager.RoleExistsAsync(Role.Admin).Result)
            {
                IdentityRole role = new IdentityRole() { Name = Role.Admin };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(Role.Host).Result)
            {
                IdentityRole role = new IdentityRole() { Name = Role.Host };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(Role.Guest).Result)
            {
                IdentityRole role = new IdentityRole() { Name = Role.Guest };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
