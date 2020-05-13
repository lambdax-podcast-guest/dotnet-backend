using Microsoft.EntityFrameworkCore;
using Guests.Models.ModelsConfig;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace Guests.Models
{
    public class AppUserContext : IdentityDbContext<AppUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GuestsConfigurations());
            base.OnModelCreating(builder);
            builder.Entity<AppUser>().ToTable("AppUser");
            builder.Entity<IdentityRole>().ToTable("Roles");
        }

        public AppUserContext(DbContextOptions<AppUserContext> options) : base(options) { }

        public DbSet<Guest> Guests { get; set; }

    }

}