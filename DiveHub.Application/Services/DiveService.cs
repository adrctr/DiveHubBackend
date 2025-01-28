using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Infrastructure.repositories;
namespace DiveHub.Application.Services;

public class DiveService(IDiveRepository diveRepository, IDivePhotoService divePhotoService, IDivePointService divePointService, IMapper mapper) : IDiveService
{
    public async Task CreateDiveAsync(DiveDto diveDto, int userId)
    {
        var dive = mapper.Map<Dive>(diveDto);
        dive.UserId = userId;
        await diveRepository.AddAsync(dive);
    }

    public async Task CreateDiveWithDetailAsync(DiveSaveDto diveSaveDto, int userId)
    {
             // Étape 1 : Sauvegarder la plongée principale
            var dive = new Dive
            {
                UserId = userId,
                DiveName = diveSaveDto.DiveName,
                DiveDate = diveSaveDto.DiveDate,
                Description = diveSaveDto.Description
            };
           await diveRepository.AddAsync(dive);

            // Récupérer l'ID généré
            var diveId = dive.DiveId;

            // Étape 2 : Ajouter les points
            var divePoints = diveSaveDto.Points.Select(p => new DivePointDto()
            {
                DiveId = diveId,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Description = p.Description
            }).ToList();

            await divePointService.AddManyDivePointAsync(divePoints);

            // Étape 3 : Ajouter les photos
            var divePhotos = diveSaveDto.Photos.Select(p => new DivePhotoDto()
            {
                DiveId = diveId,
                FileName = p.FileName,
                Url = p.Url,
                CreatedAt = p.CreatedAt
            }).ToList();

            await divePhotoService.AddManyDivePhotoAsync(divePhotos);
    }

    public async Task<DiveDto?> GetDiveByIdAsync(int diveId)
    {
        var dive = await diveRepository.GetByIdAsync(diveId);
        return mapper.Map<DiveDto?>(dive);
    }

    public async Task<IEnumerable<DiveDetailDto>> GetAllDivesAsync()
    {
        var dives = await diveRepository.GetDivesWihDetails();
        return mapper.Map<List<DiveDetailDto>>(dives);
    }

    public async Task UpdateDiveAsync(DiveDto diveDto)
    {
        var dive = await diveRepository.GetByIdAsync(diveDto.DiveId);
        if (dive != null)
        {
            mapper.Map(diveDto, dive);
            await diveRepository.UpdateAsync(dive);
        }
    }

    public async Task DeleteDiveAsync(int diveId)
    {
        await diveRepository.DeleteAsync(diveId);
    }

    // public async Task<IEnumerable<DiveDto>> GetDivesByUserIdAsync()
    // {
    //     // return await diveRepository.GetDivesWihDetails();
    // }
}