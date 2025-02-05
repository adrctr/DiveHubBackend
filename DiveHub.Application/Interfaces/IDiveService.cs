using DiveHub.Application.Dto;

namespace DiveHub.Application.Interfaces;

public interface IDiveService
{
    Task CreateDiveAsync(DiveSaveDto diveSaveDto, int userId);
    Task CreateDiveWithDetailAsync(DiveSaveDto diveSaveDto, int userId);
    Task<DiveDto?> GetDiveByIdAsync(int diveId);
    Task<IEnumerable<DiveDetailDto>> GetAllDivesAsync();
    Task UpdateDiveAsync(DiveDto diveDto);
    Task DeleteDiveAsync(int diveId);
}