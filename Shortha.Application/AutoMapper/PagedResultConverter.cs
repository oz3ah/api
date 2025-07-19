using AutoMapper;
using Shortha.Domain.Dto;

namespace Shortha.Application.AutoMapper;

public class PagedResultConverter<TSource, TDestination> :
    ITypeConverter<PaginationResult<TSource>, PaginationResult<TDestination>>
{
    public PaginationResult<TDestination> Convert(
        PaginationResult<TSource> source,
        PaginationResult<TDestination> destination,
        ResolutionContext context)
    {
        return new PaginationResult<TDestination>
               {
                   TotalCount = source.TotalCount,
                   Items = context.Mapper.Map<IEnumerable<TDestination>>(source.Items),
                     PageNumber = source.PageNumber,
                     PageSize = source.PageSize,
                     
                   
                   
               };
    }
}