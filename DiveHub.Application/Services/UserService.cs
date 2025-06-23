using AutoMapper;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class UserService(IRepository<User> userRepository, IMapper mapper) : IUserService
{
    public async Task<UserDto> SyncUserFromProviderAsync(int userId, string? email)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            user = new User
            {
                UserId = userId,
                Email = email,
                CreatedAt = DateTime.UtcNow,
            };
            await userRepository.AddAsync(user);
        }

        return mapper.Map<UserDto>(user);
    }
}