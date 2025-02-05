using DiveHub.Application.Interfaces;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using Moq;

namespace DiveHub.Tests;

public class DiveServiceTests
{
    private readonly Mock<IStorageService<Dive>> _mockDiveStorageService;
    private readonly Mock<IDivePointService> _mockDivePointService;
    private readonly Mock<IDivePhotoService> _mockDivePhotoService;
    private readonly DiveService _diveService;

    public DiveServiceTests()
    {
        _mockDiveStorageService = new Mock<IStorageService<Dive>>();
        _mockDivePointService = new Mock<IDivePointService>();
        _mockDivePhotoService = new Mock<IDivePhotoService>();

        _diveService = new DiveService(
            _mockDiveStorageService.Object,
            _mockDivePointService.Object,
            _mockDivePhotoService.Object
        );
    }

    [Fact]
    public async Task GetAllDivesAsync_ShouldReturnAllDives()
    {
        // Arrange
        var dives = new List<Dive>
        {
            new Dive { DiveId = 1, UserId = 1, DiveName = "Dive 1", DiveDate = DateTime.UtcNow },
            new Dive { DiveId = 2, UserId = 2, DiveName = "Dive 2", DiveDate = DateTime.UtcNow }
        };
        _mockDiveStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(dives);

        // Act
        var result = await _diveService.GetAllDivesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Dive 1", result[0].DiveName);
        Assert.Equal("Dive 2", result[1].DiveName);
    }

    [Fact]
    public async Task GetDiveByIdAsync_ShouldReturnDive()
    {
        // Arrange
        var dive = new Dive { DiveId = 1, UserId = 1, DiveName = "Dive 1", DiveDate = DateTime.UtcNow };
        _mockDiveStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dive);

        // Act
        var result = await _diveService.GetDiveByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.DiveId);
        Assert.Equal("Dive 1", result?.DiveName);
    }

    [Fact]
    public async Task AddDiveAsync_ShouldCallAddAsync()
    {
        // Arrange
        var dive = new Dive { DiveId = 1, UserId = 1, DiveName = "Dive 1", DiveDate = DateTime.UtcNow };

        // Act
        await _diveService.AddDiveAsync(dive);

        // Assert
        _mockDiveStorageService.Verify(s => s.AddAsync(dive), Times.Once);
    }

    [Fact]
    public async Task UpdateDiveAsync_ShouldCallUpdateAsync()
    {
        // Arrange
        var dive = new Dive { DiveId = 1, UserId = 1, DiveName = "Dive 1", DiveDate = DateTime.UtcNow };

        // Act
        await _diveService.UpdateDiveAsync(dive);

        // Assert
        _mockDiveStorageService.Verify(s => s.UpdateAsync(dive), Times.Once);
    }

    [Fact]
    public async Task DeleteDiveAsync_ShouldCallDeleteAsync()
    {
        // Arrange
        var diveId = 1;

        // Act
        await _diveService.DeleteDiveAsync(diveId);

        // Assert
        _mockDiveStorageService.Verify(s => s.DeleteAsync(diveId), Times.Once);
    }

    [Fact]
    public async Task GetDiveByUserIdAsync_ShouldReturnDivesForUser()
    {
        // Arrange
        var dives = new List<Dive>
        {
            new Dive { DiveId = 1, UserId = 1, DiveName = "Dive 1", DiveDate = DateTime.UtcNow },
            new Dive { DiveId = 2, UserId = 1, DiveName = "Dive 2", DiveDate = DateTime.UtcNow }
        };
        _mockDiveStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(dives);

        // Act
        var result = await _diveService.GetDiveByUserIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, d => Assert.Equal(1, d.UserId));
    }

    [Fact]
    public async Task AddDivePointAsync_ShouldAddDivePointToDive()
    {
        // Arrange
        var dive = new Dive { DiveId = 1, UserId = 1, DiveName = "Dive 1", DiveDate = DateTime.UtcNow };
        var divePoint = new DivePoint
            { DivePointId = 1, Latitude = 45.0, Longitude = -73.0, Description = "GPS Point" };

        _mockDiveStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dive);

        // Act
        await _diveService.AddDivePointAsync(1, divePoint);

        // Assert
        _mockDiveStorageService.Verify(s => s.GetByIdAsync(1), Times.Once);
        _mockDivePointService.Verify(s => s.AddDivePointAsync(divePoint), Times.Once);
    }

    [Fact]
    public async Task AddDivePhotoAsync_ShouldAddPhotoToDive()
    {
        // Arrange
        var dive = new Dive { DiveId = 1, UserId = 1, DiveName = "Dive 1", DiveDate = DateTime.UtcNow };
        var divePhoto = new DivePhoto
            { DivePhotoId = 1, FileName = "photo.jpg", Url = "http://example.com/photo.jpg" };

        _mockDiveStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dive);

        // Act
        await _diveService.AddDivePhotoAsync(1, divePhoto);

        // Assert
        _mockDiveStorageService.Verify(s => s.GetByIdAsync(1), Times.Once);
        _mockDivePhotoService.Verify(s => s.AddDivePhotoAsync(divePhoto), Times.Once);
    }
}