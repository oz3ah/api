using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shortha.Domain;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class UrlRepository(AppDb context) : IUrlRepository
    {
        public async Task<Url> CreateUrlAsync(Url url, string? customHash = null)
        {
            if (customHash == null)
            {
                var urlHash = HashGenerator.GenerateHash(url.OriginalUrl);

                // Check if the hash already exists: 1/300,000 Probability of collision, but we can handle it
                while (await IsHashExists(urlHash))
                {
                    urlHash = HashGenerator.GenerateHash(url.OriginalUrl);
                }

                url.ShortCode = urlHash;
            }
            else
            {
                url.ShortCode = customHash;
            }

            await context.Urls.AddAsync(url);
            await context.SaveChangesAsync();
            return url;
        }


        public async Task<Url?> FindByAsync(Expression<Func<Url, bool>> filterExpression)
        {
            return await context.Urls.FirstOrDefaultAsync(filterExpression);
        }

        public async Task<IEnumerable<Url>> FindAllByAsync(Expression<Func<Url, bool>> filterExpression, int page = 1)
        {
            return await context.Urls
                                .Where(filterExpression)
                                .Skip((page - 1) * 10)
                                .Take(10)
                                .ToListAsync();
        }

        private async Task<bool> IsHashExists(string hash)
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