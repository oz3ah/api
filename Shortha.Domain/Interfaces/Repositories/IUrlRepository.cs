using Shortha.Domain.Entites;

namespace Shortha.Domain.Interfaces.Repositories
{
    public interface IUrlRepository : IGenericRepository<Url>
    {
        //Task<Url?> FindByAsync(Expression<Func<Url, bool>> filterExpression);
        //Task<IEnumerable<Url>> FindAllByAsync(Expression<Func<Url, bool>> filterExpression, int page = 1);
        //Task<Url> CreateUrlAsync(Url url, string? customHash = null);
        //Task DeleteUrl(string urlId);

        //Task<Url> UpdateUrlAsync(Url url);

        //Task<int> GetTotalCount(string userId);
        Task<bool> IsHashExists(string hash);
    }
}