using Shortha.Domain.Enums;

namespace Shortha.Domain.Entites
{
    public class Payment : IBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; } = false;
        public string UserId { get; init; } = null!;
        public virtual AppUser User { get; init; } = null!;
        public DateTime? PaymentDate { get; init; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpirationDate { get; init; } = DateTime.UtcNow.AddHours(1);
        public decimal Amount { get; init; }
        public string Currency { get; init; } = "USD";
        public string? PaymentMethod { get; init; }
        public string? TransactionId { get; init; }
        public PaymentStatus Status { get; init; } = PaymentStatus.Pending;

        public required string PackageId { get; init; }
        public virtual Package Package { get; init; } = null!;

        public bool IsExpired => DateTime.UtcNow > ExpirationDate;
    }
}