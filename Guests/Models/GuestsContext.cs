using System;
using Microsoft.EntityFrameworkCore;
using Guests.Models.ModelsConfig;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Guests.Models
{

    public class GuestsContext : IdentityDbContext<AppUser>
    {
        public GuestsContext(DbContextOptions<GuestsContext> options)
            : base(options)
        {

        }
        // Inherit from this TimestampEntity class for your model to have CreatedAt and UpdatedAt Timestamps that are updated automatically. Unfortunately this will not work with our Identity Classes, as they already inherit from Identity
        public class TimestampEntity
        {
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GuestsConfigurations());
            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Automatically set updated at and created at on all created and updated entries from all tables
            // Use ChangeTracker to get all updated and added entries from the db
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is TimestampEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                // We only got modified and newly added entities, set the updated timestamp on all of them

                ((TimestampEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                // If the entry has just been added update its created at
                if (entityEntry.State == EntityState.Added)
                {
                    ((TimestampEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }
            return (await base.SaveChangesAsync(true, cancellationToken));
        }

        public DbSet<Guest> Guests { get; set; }

        public DbSet<Topic> Topics { get; set; }

    }

}