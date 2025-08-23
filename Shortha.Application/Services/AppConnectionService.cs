using AutoMapper;
using Shortha.Application.Dto.Responses.AppConnection;
using Shortha.Application.Exceptions;
using Shortha.Application.Interfaces.Services;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class AppConnectionService(IAppConnectionRepository repo, IMapper mapper) : IAppConnectionService
{
    private string GeneratePairCode()
    {
        int length = 6;
        const string chars = "1234567890";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private string GenerateSecretKey(int length = 32)
    {
        const string chars = "123456780-=+!@#$%^&*()_QWERTYUIOP{}ASDFGHJKL:ZXCVBNM<>?";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public async Task<CreatedConnectionDto> CreateNewConnection(decimal version, ConnectionDevice device,
        Dictionary<string, object>? deviceMetadata)
    {
        var pairCode = GeneratePairCode();
        var appConnection = new AppConnection()
        {
            Version = version,
            PairCode = pairCode,
            SecretKey = GenerateSecretKey(48),
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

    public async Task<AppConnection?> ActivateConnection(string pairCode, string userId)
    {
        var appConnection = await repo.GetAsync(e => e.PairCode == pairCode);
        if (appConnection == null || appConnection.IsValid())
        {
            throw new NotFoundException("The Pair Code is not valid or already used");
        }

        var apiKey = GeneratePairCode(); //TEMP: UNTIL WE IMPLEMENT A NEW ALGORITHM;
        appConnection.Activate();
        appConnection.ActivatedAt = DateTime.UtcNow;
        appConnection.ConnectKey = apiKey;
        appConnection.UserId = userId;
        repo.Update(appConnection);
        await repo.SaveAsync();
        return appConnection;
    }

    public async Task<AppConnection?> GetByApiKey(string apiKey)
    {
        var connection = await repo.GetAsync(e => e.ConnectKey == apiKey);
        if (connection == null || !connection.IsValid())
        {
            return null;
        }

        return connection;
    }

    public async Task RevokeConnection(string apiKey, string userId)
    {
        var existingConnection = await repo.GetAsync(e => e.ConnectKey == apiKey && e.UserId == userId);
        if (existingConnection == null || !existingConnection.IsValid())
        {
            throw new NotFoundException("The connection is not valid or already revoked");
        }

        existingConnection.Revoke();
        repo.Update(existingConnection);
        await repo.SaveAsync();
    }

    public async Task<UserConnectionDto> GetAllByUserId(string userId, int page, int pageSize)
    {
        var connections = await repo.GetAsync(e => e.UserId == userId, page, pageSize);
        return mapper.Map<UserConnectionDto>(connections);
    }
}