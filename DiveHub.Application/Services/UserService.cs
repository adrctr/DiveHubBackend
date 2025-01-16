using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class UserService(IStorageService<User> storageService)
{
    public Task<List<User>> GetAllUsersAsync() => storageService.GetAllAsync();

    public Task<User?> GetUserByIdAsync(int id) => storageService.GetByIdAsync(id);

    public Task AddUserAsync(User user) => storageService.AddAsync(user);

    public Task UpdateUserAsync(User user) => storageService.UpdateAsync(user);

    public Task DeleteUserAsync(int id) => storageService.DeleteAsync(id);
}
