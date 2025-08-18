namespace Shortha.Application.Dto.Responses.Api;

public class ApiKeyResponse
{
    public string ApiKeyName { get; set; }
    public string Id { get; set; }
    public string MaskedApiKey { get; set; }
    public bool isActive { get; set; }

    public DateTime? LastUsed { get; set; }

    public DateTime? ExpiresAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}