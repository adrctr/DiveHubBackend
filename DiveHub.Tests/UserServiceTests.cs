using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using Moq;

namespace DiveHub.Tests;

public class UserServiceTests
{
 private readonly Mock<IStorageService<User>> _mockStorageService;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockStorageService = new Mock<IStorageService<User>>();
        _userService = new UserService(_mockStorageService.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>();
        users.Add(new User() { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" });
        users.Add(new User() { UserId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" });

        _mockStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("John", result[0].FirstName);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnCorrectUser()
    {
        // Arrange
        var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

        _mockStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result?.FirstName);
    }

    [Fact]
    public async Task AddUserAsync_ShouldCallStorageServiceAddAsync()
    {
        // Arrange
        var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

        // Act
        await _userService.AddUserAsync(user);

        // Assert
        _mockStorageService.Verify(s => s.AddAsync(user), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldCallStorageServiceUpdateAsync()
    {
        // Arrange
        var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

        // Act
        await _userService.UpdateUserAsync(user);

        // Assert
        _mockStorageService.Verify(s => s.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldCallStorageServiceDeleteAsync()
    {
        // Arrange
        var userId = 1;

        // Act
        await _userService.DeleteUserAsync(userId);

        // Assert
        _mockStorageService.Verify(s => s.DeleteAsync(userId), Times.Once);
    }
}