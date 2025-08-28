using DiveHub.Application.Dto;
using DiveHub.Core.Entities;

namespace DiveHub.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto);
    Task<UserDto?> GetUserByAuth0IdAsync(string auth0UserId);
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task<UserDto> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto);
    Task<UserDto> SyncUserFromProviderAsync(int userId, string? email);

    Task DeleteUserAsync(int userId);
}