using System.Linq.Expressions;

namespace DiveHub.Core.Interfaces;

public interface IRepository<T>
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entity);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}