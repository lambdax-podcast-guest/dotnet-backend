using System;
using Microsoft.EntityFrameworkCore;
using Guests.Models.ModelsConfig;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Guests.Helpers;



namespace Guests.Models
{

    public class AppUserContext : IdentityDbContext<AppUser>
    {
        protected IWebHostEnvironment WebHostEnv { get; }
        public AppUserContext(DbContextOptions<AppUserContext> options, IWebHostEnvironment webHostEnv)
            : base(options)
        {
            WebHostEnv = webHostEnv;
        }
        // Inherit from this TimestampEntity class for your model to have CreatedAt and UpdatedAt Timestamps that are updated automatically. Unfortunately this will not work with our Identity Classes, as they already inherit from Identity
        public class TimestampEntity
        {
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GuestsConfiguration());
            // many to many podcast to topics
            builder.ApplyConfiguration(new PodcastTopicConfiguration());
            // many to many podcast to host
            builder.ApplyConfiguration(new PodcastHostConfiguration());
            // many to many guest to topic
            builder.ApplyConfiguration(new GuestTopicConfiguration());
            // many to many podcast to guest
            builder.ApplyConfiguration(new PodcastGuestConfiguration());
            // many to many podcast to guest and host
            builder.ApplyConfiguration(new InvitationConfiguration());
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<AppUser>()
                .ToTable("Users")
                .Ignore(p => p.AccessFailedCount)
                .Ignore(p => p.TwoFactorEnabled)
                .Ignore(p => p.LockoutEnd)
                .Ignore(p => p.LockoutEnabled);

            // get topics from DataInitializer and seed to db
            var contentRootPath = WebHostEnv.ContentRootPath;
            string[] topicList = DataInitializer.Topics;
            int count = 1;
            foreach (string topic in topicList)
            {
                builder.Entity<Topic>()
                .HasData
                (
                    new Topic
                    {
                        Id = count,
                        Name = topic,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                );
                count++;
            }
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

        public DbSet<Podcast> Podcasts { get; set; }

        public DbSet<PodcastTopic> PodcastTopics { get; set; }

        public DbSet<GuestTopic> GuestTopics { get; set; }

        public DbSet<PodcastHost> PodcastHosts { get; set; }

        public DbSet<PodcastGuest> PodcastGuests { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

    }

}