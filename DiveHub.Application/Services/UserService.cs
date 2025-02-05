using AutoMapper;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;

namespace DiveHub.Application.Services;

public class UserService(IRepository<User> userRepository, IMapper mapper) : IUserService
{
    public async Task CreateUserAsync(UserDto userDto)
    {
        var user = mapper.Map<User>(userDto);
        await userRepository.AddAsync(user);
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        return mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllAsync();
        return mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task UpdateUserAsync(UserDto userDto)
    {
        var user = await userRepository.GetByIdAsync(userDto.UserId);
        if (user != null)
        {
            mapper.Map(userDto, user);
            await userRepository.UpdateAsync(user);
        }
    }

    public async Task DeleteUserAsync(int userId)
    {
        await userRepository.DeleteAsync(userId);
    }
}