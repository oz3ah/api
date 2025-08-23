using Shortha.Application.Dto.Responses.AppConnection;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;

namespace Shortha.Application.Interfaces.Services;

public interface IAppConnectionService
{
    Task<CreatedConnectionDto> CreateNewConnection(decimal version, ConnectionDevice device,
        Dictionary<string, object>? deviceMetadata);

    Task<AppConnection?> ActivateExtension(string pairCode);
    Task<AppConnection?> GetByApiKey(string apiKey);
}