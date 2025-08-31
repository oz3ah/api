using Shortha.Domain.Enums;

namespace Shortha.Application.Dto.Responses.AppConnection;

public class UserConnectionDto
{
    public required string Id { get; set; }
    public string? Status { get; set; }
    public string? Device { get; set; }
    public required string PairCode { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public string Version { get; set; }
}