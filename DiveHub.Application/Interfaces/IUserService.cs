using DiveHub.Core.Entities;

namespace DiveHub.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> SyncUserFromProviderAsync(string userId, string? email);

}