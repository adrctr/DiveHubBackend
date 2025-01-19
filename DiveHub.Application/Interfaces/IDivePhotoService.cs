using DiveHub.Core.Entities;

namespace DiveHub.Application.Interfaces;

public interface IDivePhotoService
{
    Task AddDivePhotoAsync(DivePhotoDto divePhotoDto, int userId);
    Task AddManyDivePhotoAsync(IEnumerable<DivePhotoDto> divePhotoDtos);
    Task<DivePhotoDto?> GetDivePhotoByIdAsync(int divePhotoId);
    Task<IEnumerable<DivePhotoDto>> GetAllDivePhotosAsync();
    Task UpdateDivePhotoAsync(DivePhotoDto divePhotoDto);
    Task DeleteDivePhotoAsync(int divePhotoId);
}