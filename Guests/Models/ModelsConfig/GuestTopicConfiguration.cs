using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guests.Models.ModelsConfig
{
    public class GuestTopicConfigurations : IEntityTypeConfiguration<GuestTopic>
    {
        public void Configure(EntityTypeBuilder<GuestTopic> builder)
        {
            builder.HasOne(gt => gt.User)
            .WithMany(u => u.GuestTopics)
            .HasForeignKey(gt => gt.GuestId);

            builder.HasOne(gt => gt.Topic)
            .WithMany(t => t.GuestTopics)
            .HasForeignKey(gt => gt.TopicId);
        }
    }
}