using Shortha.Domain.Entites;

namespace Shortha.Domain.Interfaces.Repositories
{
    public interface IUrlRepository : IGenericRepository<Url>
    {
        Task<bool> IsHashExists(string hash);
        Task<int> GetTotalClicksByUserId(string userId);
    }
}