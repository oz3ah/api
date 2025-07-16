using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shortha.Domain.Entites;

namespace Shortha.Infrastructre.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // Properties
            builder.HasKey(e => e.Id);
            builder.Property(e => e.IsPremium)
                   .HasDefaultValue(false);

            builder.Property(e => e.IsBlocked)
                   .HasDefaultValue(false);


            // Relationships
            builder.HasMany(e => e.Urls)
                   .WithOne(e => e.User)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(e => e.Payments)
                   .WithOne(e => e.User)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}