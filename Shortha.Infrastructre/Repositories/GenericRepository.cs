using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Dto;
using Shortha.Domain.Interfaces;
using System.Linq.Expressions;

namespace Shortha.Infrastructre.Repositories
{
    public class GenericRepository<T>(DbContext context) : IGenericRepository<T>
        where T : class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<PaginationResult<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            int pageNumber = 1,
            int pageSize = 10,
            Expression<Func<T, object>>? orderBy = null, bool descending = false,
            params string[] includes
        )
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            var totalCount = await query.CountAsync();


            if (orderBy != null)
            {
                query = descending
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }

            var items = await query
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

            return new PaginationResult<T>
                   {
                       Items = items,
                       TotalCount = totalCount,
                       PageNumber = pageNumber,
                       PageSize = pageSize,
                   };
        }

        public Task<bool> IsExistsAsync(Expression<Func<T, bool>> filter)
        {
            return _dbSet.AnyAsync(filter);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);
            return query.CountAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null,
                                       params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);
            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync();
        }
    }
}