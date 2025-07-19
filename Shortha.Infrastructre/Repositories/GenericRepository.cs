using Shortha.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            int pageNumber = 1,
            int pageSize = 10,
            params string[] includes
        )
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            var totalCount = await query.CountAsync();
            var items = await query
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

            return (items, totalCount);
        }
    }
}