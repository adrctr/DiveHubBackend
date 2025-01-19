using DiveHub.Application.Dto;

namespace DiveHub.Application.Interfaces;

public interface IDiveService
{
    Task CreateDiveAsync(DiveDto diveDto, int userId);
    Task<DiveDto?> GetDiveByIdAsync(int diveId);
    Task<IEnumerable<DiveDto>> GetAllDivesAsync();
    Task UpdateDiveAsync(DiveDto diveDto);
    Task DeleteDiveAsync(int diveId);
}