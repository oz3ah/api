using Shortha.Application.Dto.Responses.Activity;
using Shortha.Domain.Dto;
using Shortha.Domain.Entites;

namespace Shortha.Application.Interfaces.Services;

public interface IActivityService
{
    Task<PaginationResult<ActivityViewDto>> GetAllActivities(string userId, int page, int pageSize);
}