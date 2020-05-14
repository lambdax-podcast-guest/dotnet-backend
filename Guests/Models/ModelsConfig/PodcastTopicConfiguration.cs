using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guests.Models.ModelsConfig
{
    public class PodcastTopicConfiguration : IEntityTypeConfiguration<PodcastTopic>
    {
        public void Configure(EntityTypeBuilder<PodcastTopic> builder)
        {
            builder.HasOne(pt => pt.Podcast)
            .WithMany(p => p.PodcastTopics)
            .HasForeignKey(pt => pt.PodcastId);

            builder.HasOne(pt => pt.Topic)
            .WithMany(t => t.PodcastTopics)
            .HasForeignKey(pt => pt.TopicId);
        }
    }
}