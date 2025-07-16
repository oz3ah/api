using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shortha.Domain.Entites;

namespace Shortha.Infrastructre.Configuration
{
    internal class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasConversion<string>();
            builder.HasKey(p => p.Id);

            builder.Property(p => p.MaxUrls)
                   .IsRequired();

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(p => p.DurationInDays)
                   .IsRequired();

            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);
        }
    }
}