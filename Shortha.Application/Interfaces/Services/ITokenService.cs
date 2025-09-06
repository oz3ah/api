namespace Shortha.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(string userId, string email, string role, int expireMinutes = 60);
}