using AutoMapper;
using Shortha.Application.Dto.Requests.Url;
using Shortha.Application.Dto.Responses.Url;
using Shortha.Domain.Entites;

namespace Shortha.Application.AutoMapper
{
    public class UrlConfiguration : Profile
    {
        public UrlConfiguration()
        {
            CreateMap<Url, UrlResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.OriginalUrl))
                .ForMember(dest => dest.ShortCode, opt => opt.MapFrom(src => src.ShortCode))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ExpiredAt, opt => opt.MapFrom(src => src.ExpiresAt))
                .ForMember(dest => dest.ClickCount, opt => opt.MapFrom(src => src.ClickCount))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive)).ReverseMap();

            CreateMap<Url, UrlCreateRequest>().ReverseMap();
            CreateMap<Url, PublicUrlResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.OriginalUrl))
                .ReverseMap();
        }
    }
}