using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string? Name { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastUsed { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}