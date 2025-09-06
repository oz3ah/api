using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shortha.Application.Services;

namespace Shortha.Infrastructre.Authentication;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock,
    IApiKeyService apiKeyService)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-API-Key".ToLower(), out var apiKeyHeaderValues))
        {
            return AuthenticateResult.NoResult();
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
        if (string.IsNullOrEmpty(providedApiKey) || !ApiKeyService.IsValidApiKey(providedApiKey))
        {
            return AuthenticateResult.Fail("API Key was not provided or not correct.");
        }

        var apiKey = await apiKeyService.GetApiKeyByKeyAsync(providedApiKey);

        if (apiKey.IsExpired || !apiKey.IsActive)
        {
            return AuthenticateResult.Fail("API Key is either expired or inactive.");
        }

        if (!apiKey.User.IsPremium)
        {
            return AuthenticateResult.Fail("API Key is only allowed for premium users.");
        }

        apiKey.UpdateLastUsed();
        await apiKeyService.Update(apiKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, apiKey.User.Id.ToString()),
            new Claim(ClaimTypes.Name, apiKey.User.Name),
            new Claim("Subscription", "Premium")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}