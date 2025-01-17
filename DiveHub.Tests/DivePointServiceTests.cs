using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using Moq;

namespace DiveHub.Tests;

public class DivePointServiceTests
{
    private readonly Mock<IStorageService<DivePoint>> _mockDivePointStorageService;
    private readonly DivePointService _divePointService;

    public DivePointServiceTests()
    {
        _mockDivePointStorageService = new Mock<IStorageService<DivePoint>>();
        _divePointService = new DivePointService(_mockDivePointStorageService.Object);
    }

    [Fact]
    public async Task GetAllDivePointsAsync_ShouldReturnAllDivePoints()
    {
        // Arrange
        var divePoints = new List<DivePoint>
        {
            new DivePoint { DivePointId = 1, DiveId = 1, Latitude = 45.0, Longitude = -73.0, Description = "Point 1" },
            new DivePoint { DivePointId = 2, DiveId = 2, Latitude = 46.0, Longitude = -74.0, Description = "Point 2" }
        };
        _mockDivePointStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(divePoints);

        // Act
        var result = await _divePointService.GetAllDivePointsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(45.0, result[0].Latitude);
        Assert.Equal(46.0, result[1].Latitude);
    }

    [Fact]
    public async Task GetDivePointByIdAsync_ShouldReturnDivePoint()
    {
        // Arrange
        var divePoint = new DivePoint { DivePointId = 1, DiveId = 1, Latitude = 45.0, Longitude = -73.0, Description = "Point 1" };
        _mockDivePointStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(divePoint);

        // Act
        var result = await _divePointService.GetDivePointByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.DivePointId);
        Assert.Equal(45.0, result?.Latitude);
    }

    [Fact]
    public async Task GetDivePointsByDiveIdAsync_ShouldReturnDivePointsForDive()
    {
        // Arrange
        var divePoints = new List<DivePoint>
        {
            new DivePoint { DivePointId = 1, DiveId = 1, Latitude = 45.0, Longitude = -73.0, Description = "Point 1" },
            new DivePoint { DivePointId = 2, DiveId = 1, Latitude = 46.0, Longitude = -74.0, Description = "Point 2" },
            new DivePoint { DivePointId = 3, DiveId = 2, Latitude = 47.0, Longitude = -75.0, Description = "Point 3" }
        };
        _mockDivePointStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(divePoints);

        // Act
        var result = await _divePointService.GetDivePointsByDiveIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, dp => Assert.Equal(1, dp.DiveId));
    }

    [Fact]
    public async Task AddDivePointAsync_ShouldCallAddAsync()
    {
        // Arrange
        var divePoint = new DivePoint { DivePointId = 1, DiveId = 1, Latitude = 45.0, Longitude = -73.0, Description = "Point 1" };

        // Act
        await _divePointService.AddDivePointAsync(divePoint);

        // Assert
        _mockDivePointStorageService.Verify(s => s.AddAsync(divePoint), Times.Once);
    }

    [Fact]
    public async Task UpdateDivePointAsync_ShouldCallUpdateAsync()
    {
        // Arrange
        var divePoint = new DivePoint { DivePointId = 1, DiveId = 1, Latitude = 45.0, Longitude = -73.0, Description = "Point 1" };

        // Act
        await _divePointService.UpdateDivePointAsync(divePoint);

        // Assert
        _mockDivePointStorageService.Verify(s => s.UpdateAsync(divePoint), Times.Once);
    }

    [Fact]
    public async Task DeleteDivePointAsync_ShouldCallDeleteAsync()
    {
        // Arrange
        var divePointId = 1;

        // Act
        await _divePointService.DeleteDivePointAsync(divePointId);

        // Assert
        _mockDivePointStorageService.Verify(s => s.DeleteAsync(divePointId), Times.Once);
    }
}