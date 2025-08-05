using DiveHub.Application.Dto;

namespace DiveHub.Application.Interfaces;

public interface IDiveService
{
    Task<DiveDto> CreateDiveAsync(DiveSaveDto diveSaveDto, string useridauth0);
    Task<DiveDto?> GetDiveByIdAsync(int diveId);
    Task<IEnumerable<DiveDto>> GetAllDivesAsync(string auth0UserId);
    Task UpdateDiveAsync(DiveDto diveDto);
    Task DeleteDiveAsync(int diveId);
}