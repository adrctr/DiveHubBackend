using DiveHub.Core.Entities;

namespace DiveHub.Application.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);

    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    
    Task AddDiveAsync(int userid, Dive dive);
    Task<List<Dive>> GetDiveAsync(int userid);
}