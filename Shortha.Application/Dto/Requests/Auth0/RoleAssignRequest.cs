using System.Text.Json.Serialization;

namespace Shortha.Application.Dto.Requests.Auth0;

public class RoleAssignRequest
{
    [JsonPropertyName("roles")] public List<string> Roles { get; set; } = [];
}