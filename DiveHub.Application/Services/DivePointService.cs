using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class DivePointService(IStorageService<DivePoint> storageService)
{
    public Task<List<DivePoint>> GetAllDivesPointAsync() => storageService.GetAllAsync();

    public Task<DivePoint?> GetDivePointByIdAsync(int id) => storageService.GetByIdAsync(id);

    public Task AddDivePointAsync(DivePoint divePointService) => storageService.AddAsync(divePointService);

    public Task UpdateDivePointAsync(DivePoint divePointService) => storageService.UpdateAsync(divePointService);
    public Task DeleteDivePointAsync(int id) => storageService.DeleteAsync(id);
}