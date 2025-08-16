using Shortha.Domain.Enums;

namespace Shortha.Domain.Entites
{
    public class Url : IBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; } = false;
        public string OriginalUrl { get; init; } = null!;
        public string ShortCode { get; set; } = null!;
        public int ClickCount { get; private set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public DateTime? ExpiresAt { get; init; }
        public bool IsActive { get; private set; } = true;

        public string? UserId { get; set; }
        public virtual AppUser? User { get; init; }
        public UrlCreationSource CreationSource { get; set; }

        public virtual ICollection<Visit> Visits { get; set; } = new HashSet<Visit>();

        public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;

        public int UniqueVisitorsCount => Visits?.Select(v => v.IpAddress).Distinct().Count() ?? 0;

        public DateTime? LastVisitDate => Visits?.OrderByDescending(v => v.VisitDate).FirstOrDefault()?.VisitDate;

        public string? CreatedBy { get; set; } = "system";
        public string? UpdatedBy { get; set; } = "system";

        public void IncrementClick() => ClickCount++;
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}