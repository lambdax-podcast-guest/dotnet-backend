using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guests.Models.ModelsConfig
{
    public class PodcastHostConfiguration : IEntityTypeConfiguration<PodcastHost>
    {
        public void Configure(EntityTypeBuilder<PodcastHost> builder)
        {
            builder.HasOne(ph => ph.User)
                .WithMany(u => u.PodcastHosts)
                .HasForeignKey(ph => ph.HostId);

            builder.HasOne(ph => ph.Podcast)
                .WithMany(t => t.PodcastHosts)
                .HasForeignKey(ph => ph.PodcastId);
        }
    }
}
