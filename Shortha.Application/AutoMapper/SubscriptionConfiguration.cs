using AutoMapper;
using Shortha.Application.Dto.Responses.Subscription;
using Shortha.Domain.Entites;

namespace Shortha.Application.AutoMapper;

public class SubscriptionConfiguration : Profile
{
    public SubscriptionConfiguration()
    {
        CreateMap<Subscription, SubscriptionCreationResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Package.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Package.Price))
            .ForMember(dest => dest.PaymentLink, opt => opt.MapFrom(src => src.Payment.PaymentLink))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Payment.Status))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
    }
}