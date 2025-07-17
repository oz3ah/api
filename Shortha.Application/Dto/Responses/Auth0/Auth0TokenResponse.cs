using System.Text.Json.Serialization;

namespace Shortha.Application.Dto.Responses.Auth0;

public class Auth0TokenResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = null!;

    [JsonPropertyName("token_type")] public string TokenType { get; set; } = null!;
}
