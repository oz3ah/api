namespace Shortha.Domain.Entites
{
    public class Subscription : IBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; } = false;


        public DateTime StartDate { get; init; } = DateTime.UtcNow;
        public DateTime EndDate { get; init; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public virtual Payment Payment { get; init; } = null!;
        public required string PaymentId { get; init; }

        public required string UserId { get; init; }
        public virtual AppUser User { get; init; } = null!;

        public required string PackageId { get; init; }
        public virtual Package Package { get; init; } = null!;
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        // Helper properties
        public bool IsExpired => DateTime.UtcNow > EndDate;
        public TimeSpan TimeRemaining => EndDate > DateTime.UtcNow ? EndDate - DateTime.UtcNow : TimeSpan.Zero;
        public void Deactivate() => IsActive = false;
    }
}