using Shortha.Application.Dto.Responses.AppConnection;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;

namespace Shortha.Application.Interfaces.Services;

public interface IAppConnectionService
{
    Task<CreatedConnectionDto> CreateNewConnection(decimal version, ConnectionDevice device,
        Dictionary<string, object>? deviceMetadata);

    Task<AppConnection?> ActivateConnection(string pairCode, string userId);
    Task<AppConnection?> GetByApiKey(string apiKey);

    Task RevokeConnection(string apiKey, string userId);
    Task<UserConnectionDto> GetAllByUserId(string userId, int page, int pageSize);
}