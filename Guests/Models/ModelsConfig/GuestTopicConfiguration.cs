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
            .HasOne(gt => gt.User)
            .WithMany(u => u.GuestTopics)
            .HasForeignKey(gt => gt.GuestId);

            builder.Entity<GuestTopic>()
            .HasOne(gt => gt.Topic)
            .WithMany(t => t.GuestTopics)
            .HasForeignKey(gt => gt.TopicId);

        }

    }
}