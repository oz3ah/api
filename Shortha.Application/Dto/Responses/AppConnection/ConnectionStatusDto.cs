namespace Shortha.Application.Dto.Responses.AppConnection;

public class ConnectionStatusDto()
{
    public bool IsActive { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsExist { get; set; }

    public DateTime? ConnectedSince { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Photo { get; set; }
}