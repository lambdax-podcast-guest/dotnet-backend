using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guests.Models.ModelsConfig
{
    public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.HasOne(i => i.GuestUser)
            .WithMany(gu => gu.Invitations)
            .HasForeignKey(i => i.GuestId);

            builder.HasOne(i => i.HostUser)
            .WithMany()
            .HasForeignKey(i => i.HostId);

            builder.HasOne(i => i.Podcast)
            .WithMany(p => p.Invitations)
            .HasForeignKey(i => i.PodcastId);
        }
    }
}