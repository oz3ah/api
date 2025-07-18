using System.Text.Json.Serialization;

namespace Shortha.Application.Dto.Responses.Auth0;

public class Auth0UserResponse
{
    [JsonPropertyName("user_id")] public string UserId { get; set; } = null!;

    [JsonPropertyName("email")] public string Email { get; set; } = null!;

    [JsonPropertyName("email_verified")] public bool EmailVerified { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [JsonPropertyName("nickname")] public string Nickname { get; set; } = null!;

    [JsonPropertyName("picture")] public string Picture { get; set; } = null!;

    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("last_ip")] public string LastIp { get; set; } = null!;

    [JsonPropertyName("last_login")] public DateTime LastLogin { get; set; }

    [JsonPropertyName("logins_count")] public int LoginsCount { get; set; }

    [JsonPropertyName("identities")] public List<Auth0IdentityDto> Identities { get; set; } = new();
}

public class Auth0IdentityDto
{
    [JsonPropertyName("provider")] public string Provider { get; set; } = null!;

    [JsonPropertyName("user_id")] public string UserId { get; set; } = null!;

    [JsonPropertyName("connection")] public string Connection { get; set; } = null!;

    [JsonPropertyName("isSocial")] public bool IsSocial { get; set; }

    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = null!;

    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
}