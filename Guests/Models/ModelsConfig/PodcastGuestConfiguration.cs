using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guests.Models.ModelsConfig
{
    public class PodcastGuestConfiguration : IEntityTypeConfiguration<PodcastGuest>
    {
        public void Configure(EntityTypeBuilder<PodcastGuest> builder)
        {
            builder.HasOne(pg => pg.User)
            .WithMany(u => u.PodcastGuests)
            .HasForeignKey(pg => pg.GuestId);

            builder.HasOne(pg => pg.Podcast)
            .WithMany(p => p.PodcastGuests)
            .HasForeignKey(pg => pg.PodcastId);
        }
    }
}