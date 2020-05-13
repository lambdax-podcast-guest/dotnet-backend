using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guests.Models.ModelsConfig
{
    public class PodcastHostConfiguration
    {
        // explanation in podcasttopicconfiguration
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<PodcastHost>()
            .HasOne(ph => ph.User)
            .WithMany(u => u.PodcastHosts)
            .HasForeignKey(ph => ph.HostId);

            builder.Entity<PodcastHost>()
            .HasOne(ph => ph.Podcast)
            .WithMany(t => t.PodcastHosts)
            .HasForeignKey(ph => ph.PodcastId);

        }

    }
}