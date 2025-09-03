using AutoMapper;
using Shortha.Application.Dto.Responses.Activity;
using Shortha.Domain.Entites;

namespace Shortha.Application.AutoMapper;

public class ActivityConfiguration : Profile
{
    public ActivityConfiguration()
    {
        CreateMap<AuditTrail, ActivityViewDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.TrailType, opt => opt.MapFrom(src => src.TrailType.ToString()))
            .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.EntityName))
            .ForMember(dest => dest.DateUtc, opt => opt.MapFrom(src => src.DateUtc))
            .ReverseMap();
    }
}