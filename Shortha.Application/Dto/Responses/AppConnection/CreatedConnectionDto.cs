namespace Shortha.Application.Dto.Responses.AppConnection;

public class CreatedConnectionDto
{
    public required string PairCode { get; set; }
    public required string SecretKey { get; set; }

    public required string ApiKey { get; set; }
}