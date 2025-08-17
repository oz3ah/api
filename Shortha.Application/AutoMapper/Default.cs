using AutoMapper;
using Shortha.Domain.Dto;

namespace Shortha.Application.AutoMapper;

public class Default : Profile
{
    public Default()
    {
        CreateMap(typeof(PaginationResult<>), typeof(PaginationResult<>))
            .ConvertUsing(typeof(PagedResultConverter<,>));
    }
}