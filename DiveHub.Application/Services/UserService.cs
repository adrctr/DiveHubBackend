using AutoMapper;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class UserService(IRepository<User> userRepository, IMapper mapper) : IUserService
{
    public async Task<UserDto> SyncUserFromProviderAsync(string userId, string? email)
    {
        Guid guid = Guid.Parse(userId);
        var user = await userRepository.GetByIdAsync(guid);
        if (user == null)
        {
            user = new User
            {
                UserId = guid,
                Email = email,
                CreatedAt = DateTime.UtcNow,
            };
            await userRepository.AddAsync(user);
        }

        return mapper.Map<UserDto>(user);
    }
}