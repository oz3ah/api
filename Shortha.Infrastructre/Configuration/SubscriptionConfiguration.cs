using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shortha.Domain.Entites;

namespace Shortha.Infrastructre.Configuration
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(e => e.Id);


            // Properties
            builder.Property(e => e.UserId)
                   .IsRequired()
                ;
            builder.Property(e => e.StartDate)
                   .IsRequired();

            builder.Property(e => e.IsActive)
                   .HasDefaultValue(true);

            builder.Property(e => e.EndDate)
                   .IsRequired();

            builder.Property(e => e.CreatedAt);

            builder.Ignore(e => e.IsExpired);
            builder.Ignore(e => e.TimeRemaining);


            // Relationships
            builder.HasOne(e => e.User);

            builder.HasOne(e => e.Package)
                   .WithMany(e => e.Subscriptions)
                   .HasForeignKey(e => e.PackageId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}