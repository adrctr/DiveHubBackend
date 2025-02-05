using DiveHub.Core.Entities;

namespace DiveHub.Application.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(UserDto userDto);
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task UpdateUserAsync(UserDto userDto);
    Task DeleteUserAsync(int userId);
}