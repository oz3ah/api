using AutoMapper;
using Shortha.Application.Dto.Responses.AppConnection;
using Shortha.Domain.Entites;

namespace Shortha.Application.AutoMapper;

public class AppConnectionConfiguration : Profile
{
    public AppConnectionConfiguration()
    {
        CreateMap<AppConnection, CreatedConnectionDto>()
            .ForMember(dest => dest.ApiKey, (map) => map.MapFrom(src => src.ConnectKey))
            .ReverseMap();
        CreateMap<AppConnection, UserConnectionDto>()
            .ReverseMap();
    }
}