namespace Shortha.Domain.Entites
{
    public class Api : IBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string Key { get; set; } = null!;
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public virtual AppUser User { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime? LastUsed { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
        public void UpdateLastUsed() => LastUsed = DateTime.UtcNow;

        public virtual List<Url> Urls { get; set; } = [];
    }
}