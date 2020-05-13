using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guests.Models.ModelsConfig
{
    public class PodcastTopicConfiguration
    {
        // We need the model builder, but I don't want all this code mucking up our context
        // So rather than writing a configuration to call apply configuration on, we'll just call this custom function inside onmodelcreating
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<PodcastTopic>()
            .HasOne(pt => pt.Podcast)
            .WithMany(p => p.PodcastTopics)
            .HasForeignKey(pt => pt.PodcastId);

            builder.Entity<PodcastTopic>()
            .HasOne(pt => pt.Topic)
            .WithMany(t => t.PodcastTopics)
            .HasForeignKey(pt => pt.TopicId);

        }

    }
}