using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiveHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Cr�e un nouvel utilisateur
    /// </summary>
    /// <param name="userCreateDto">Les donn�es de l'utilisateur � cr�er</param>
    /// <returns>L'utilisateur cr��</returns>
    [HttpPost]
    [ProducesResponseType<UserDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
    {
        try
        {
            var userDto = await userService.CreateUserAsync(userCreateDto);
            return CreatedAtAction(nameof(GetUserById), new { userId = userDto.UserId }, userDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return BadRequest("Erreur lors de la cr�ation de l'utilisateur");
        }
    }

    /// <summary>
    /// R�cup�re un utilisateur par son Auth0 ID
    /// </summary>
    /// <param name="auth0UserId">L'identifiant Auth0 de l'utilisateur</param>
    /// <returns>L'utilisateur correspondant</returns>
    [HttpGet("auth0/{auth0UserId}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByAuth0Id(string auth0UserId)
    {
        var user = await userService.GetUserByAuth0IdAsync(auth0UserId);
        return user != null ? Ok(user) : NotFound($"Utilisateur avec Auth0UserId '{auth0UserId}' non trouv�");
    }

    /// <summary>
    /// R�cup�re un utilisateur par son ID
    /// </summary>
    /// <param name="userId">L'identifiant de l'utilisateur</param>
    /// <returns>L'utilisateur correspondant</returns>
    [HttpGet("{userId}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUserById(int userId)
    {
        // V�rifier que l'utilisateur connect� peut acc�der � ces donn�es
        var currentUserId = GetCurrentUserId();
        if (currentUserId != userId)
        {
            return Forbid("Vous ne pouvez acc�der qu'� vos propres donn�es");
        }

        var user = await userService.GetUserByIdAsync(userId);
        return user != null ? Ok(user) : NotFound($"Utilisateur avec l'ID {userId} non trouv�");
    }

    /// <summary>
    /// Met � jour un utilisateur
    /// </summary>
    /// <param name="userId">L'identifiant de l'utilisateur</param>
    /// <param name="userUpdateDto">Les donn�es � mettre � jour</param>
    /// <returns>L'utilisateur mis � jour</returns>
    [HttpPut("{userId}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDto userUpdateDto)
    {
        // V�rifier que l'utilisateur connect� peut modifier ces donn�es
        var currentUserId = GetCurrentUserId();
        if (currentUserId != userId)
        {
            return Forbid("Vous ne pouvez modifier que vos propres donn�es");
        }

        try
        {
            var updatedUser = await userService.UpdateUserAsync(userId, userUpdateDto);
            return Ok(updatedUser);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return BadRequest("Erreur lors de la mise � jour de l'utilisateur");
        }
    }

    /// <summary>
    /// R�cup�re l'ID de l'utilisateur actuel depuis le token JWT
    /// </summary>
    /// <returns>L'ID de l'utilisateur connect�</returns>
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("Impossible de r�cup�rer l'ID de l'utilisateur depuis le token");
        }
        return userId;
    }

    /// <summary>
    /// R�cup�re l'Auth0UserId de l'utilisateur actuel depuis le token JWT
    /// </summary>
    /// <returns>L'Auth0UserId de l'utilisateur connect�</returns>
    private string GetCurrentAuth0UserId()
    {
        var auth0UserId = User.FindFirst("sub")?.Value; // "sub" est le claim standard pour l'ID utilisateur Auth0
        if (string.IsNullOrEmpty(auth0UserId))
        {
            throw new UnauthorizedAccessException("Impossible de r�cup�rer l'Auth0UserId depuis le token");
        }
        return auth0UserId;
    }
}