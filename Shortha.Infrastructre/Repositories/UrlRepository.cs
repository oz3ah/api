using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class UrlRepository(AppDb context) : GenericRepository<Url>(context), IUrlRepository
    {
        public async Task<bool> IsHashExists(string hash)
        {
            return await context.Urls.AnyAsync(x => x.ShortCode == hash);
        }

        public async Task<int> GetTotalClicksByUserId(string userId)
        {
            return await context.Urls
                                .Where(x => x.UserId == userId)
                                .SumAsync(x => x.ClickCount);
        }
    }
}