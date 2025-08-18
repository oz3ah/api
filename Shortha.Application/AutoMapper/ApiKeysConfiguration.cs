using AutoMapper;
using Shortha.Application.Dto.Responses.Api;
using Shortha.Domain.Entites;

namespace Shortha.Application.AutoMapper;

public class ApiKeysConfiguration : Profile
{
    public ApiKeysConfiguration()
    {
        CreateMap<Api, ApiKeyResponse>()
            .ForMember(dest => dest.MaskedApiKey,
                opt => opt.MapFrom(src => src.Key.Substring(0, 12) + "****" + src.Key.Substring(src.Key.Length - 12)))
            .ForMember(dest => dest.isActive,
                opt => opt.MapFrom(src => !src.IsExpired && src.IsActive))
            .ForMember(dest => dest.ApiKeyName,
                opt => opt.MapFrom(src => src.Name ?? "No Name"))
            .ForMember(dest => dest.LastUsed, opt => opt.MapFrom(src => src.LastUsed))
            .ReverseMap();
    }
}