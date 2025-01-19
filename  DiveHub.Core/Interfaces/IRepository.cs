using System.Linq.Expressions;

namespace DiveHub.Core.Interfaces;

public interface IRepository<T>
{
    Task AddAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}