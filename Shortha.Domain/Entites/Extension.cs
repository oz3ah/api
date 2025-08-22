namespace Shortha.Domain.Entites;

public class Extension : IBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public required string PairCode { get; set; }
    public bool IsActivated { get; set; } = false;
    public DateTime? ActivatedAt { get; set; } = null;
    public string? UserId { get; set; }
    public required decimal Version { get; set; }
    public string? ApiKey { get; set; }

    public AppUser? User { get; set; }
}