using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;

namespace Shortha.Infrastructre.Configuration;

public class AppConnectionConfiguration : IEntityTypeConfiguration<AppConnection>
{
    public void Configure(EntityTypeBuilder<AppConnection> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .IsRequired();
        builder.Property(e => e.PairCode)
            .IsRequired();
        builder.Property(e => e.Version)
            .IsRequired();
        builder.Property(e => e.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasDefaultValue(ConnectionStatus.Pending);
        builder.Property(e => e.ActivatedAt).HasDefaultValue(null);
        builder.Property(e => e.UserId);
        builder.Property(e => e.ConnectKey)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.Device)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany(e => e.Extensions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasIndex(a => new { a.UserId, a.ConnectKey })
            .IsUnique();
    }
}