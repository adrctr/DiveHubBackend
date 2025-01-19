using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class DiveService(IRepository<Dive> diveRepository, IMapper mapper) : IDiveService
{
    public async Task CreateDiveAsync(DiveDto diveDto, int userId)
    {
        var dive = mapper.Map<Dive>(diveDto);
        dive.UserId = userId;
        await diveRepository.AddAsync(dive);
    }

    public async Task<DiveDto?> GetDiveByIdAsync(int diveId)
    {
        var dive = await diveRepository.GetByIdAsync(diveId);
        return mapper.Map<DiveDto?>(dive);
    }

    public async Task<IEnumerable<DiveDto>> GetAllDivesAsync()
    {
        var dives = await diveRepository.GetAllAsync();
        return mapper.Map<IEnumerable<DiveDto>>(dives);
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
}