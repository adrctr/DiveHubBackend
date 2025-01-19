using DiveHub.Application.Dto;
using DiveHub.Core.Entities;

namespace DiveHub.Application.Interfaces;

public interface IDiveService
{
    Task CreateDiveAsync(DiveDto diveDto, int userId);
    Task CreateDiveWithDetailAsync(DiveSaveDto diveSaveDto, int userId);
    Task<DiveDto?> GetDiveByIdAsync(int diveId);
    Task<IEnumerable<DiveDto>> GetAllDivesAsync();
    Task UpdateDiveAsync(DiveDto diveDto);
    Task DeleteDiveAsync(int diveId);
}