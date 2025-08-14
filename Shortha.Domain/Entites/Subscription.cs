namespace Shortha.Domain.Entites
{
    public class Subscription : IBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; } = false;


        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public virtual Payment Payment { get; set; } = null!; // Assuming a subscription is linked to a payment
        public required string PaymentId { get; set; } = null!;

        public required string UserId { get; init; }
        public virtual AppUser User { get; init; } = null!;

        public required string PackageId { get; set; }
        public virtual Package Package { get; set; } = null!;
        public string CreatedBy { get; set; } = "system";
        public string UpdatedBy { get; set; } = "system";

        // Helper properties
        public bool IsExpired => DateTime.UtcNow > EndDate;
        public TimeSpan TimeRemaining => EndDate > DateTime.UtcNow ? EndDate - DateTime.UtcNow : TimeSpan.Zero;
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}