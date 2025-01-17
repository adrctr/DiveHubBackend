using DiveHub.Core.Entities;

namespace DiveHub.Core.Interfaces;

public interface IStorageService<T> where T : class 
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T item);
    Task UpdateAsync(T item);
    Task DeleteAsync(int id);
}