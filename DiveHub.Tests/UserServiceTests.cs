using DiveHub.Application.Interfaces;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using Moq;

namespace DiveHub.Tests;

public class UserServiceTests
{
    private readonly Mock<IStorageService<User>> _mockUserStorageService;
    private readonly Mock<IDiveService> _mockDiveService;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserStorageService = new Mock<IStorageService<User>>();
        _mockDiveService = new Mock<IDiveService>();
        _userService = new UserService(_mockUserStorageService.Object, _mockDiveService.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new User { UserId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
        };
        _mockUserStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("John", result[0].FirstName);
        Assert.Equal("Jane", result[1].FirstName);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser()
    {
        // Arrange
        var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        _mockUserStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.UserId);
        Assert.Equal("John", result?.FirstName);
    }

    [Fact]
    public async Task AddUserAsync_ShouldCallAddAsync()
    {
        // Arrange
        var user = new User { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

        // Act
        await _userService.AddUserAsync(user);

        // Assert
        _mockUserStorageService.Verify(s => s.AddAsync(user), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldCallUpdateAsync()
    {
        // Arrange
        var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

        // Act
        await _userService.UpdateUserAsync(user);

        // Assert
        _mockUserStorageService.Verify(s => s.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldCallDeleteAsync()
    {
        // Arrange
        var userId = 1;

        // Act
        await _userService.DeleteUserAsync(userId);

        // Assert
        _mockUserStorageService.Verify(s => s.DeleteAsync(userId), Times.Once);
    }

    [Fact]
    public async Task AddDiveAsync_ShouldAddDiveToUser()
    {
        // Arrange
        var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        var dive = new Dive
            { DiveName = "Coral Reef Exploration", DiveDate = DateTime.UtcNow, Description = "Reef diving" };

        _mockUserStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        await _userService.AddDiveAsync(1, dive);

        // Assert
        _mockUserStorageService.Verify(s => s.GetByIdAsync(1), Times.Once);
        _mockDiveService.Verify(
            s => s.AddDiveAsync(It.Is<Dive>(d => d.UserId == 1 && d.DiveName == "Coral Reef Exploration")), Times.Once);
    }

    [Fact]
    public async Task GetDiveAsync_ShouldReturnDivesForUser()
    {
        // Arrange
        var dives = new List<Dive>
        {
            new Dive { DiveId = 1, DiveName = "Coral Reef Exploration", DiveDate = DateTime.UtcNow },
            new Dive { DiveId = 2, DiveName = "Wreck Dive", DiveDate = DateTime.UtcNow }
        };

        _mockDiveService.Setup(s => s.GetDiveByUserIdAsync(1)).ReturnsAsync(dives);

        // Act
        var result = await _userService.GetDiveAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Coral Reef Exploration", result[0].DiveName);
        Assert.Equal("Wreck Dive", result[1].DiveName);
    }
}