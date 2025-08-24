using Shortha.Application.Dto.Responses.AppConnection;
using Shortha.Domain.Dto;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;

namespace Shortha.Application.Interfaces.Services;

public interface IAppConnectionService
{
    Task<CreatedConnectionDto> CreateNewConnection(string version, ConnectionDevice device,
        Dictionary<string, object>? deviceMetadata);

    Task<bool> ActivateConnection(string pairCode, string userId);
    Task<AppConnection?> GetByApiKey(string apiKey);

    Task RevokeConnection(string connectionId, string userId);
    Task<PaginationResult<UserConnectionDto>> GetAllByUserId(string userId, int page, int pageSize);
    Task<ConnectionStatusDto> IsConnectedByPairCode(string pairCode);
}