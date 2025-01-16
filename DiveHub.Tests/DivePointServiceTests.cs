using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using Moq;

namespace DiveHub.Tests;

public class DivePointServiceTests
{
    private readonly Mock<IStorageService<DivePoint>> _mockStorageService;
    private readonly DivePointService _diveService;

    public DivePointServiceTests()
    {
        _mockStorageService = new Mock<IStorageService<DivePoint>>();
        _diveService = new DivePointService(_mockStorageService.Object);
    }

    [Fact]
    public async Task GetAllDivesPointAsync_ShouldReturnAllDivesPoint()
    {
        // Arrange
        var divesPoints = new List<DivePoint>
        {
            new DivePoint()
            {
                DivePointId = 1,
                DiveId = 1,
                Longitude = 17.52,
                Latitude = 45.05,
                Description = "shipwreck site, surrounded by deep water and strong currents. Visibility is low."
            },
            new DivePoint()
            {
                DivePointId = 2,
                DiveId = 1,
                Longitude = 18.52,
                Latitude = 44.05,
                Description = "The wreck's stern, with coral growths and marine life flourishing around the remains."
            }
        };
        
        _mockStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(divesPoints);
        
        // Act
        var result = await _diveService.GetAllDivesPointAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(17.52, result[0].Longitude);
        Assert.Equal(45.05, result[0].Latitude);
    }
    
    [Fact]
    public async Task GetDivePointByIdAsync_ShouldReturnCorrectDivePoint()
    {
        // Arrange
        var divePoint = new DivePoint()
        {
            DivePointId = 1,
            DiveId = 1,
            Longitude = 17.52,
            Latitude = 45.05,
            Description = "shipwreck site, surrounded by deep water and strong currents. Visibility is low."
        };

        _mockStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(divePoint);

        // Act
        var result = await _diveService.GetDivePointByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.DivePointId);
        Assert.Equal(1, result?.DiveId);
    }

    [Fact]
    public async Task AddUserAsync_ShouldCallStorageServiceAddAsync()
    {
        // Arrange
        var divePoint = new DivePoint()
        {
            DivePointId = 1,
            DiveId = 1,
            Longitude = 17.52,
            Latitude = 45.05,
            Description = "shipwreck site, surrounded by deep water and strong currents. Visibility is low."
        };

        // Act
        await _diveService.AddDivePointAsync(divePoint);

        // Assert
        _mockStorageService.Verify(s => s.AddAsync(divePoint), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldCallStorageServiceUpdateAsync()
    {
        // Arrange
        var divePoint = new DivePoint()
        {
            DivePointId = 1,
            DiveId = 1,
            Longitude = 17.52,
            Latitude = 45.05,
            Description = "shipwreck site, surrounded by deep water and strong currents. Visibility is low."
        };

        // Act
        await _diveService.UpdateDivePointAsync(divePoint);

        // Assert
        _mockStorageService.Verify(s => s.UpdateAsync(divePoint), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldCallStorageServiceDeleteAsync()
    {
        // Arrange
        const int divePointId = 1;

        // Act
        await _diveService.DeleteDivePointAsync(divePointId);

        // Assert
        _mockStorageService.Verify(s => s.DeleteAsync(divePointId), Times.Once);
    }
}