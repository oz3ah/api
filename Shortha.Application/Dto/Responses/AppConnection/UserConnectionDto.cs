namespace Shortha.Application.Dto.Responses.AppConnection;

public class UserConnectionDto
{
    public string? ConnectKey { get; set; }
    public string? Status { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public decimal Version { get; set; }
}