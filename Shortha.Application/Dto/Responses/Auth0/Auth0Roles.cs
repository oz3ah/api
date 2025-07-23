using System.Text.Json.Serialization;

namespace Shortha.Application.Dto.Responses.Auth0;

public class Auth0Roles
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
}