using Shortha.Domain.Dto;
using System.Linq.Expressions;

namespace Shortha.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();

        Task<PaginationResult<T>> GetAsync(Expression<Func<T, bool>>? filter = null,
                                      int pageNumber = 1,
                                      int pageSize = 10,
                                      params string[] includes);

        Task<bool> IsExistsAsync(Expression<Func<T, bool>> filter);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null,
                                      params string[] includes);
    }
}