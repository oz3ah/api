namespace Shortha.Domain.Models
{
    public class Subscription
    {
        public string Id { get; set; }


        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public virtual Payment Payment { get; set; } = null!;
        public string PaymentId { get; set; }

        public string UserId { get; set; } = null!;
        public virtual AppUser User { get; set; } = null!;

        public string PackageId { get; set; }
        public virtual Package Package { get; set; } = null!;


        // Helper properties
        public bool IsExpired => DateTime.UtcNow > EndDate;
        public TimeSpan TimeRemaining => EndDate > DateTime.UtcNow ? EndDate - DateTime.UtcNow : TimeSpan.Zero;

        public Subscription()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}