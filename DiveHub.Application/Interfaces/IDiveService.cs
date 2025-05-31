using DiveHub.Application.Dto;

namespace DiveHub.Application.Interfaces;

public interface IDiveService
{
    Task<DiveDto> CreateDiveAsync(DiveSaveDto diveSaveDto, Guid userId);
    Task<DiveDto?> GetDiveByIdAsync(Guid diveId);
    Task<IEnumerable<DiveDto>> GetAllDivesAsync();
    Task UpdateDiveAsync(DiveDto diveDto);
    Task DeleteDiveAsync(Guid diveId);
}