using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Infrastructure.repositories;
namespace DiveHub.Application.Services;

public class DiveService(
    IDiveRepository diveRepository,
    IMapper mapper) : IDiveService
{
    public async Task<DiveDto> CreateDiveAsync(DiveSaveDto diveSaveDto, Guid userId)
    {
        Dive dive = mapper.Map<Dive>(diveSaveDto);
        dive.UserId = userId;
        await diveRepository.AddAsync(dive);
        return mapper.Map<DiveDto>(dive);
    }
    
    public async Task<DiveDto?> GetDiveByIdAsync(Guid diveId)
    {
        var dive = await diveRepository.GetByIdAsync(diveId);
        return mapper.Map<DiveDto?>(dive);
    }

    public async Task<IEnumerable<DiveDto>> GetAllDivesAsync()
    {
        var dives = await diveRepository.GetDivesWihDetails();
        return mapper.Map<List<DiveDto>>(dives);
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

    public async Task DeleteDiveAsync(Guid diveId)
    {
        await diveRepository.DeleteAsync(diveId);
    }
}