using Microsoft.AspNetCore.Identity;

namespace Guests.Helpers
{
    public class DataInitializer
    {
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
