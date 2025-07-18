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

        public async Task DeleteUrl(string urlId)
        {
            var url = await context.Urls.FindAsync(urlId);
            if (url != null)
            {
                context.Urls.Remove(url);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Url with ID {urlId} not found.");
            }
        }

        public async Task<Url> UpdateUrlAsync(Url url)
        {
            var existingUrl = await context.Urls.FindAsync(url.Id);
            if (existingUrl == null)
            {
                throw new KeyNotFoundException($"Url with ID {url.Id} not found.");
            }

            context.Urls.Update(existingUrl);
            await context.SaveChangesAsync();
            return existingUrl;
        }

        public async Task<int> GetTotalCount(string userId)
        {
            var count = await context.Urls.CountAsync(u => u.UserId == userId && u.IsActive);

            return count;
        }
    }
}