using AutoMapper;
using Shortha.Application.Dto.Responses.AppConnection;
using Shortha.Application.Exceptions;
using Shortha.Application.Interfaces.Services;
using Shortha.Domain.Dto;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class AppConnectionService(IAppConnectionRepository repo, IMapper mapper) : IAppConnectionService
{
    public async Task<CreatedConnectionDto> CreateNewConnection(string version, ConnectionDevice device,
        Dictionary<string, object>? deviceMetadata)
    {
        var pairCode = Crypto.GeneratePairCode();
        var appConnection = new AppConnection()
        {
            Version = version,
            PairCode = pairCode,
            SecretKey = Crypto.GenerateSecretKey(48),
            Device = device,
            DeviceMetadata = deviceMetadata?.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.ToString() ?? string.Empty
            )
        };

        await repo.AddAsync(appConnection);
        await repo.SaveAsync();

        return mapper.Map<CreatedConnectionDto>(appConnection);
    }

    public async Task<bool> ActivateConnection(string pairCode, string userId)
    {
        var appConnection = await repo.GetAsync(e => e.PairCode == pairCode);
        if (appConnection == null || appConnection.IsRevoked() || appConnection.IsValid())
        {
            throw new NotFoundException("The Pair Code is not valid or already used");
        }

        var apiKey = Crypto.GenerateApiToken();
        appConnection.Activate();
        appConnection.ActivatedAt = DateTime.UtcNow;
        appConnection.ConnectKey = apiKey;
        appConnection.UserId = userId;
        repo.Update(appConnection);
        await repo.SaveAsync();

        return true;
    }

    public async Task<AppConnection?> GetByPairCode(string pairCode)
    {
        var connection = await repo.GetAsync(e => e.PairCode == pairCode);
        return connection;
    }

    public async Task RevokeConnection(string pairCode, string userId)
    {
        var existingConnection = await repo.GetAsync(e => e.PairCode == pairCode && e.UserId == userId);
        if (existingConnection == null || !existingConnection.IsValid())
        {
            throw new NotFoundException("The connection is not valid or already revoked");
        }

        existingConnection.Revoke();
        await repo.SaveAsync();
    }

    public async Task<PaginationResult<UserConnectionDto>> GetAllByUserId(string userId, int page, int pageSize)
    {
        var connections = await repo.GetAsync(e => e.UserId == userId, page, pageSize, orderBy: c => c.CreatedAt, true);
        return mapper.Map<PaginationResult<UserConnectionDto>>(connections);
    }

    public async Task<ConnectionStatusDto> IsConnectedByPairCode(string pairCode)
    {
        var connection = await repo.GetAsync(e => e.PairCode == pairCode, includes: ["User"]);


        return new ConnectionStatusDto()
        {
            IsActive = connection?.IsValid() ?? false,
            IsRevoked = connection?.IsRevoked() ?? false,
            IsExist = connection != null,
            ConnectedSince = connection?.ActivatedAt,
            Username = connection?.User?.Name,
            Email = connection?.User?.Email,
            Photo = connection?.User?.Picture
        };
    }
}