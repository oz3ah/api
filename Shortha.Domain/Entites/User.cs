namespace Shortha.Domain.Entites
{
    public class AppUser
    {
        public string Id { get; set; }
        public bool IsPremium { get; set; } = false;
        public bool IsBlocked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}