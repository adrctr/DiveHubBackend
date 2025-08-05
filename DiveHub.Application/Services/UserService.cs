using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Infrastructure.repositories;

namespace DiveHub.Application.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto)
    {
        // Vérifier si l'utilisateur existe déjà
        var existingUser = await userRepository.GetByAuth0UserIdAsync(userCreateDto.Auth0UserId);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"Un utilisateur avec l'Auth0UserId '{userCreateDto.Auth0UserId}' existe déjà.");
        }

        var user = mapper.Map<User>(userCreateDto);
        user.CreatedAt = DateTime.UtcNow;
        
        await userRepository.AddAsync(user);
        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByAuth0IdAsync(string auth0UserId)
    {
        var user = await userRepository.GetByAuth0UserIdAsync(auth0UserId);
        return user != null ? mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        return user != null ? mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
    {
        var existingUser = await userRepository.GetByIdAsync(userId);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"Utilisateur avec l'ID {userId} non trouvé.");
        }

        // Mettre à jour les propriétés
        existingUser.FirstName = userUpdateDto.FirstName;
        existingUser.LastName = userUpdateDto.LastName;
        existingUser.Email = userUpdateDto.Email;
        existingUser.Picture = userUpdateDto.Picture;

        await userRepository.UpdateAsync(existingUser);
        return mapper.Map<UserDto>(existingUser);
    }

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

    public async Task DeleteUserAsync(int userId)
    {
        await userRepository.DeleteAsync(userId);
    }
}