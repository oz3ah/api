using Shortha.Domain.Enums;

namespace Shortha.Domain.Entites
{
    public class Package
    {
        public string Id { get; set; }
        public PackagesNames Name { get; set; }
        public string Description { get; set; } = null!;
        public int MaxUrls { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int DurationInDays { get; set; } = 365;

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
        public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

        public Package()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}