using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shortha.Domain.Entites;

namespace Shortha.Infrastructre.Configuration
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.HasKey(e => e.Id);

            // Properties
            builder.Property(e => e.VisitDate);

            builder.Property(e => e.UserAgent)
                   .HasMaxLength(1000);

            builder.Property(e => e.IpAddress)
                   .HasMaxLength(45); // IPv6 max length

            builder.Property(e => e.Referrer)
                   .HasMaxLength(2048);

            builder.Property(e => e.Country)
                   .HasMaxLength(100);

            builder.Property(e => e.Region)
                   .HasMaxLength(100);

            builder.Property(e => e.City)
                   .HasMaxLength(100);

            builder.Property(e => e.DeviceBrand)
                   .HasMaxLength(50);

            builder.Property(e => e.DeviceType)
                   .HasMaxLength(50);

            builder.Property(e => e.Browser)
                   .HasMaxLength(50);

            builder.Property(e => e.Os)
                   .HasMaxLength(50);

            builder.Property(e => e.Language)
                   .HasMaxLength(10);

            builder.Property(e => e.TimeZone)
                   .HasMaxLength(50);

            builder.Property(e => e.IsBot)
                   .HasDefaultValue(false);

            // Computed columns
            builder.Ignore(e => e.DeviceInfo);
            builder.Ignore(e => e.LocationInfo);
            builder.Ignore(e => e.IsUnique);


            // Relationships
            builder.HasOne(e => e.Url)
                   .WithMany(e => e.Visits)
                   .HasForeignKey(e => e.UrlId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}