using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shortha.Domain.Entites;

namespace Shortha.Infrastructre.Configuration
{
    public class UrlConfiguration : IEntityTypeConfiguration<Url>
    {
        public void Configure(EntityTypeBuilder<Url> builder)
        {
            builder.HasKey(e => e.Id);

            // Properties
            builder.Property(e => e.OriginalUrl)
                   .IsRequired()
                   .HasMaxLength(2048); // Maximum URL length

            builder.Property(e => e.ShortCode)
                   .IsRequired()
                   .HasMaxLength(20);


            builder.Property(e => e.UserId).IsRequired(false);

            builder.Property(e => e.ClickCount)
                   .HasDefaultValue(0);

            builder.Property(e => e.IsActive)
                   .HasDefaultValue(true);

            builder.Property(e => e.CreatedAt);
            // Computed columns
            builder.Ignore(e => e.IsExpired);
            builder.Ignore(e => e.ShortCode);
            builder.Ignore(e => e.UniqueVisitorsCount);
            builder.Ignore(e => e.LastVisitDate);


            // Relationships
            builder.HasOne(e => e.User)
                   .WithMany(e => e.Urls)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Visits)
                   .WithOne(e => e.Url)
                   .HasForeignKey(e => e.UrlId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}