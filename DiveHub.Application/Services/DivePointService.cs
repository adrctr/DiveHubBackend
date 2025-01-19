using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class DivePointService(IRepository<DivePoint> divePointRepository, IMapper mapper) : IDivePointService
{
    public async Task AddDivePointAsync(DivePointDto divePointDto, int diveId)
    {
        var divePoint = mapper.Map<DivePoint>(divePointDto);
        divePoint.DiveId = diveId;
        await divePointRepository.AddAsync(divePoint);
    }
    
    public async Task AddManyDivePointAsync(IEnumerable<DivePointDto> divePointsDtos)
    {
        var divePoints = mapper.Map<IEnumerable<DivePoint>>(divePointsDtos);
        // Ajouter tous les DivePoint en une seule opération
        await divePointRepository.AddRangeAsync(divePoints);
    }

    public async Task<DivePointDto?> GetDivePointByIdAsync(int divePointId)
    {
        var divePoint = await divePointRepository.GetByIdAsync(divePointId);
        return mapper.Map<DivePointDto?>(divePoint);
    }

    public async  Task<IEnumerable<DivePointDto>> GetAllDivePointsAsync()
    {
        var divePoints = await divePointRepository.GetAllAsync();
        return mapper.Map<IEnumerable<DivePointDto>>(divePoints);
    }

    public async Task UpdateDivePointAsync(DivePointDto divePointDto)
    {
        var divePoint = await divePointRepository.GetByIdAsync(divePointDto.DivePointId);
        if (divePoint != null)
        {
            await divePointRepository.UpdateAsync(mapper.Map<DivePoint>(divePointDto));
        }
    }

    public async Task DeleteDivePointAsync(int divePointId)
    {
        await divePointRepository.DeleteAsync(divePointId);
    }
}