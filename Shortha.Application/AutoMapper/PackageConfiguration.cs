using AutoMapper;
using Shortha.Application.Dto.Responses.Package;
using Shortha.Domain.Entites;

namespace Shortha.Application.AutoMapper;

public class PackageConfiguration : Profile
{
    public PackageConfiguration()
    {
        
        CreateMap<Package, PackageInfoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ReverseMap();
        
    }
}