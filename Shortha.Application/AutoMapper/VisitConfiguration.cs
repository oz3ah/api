using AutoMapper;
using Shortha.Application.Dto.Responses.Visit;
using Shortha.Domain.Entites;

namespace Shortha.Application.AutoMapper
{
    public class VisitConfiguration : Profile
    {
        public VisitConfiguration()
        {

            CreateMap<Visit, VisitsResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Browser, opt => opt.MapFrom(src => src.Browser))
                .ForMember(dest => dest.Os, opt => opt.MapFrom(src => src.Os))
                .ForMember(dest => dest.DeviceBrand, opt => opt.MapFrom(src => src.DeviceBrand))
                .ForMember(dest => dest.DeviceType, opt => opt.MapFrom(src => src.DeviceType))
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.VisitDate, opt => opt.MapFrom(src => src.VisitDate))

                .ReverseMap();

        }
    }
}
