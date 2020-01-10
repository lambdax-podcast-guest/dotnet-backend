using Microsoft.EntityFrameworkCore;
using Guests.Models.ModelsConfig;

namespace Guests.Models
{
    public class GuestsContext : DbContext
    {
        public GuestsContext(DbContextOptions<GuestsContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder) 
            => builder.ApplyConfiguration(new GuestsConfigurations());

        public DbSet<Guest> Guests { get; set; }
    }

}