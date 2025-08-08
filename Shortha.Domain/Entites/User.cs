namespace Shortha.Domain.Entites
{
    public class AppUser : IBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; } = false;
        public bool IsPremium { get; set; } = false;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public bool IsBlocked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = "system";
        public string UpdatedBy { get; set; } = "system";

        public string Provider { get; set; } = null!;

        public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}