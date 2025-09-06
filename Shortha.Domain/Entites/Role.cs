namespace Shortha.Domain.Entites;

public class Role : IBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;


    public virtual ICollection<AppUser> Users { get; set; } = [];
}