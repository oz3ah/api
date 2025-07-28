using Shortha.Domain.Enums;

namespace Shortha.Domain.Entites
{
    public class Package : IBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; } = false;
        public PackagesName Name { get; init; }
        public string Description { get; init; } = null!;
        public int MaxUrls { get; init; }
        public decimal Price { get; init; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public int DurationInDays { get; init; } = 365;

        public virtual ICollection<Subscription> Subscriptions { get; init; } = new HashSet<Subscription>();
        public virtual ICollection<Payment> Payments { get; init; } = new HashSet<Payment>();
    }
}