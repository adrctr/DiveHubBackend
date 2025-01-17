using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class DiveService(
    IStorageService<Dive> diveStorageService,
    IDivePointService divePointService,
    IDivePhotoService divePhotoService)
    : IDiveService
{
    public async Task<List<Dive>> GetAllDivesAsync() => await diveStorageService.GetAllAsync();

    public async Task<Dive?> GetDiveByIdAsync(int id) => await diveStorageService.GetByIdAsync(id);

    /// <summary>
    /// Récupérer les plongées par l'id Utilisateur
    /// </summary>
    /// <param name="userid"></param>
    /// <returns></returns>
    public async Task<List<Dive>> GetDiveByUserIdAsync(int userid)
    {
        var dive = await GetAllDivesAsync();
        return dive.Where(dp => dp.UserId == userid).ToList();
    }

    public async Task AddDiveAsync(Dive dive) => await diveStorageService.AddAsync(dive);

    public async Task UpdateDiveAsync(Dive dive) => await diveStorageService.UpdateAsync(dive);

    public async Task DeleteDiveAsync(int id) => await diveStorageService.DeleteAsync(id);

    /// <summary>
    /// Ajouter un point GPS à une plongée
    /// </summary>
    /// <param name="diveId"></param>
    /// <param name="divePoint"></param>
    public async Task AddDivePointAsync(int diveId, DivePoint divePoint)
    {
        var dive = await diveStorageService.GetByIdAsync(diveId);
        divePoint.DiveId = diveId;
        // Mise à jour de la plongée avec les nouveaux points GPS
        if (dive != null) await divePointService.AddDivePointAsync(divePoint);
    }

    /// <summary>
    /// Ajouter une photo à une plongée
    /// </summary>
    public async Task AddDivePhotoAsync(int diveId, DivePhoto divePhoto)
    {
        var dive = await diveStorageService.GetByIdAsync(diveId);
        divePhoto.DiveId = diveId;
        // Mise à jour de la plongée avec les nouveaux points GPS
        if (dive != null) await divePhotoService.AddDivePhotoAsync(divePhoto);
    }

    /// <summary>
    /// Récupérer les points GPS d'une plongée
    /// </summary>
    /// <param name="diveId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<DivePoint?> GetDivePointsAsync(int diveId)
    {
        return await divePointService.GetDivePointByIdAsync(diveId);
    }
}