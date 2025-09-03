using AutoMapper;
using Shortha.Application.Dto.Responses.Activity;
using Shortha.Application.Interfaces.Services;
using Shortha.Domain.Dto;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class ActivityService(IActivityRepository repo, IMapper mapper) : IActivityService
{
    public async Task<PaginationResult<ActivityViewDto>> GetAllActivities(string userId, int page, int pageSize)
    {
        var query = await repo.GetAsync(a => a.UserId == userId, page, pageSize, orderBy: a => a.DateUtc, true);


        return mapper.Map<PaginationResult<ActivityViewDto>>(query);
    }
}