using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guests.Models.ModelsConfig
{
    public class GuestTopicConfiguration
    {
        // explanation in podcasttopicconfiguration
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<GuestTopic>()
            .HasOne(pt => pt.User)
            .WithMany(p => p.GuestTopics)
            .HasForeignKey(pt => pt.GuestId);

            builder.Entity<GuestTopic>()
            .HasOne(pt => pt.Topic)
            .WithMany(t => t.GuestTopics)
            .HasForeignKey(pt => pt.TopicId);

        }

    }
}