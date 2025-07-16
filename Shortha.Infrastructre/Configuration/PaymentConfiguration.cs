using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shortha.Domain.Entites;

namespace Shortha.Infrastructre.Configuration
{
    internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(e => e.Id);


            // Properties
            builder.Property(e => e.UserId)
                   .IsRequired();

            builder.Property(e => e.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(e => e.Currency)
                   .HasMaxLength(3);

            builder.Property(e => e.PaymentMethod)
                   .HasMaxLength(50);

            builder.Property(e => e.TransactionId);

            builder.Property(e => e.CreatedAt);

            builder.Property(e => e.ExpirationDate);


            // Relationships
            builder.HasOne(e => e.User)
                   .WithMany(e => e.Payments)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Package)
                   .WithMany(e => e.Payments)
                   .HasForeignKey(e => e.PackageId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}