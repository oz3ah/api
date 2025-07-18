using System.Text.Json.Serialization;

namespace Shortha.Application.Dto.Requests.Auth0;

public class Auth0TokenRequest
{
    [JsonPropertyName("client_id")] public string ClientId { get; set; } = null!;

    [JsonPropertyName("client_secret")] public string ClientSecret { get; set; } = null!;

    [JsonPropertyName("audience")] public string Audience { get; set; } = null!;

    [JsonPropertyName("grant_type")] public string GrantType { get; set; } = "client_credentials";
}