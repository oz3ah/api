using Shortha.Domain;

namespace Shortha.Application.Services
{
    public interface IVisitService
    {
        Task Record(RequestInfo request, string urlId);
    }
}