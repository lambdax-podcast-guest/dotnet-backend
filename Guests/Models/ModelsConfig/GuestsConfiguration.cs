using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guests.Models.ModelsConfig
{
    public class GuestsConfigurations : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Name)
            .HasMaxLength(200)
            .IsRequired();

            builder.Property(prop => prop.Email)
            .HasMaxLength(200)
            .IsRequired();

        }

    }
}