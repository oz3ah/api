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

    public async Task<CreatedConnectionDto> CreateNewConnection(decimal version, ConnectionDevice device)
    {
        var pairCode = GeneratePairCode();
        var appConnection = new AppConnection()
        {
            Version = version,
            PairCode = pairCode,
            SecretKey = GenerateSecretKey(48),
            Device = device
        };

        await repo.AddAsync(appConnection);
        await repo.SaveAsync();

        return mapper.Map<CreatedConnectionDto>(appConnection);
    }

    public async Task<AppConnection?> ActivateExtension(string pairCode)
    {
        var extension = await repo.GetAsync(e => e.PairCode == pairCode);
        if (extension == null || extension.IsActivated)
        {
            throw new NotFoundException("The Pair Code is not valid or already used");
        }

        var apiKey = GeneratePairCode(); //TEMP: UNTIL WE IMPLEMENT A NEW ALGORITHM;
        extension.IsActivated = true;
        extension.ActivatedAt = DateTime.UtcNow;
        extension.ConnectKey = apiKey;
        repo.Update(extension);
        await repo.SaveAsync();
        return extension;
    }

    public async Task<AppConnection?> GetByApiKey(string apiKey)
    {
        var extension = await repo.GetAsync(e => e.ConnectKey == apiKey && e.IsActivated);
        return extension;
    }
}