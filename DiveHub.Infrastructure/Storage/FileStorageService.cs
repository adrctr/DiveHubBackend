using System.Text.Json;
using DiveHub.Core.Interfaces;

namespace DiveHub.Infrastructure.Storage;

public class FileStorageService<T> : IStorageService<T> where T : class
{
    private readonly string _filePath;

    public FileStorageService(string filePath)
    {
        _filePath = filePath;

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<List<T>> GetAllAsync()
    {
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var items = await GetAllAsync();
        var propertyInfo = typeof(T).GetProperty("UserId") ?? typeof(T).GetProperty($"{typeof(T).Name}Id");
        return propertyInfo != null
            ? items.FirstOrDefault(item => propertyInfo.GetValue(item)?.Equals(id) == true)
            : null;
    }

    public async Task AddAsync(T item)
    {
        var items = await GetAllAsync();
        var propertyInfo = typeof(T).GetProperty("UserId") ?? typeof(T).GetProperty($"{typeof(T).Name}Id");

        if (propertyInfo != null && propertyInfo.PropertyType == typeof(int))
        {
            var maxId = items.Select(i => (int)propertyInfo.GetValue(i)!).DefaultIfEmpty(0).Max();
            propertyInfo.SetValue(item, maxId + 1);
        }

        items.Add(item);
        await SaveAllAsync(items);
    }

    public async Task UpdateAsync(T item)
    {
        var items = await GetAllAsync();
        var propertyInfo = typeof(T).GetProperty("UserId") ?? typeof(T).GetProperty($"{typeof(T).Name}Id");

        if (propertyInfo == null)
        {
            throw new InvalidOperationException("ID property not found");
        }

        var existingItem =
            items.FirstOrDefault(i => propertyInfo.GetValue(i)?.Equals(propertyInfo.GetValue(item)) == true);
        if (existingItem == null)
        {
            throw new KeyNotFoundException("Item not found");
        }

        items.Remove(existingItem);
        items.Add(item);
        await SaveAllAsync(items);
    }

    public async Task DeleteAsync(int id)
    {
        var items = await GetAllAsync();
        var propertyInfo = typeof(T).GetProperty("UserId") ?? typeof(T).GetProperty($"{typeof(T).Name}Id");

        if (propertyInfo == null)
        {
            throw new InvalidOperationException("ID property not found");
        }

        var item = items.FirstOrDefault(i => propertyInfo.GetValue(i)?.Equals(id) == true);
        if (item == null)
        {
            throw new KeyNotFoundException("Item not found");
        }

        items.Remove(item);
        await SaveAllAsync(items);
    }

    private async Task SaveAllAsync(List<T> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json);
    }
}