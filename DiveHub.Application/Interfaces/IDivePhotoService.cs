using DiveHub.Core.Entities;

namespace DiveHub.Application.Interfaces;

public interface IDivePhotoService
{
    Task<List<DivePhoto>> GetAllDivePhotosAsync();
    Task<DivePhoto?> GetDivePhotoByIdAsync(int id);
    
    Task<List<DivePhoto>> GetDivePhotoByDiveIdAsync(int diveId);

    Task AddDivePhotoAsync(DivePhoto divePhoto);
    Task UpdateDivePhotoAsync(DivePhoto divePhoto);
    Task DeleteDivePhotoAsync(int id);
}