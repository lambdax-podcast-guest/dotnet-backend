using Microsoft.EntityFrameworkCore;
using Guests.Models.ModelsConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Guests.Models
{
    public class GuestsContext : IdentityDbContext<AppUser>
    {
        public GuestsContext(DbContextOptions<GuestsContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GuestsConfigurations());
            base.OnModelCreating(builder);                                        
        }

        public DbSet<Guest> Guests { get; set; }

        //public DbSet<User> Users { get; set; }

    }

}