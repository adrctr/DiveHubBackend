using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class DiveService(IStorageService<Dive> storageService)
{
    public Task<List<Dive>> GetAllDivesAsync() => storageService.GetAllAsync();

    public Task<Dive?> GetDiveByIdAsync(int id) => storageService.GetByIdAsync(id);

    public Task AddDiveAsync(Dive dive) => storageService.AddAsync(dive);

    public Task UpdateDiveAsync(Dive dive) => storageService.UpdateAsync(dive);

    public Task DeleteDiveAsync(int id) => storageService.DeleteAsync(id);
}
