using AutoMapper;
using Shortha.Application.Dto.Responses.AppConnection;
using Shortha.Domain.Entites;

namespace Shortha.Application.AutoMapper;

public class AppConnectionConfiguration : Profile
{
    public AppConnectionConfiguration()
    {
        CreateMap<AppConnection, CreatedConnectionDto>().ReverseMap();
    }
}