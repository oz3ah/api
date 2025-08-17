namespace Shortha.Application.Dto.Requests.Api;

public class CreateApiKeyDto
{
    public required string KeyName { get; set; }
    public DateTime? ExpirationDate { get; set; }
}