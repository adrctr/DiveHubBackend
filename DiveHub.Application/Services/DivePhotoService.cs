using AutoMapper;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class DivePhotoService(IRepository<DivePhoto> divePhotoRepository, IMapper mapper) : IDivePhotoService
{
    public async Task AddDivePhotoAsync(DivePhotoDto divePhotoDto, int diveId)
    {
        var divePhoto = mapper.Map<DivePhoto>(divePhotoDto);
        divePhoto.DiveId = diveId;
        await divePhotoRepository.AddAsync(divePhoto);
    }

    public async Task AddManyDivePhotoAsync(IEnumerable<DivePhotoSaveDto> divePhotoSaveDtos)
    {
        var divePhotos = mapper.Map<IEnumerable<DivePhoto>>(divePhotoSaveDtos);
        // Ajouter tous les DivePhoto en une seule opération
        await divePhotoRepository.AddRangeAsync(divePhotos);
    }
    public async Task<DivePhotoDto?> GetDivePhotoByIdAsync(int divePhotoId)
    {
        var divePhoto = await divePhotoRepository.GetByIdAsync(divePhotoId);
        return mapper.Map<DivePhotoDto?>(divePhoto);
    }

    public async  Task<IEnumerable<DivePhotoDto>> GetAllDivePhotosAsync()
    {
        var divePhotos = await divePhotoRepository.GetAllAsync();
        return mapper.Map<IEnumerable<DivePhotoDto>>(divePhotos);
    }

    public async Task UpdateDivePhotoAsync(DivePhotoDto divePhotoDto)
    {
        var divePhoto = await divePhotoRepository.GetByIdAsync(divePhotoDto.DivePhotoId);
        if (divePhoto != null)
        {
            await divePhotoRepository.UpdateAsync(mapper.Map<DivePhoto>(divePhotoDto));
        }
    }

    public async Task DeleteDivePhotoAsync(int divePhotoId)
    {
        await divePhotoRepository.DeleteAsync(divePhotoId);
    }
}