using Shortha.Domain.Enums;

namespace Shortha.Domain.Models
{
    public class Payment
    {
        public string Id { get; set; }
        public string UserId { get; set; } = null!;
        public virtual AppUser User { get; set; } = null!;
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddHours(1);
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string PackageId { get; set; }
        public virtual Package Package { get; set; } = null!;

        public bool IsExpired => DateTime.UtcNow > ExpirationDate;

        public Payment()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}