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
            builder.Entity<AppUser>()
                .ToTable("AppUser")
                .Ignore(p => p.AccessFailedCount)
                .Ignore(p => p.TwoFactorEnabled)
                .Ignore(p => p.LockoutEnd)
                .Ignore(p => p.LockoutEnabled);
            builder.Entity<IdentityRole>().ToTable("Roles");
        }

        public AppUserContext(DbContextOptions<AppUserContext> options) : base(options) { }

        public DbSet<Guest> Guests { get; set; }

    }

}