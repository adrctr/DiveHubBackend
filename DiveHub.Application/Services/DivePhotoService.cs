using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class DivePhotoService(IStorageService<DivePhoto> storageService) : IDivePhotoService
{
    public async Task<List<DivePhoto>> GetAllDivePhotosAsync()=> await storageService.GetAllAsync(); 
    public async Task<DivePhoto?> GetDivePhotoByIdAsync(int id) => await storageService.GetByIdAsync(id);
    
    /// <summary>
    /// Récupérer les photos par diveId
    /// </summary>
    /// <param name="userid"></param>
    /// <returns></returns>
    public async Task<List<DivePhoto>> GetDivePhotoByDiveIdAsync(int diveid)
    {
        var divePhoto = await GetAllDivePhotosAsync();
        return divePhoto.Where(dp => dp.DiveId == diveid).ToList();
    }

    public async Task AddDivePhotoAsync(DivePhoto divePhotoService) => await storageService.AddAsync(divePhotoService);
    public async Task UpdateDivePhotoAsync(DivePhoto divePhotoService) => await storageService.UpdateAsync(divePhotoService);
    public async Task DeleteDivePhotoAsync(int id) => await storageService.DeleteAsync(id);
}