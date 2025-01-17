using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class UserService(IStorageService<User> storageService, IDiveService diveService) :IUserService
{
    public async Task<List<User>> GetAllUsersAsync() => await storageService.GetAllAsync();

    public async Task<User?> GetUserByIdAsync(int id) => await storageService.GetByIdAsync(id);

    public async Task AddUserAsync(User user) => await storageService.AddAsync(user);

    public async Task UpdateUserAsync(User user) => await storageService.UpdateAsync(user);

    public async Task DeleteUserAsync(int id) => await storageService.DeleteAsync(id);
    
    /// <summary>
    /// Ajoute une plongée a l'utilisateur
    /// </summary>
    /// <param name="userid"></param>
    /// <param name="dive"></param>
    public async Task AddDiveAsync(int userid, Dive dive)
    {
        var user = await storageService.GetByIdAsync(userid);
        dive.UserId = userid;
        // Mise à jour de l'utilisateur avec la nouvelle plongée
        if (user != null) await diveService.AddDiveAsync(dive);
    }

    /// <summary>
    /// Récupérer les plongées d'un utilisateur 
    /// </summary>
    /// <param name="diveId"></param>
    /// <param name="userid"></param>
    /// <returns></returns>
    public async Task<List<Dive>> GetDiveAsync(int userid)
    {
        return await diveService.GetDiveByUserIdAsync(userid);
    }
}
