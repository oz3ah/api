using Shortha.Domain.Enums;

namespace Shortha.Domain.Entites
{
    public class Payment : IBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; } = false;
        public string UserId { get; init; } = null!;
        public virtual AppUser User { get; init; } = null!;
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? PaymentLink { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpirationDate { get; init; } = DateTime.UtcNow.AddMinutes(10);
        public decimal Amount { get; init; }
        public string Currency { get; set; } = "USD";
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? CreatedBy { get; set; } = "system";
        public string? UpdatedBy { get; set; } = "system";
        public required string PackageId { get; init; }
        public virtual Package Package { get; init; } = null!;

        public bool IsExpired => DateTime.UtcNow > ExpirationDate;
    }
}