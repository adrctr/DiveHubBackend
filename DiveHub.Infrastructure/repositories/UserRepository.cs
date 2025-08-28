using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace DiveHub.Infrastructure.repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByAuth0UserIdAsync(string auth0UserId);
    Task<User?> GetUserWithDivesAsync(int userId);
}

public class UserRepository(DiveHubDbContext context) : GenericRepository<User>(context), IUserRepository
{
    private readonly DiveHubDbContext _dbContext = context;

    /// <summary>
    /// Récupère un utilisateur par son Auth0UserId
    /// </summary>
    /// <param name="auth0UserId">L'identifiant Auth0 de l'utilisateur</param>
    /// <returns>L'utilisateur correspondant ou null</returns>
    public async Task<User?> GetByAuth0UserIdAsync(string auth0UserId)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Auth0UserId == auth0UserId);
    }

    /// <summary>
    /// Récupère un utilisateur avec ses plongées
    /// </summary>
    /// <param name="userId">L'identifiant de l'utilisateur</param>
    /// <returns>L'utilisateur avec ses plongées ou null</returns>
    public async Task<User?> GetUserWithDivesAsync(int userId)
    {
        return await _dbContext.Users
            .Include(u => u.Dives)
                .ThenInclude(d => d.Equipments)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }
}