using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shortha.Domain.Entites;

namespace Shortha.Infrastructre.Configuration;

public class ApiConfiguration : IEntityTypeConfiguration<Api>
{
    public void Configure(EntityTypeBuilder<Api> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasMany(api => api.Urls)
            .WithOne(url => url.Api)
            .HasForeignKey(url => url.ApiKey)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(api => api.User)
            .WithMany(user => user.Apis)
            .HasForeignKey(api => api.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(e => e.Key)
            .IsRequired()
            .HasMaxLength(64)
            .IsUnicode(false);
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);
    }
}