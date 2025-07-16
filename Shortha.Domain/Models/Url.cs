namespace Shortha.Domain.Models
{
    public class Url
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string OriginalUrl { get; set; } = null!;
        public string ShortCode { get; set; } = null!;
        public int ClickCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public virtual ICollection<Visit> Visits { get; set; } = new HashSet<Visit>();

        public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
        public string ShortUrl => $"https://shortha.com/{ShortCode}";
        public int UniqueVisitorsCount => Visits.Select(v => v.IpAddress).Distinct().Count();
        public DateTime? LastVisitDate => Visits.OrderByDescending(v => v.VisitDate).FirstOrDefault()?.VisitDate;
    }
}