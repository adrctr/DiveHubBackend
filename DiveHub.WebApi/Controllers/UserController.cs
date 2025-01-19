using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DiveHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
    {
        await userService.CreateUserAsync(userDto);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return user != null ? Ok(user) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
    {
        await userService.UpdateUserAsync(userDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await userService.DeleteUserAsync(id);
        return NoContent();
    }
}