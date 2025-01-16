using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using Moq;

namespace DiveHub.Tests;

public class DiveServiceTests
{
 private readonly Mock<IStorageService<Dive>> _mockStorageService;
    private readonly DiveService _diveService;

    public DiveServiceTests()
    {
        _mockStorageService = new Mock<IStorageService<Dive>>();
        _diveService = new DiveService(_mockStorageService.Object);
    }

    [Fact]
    public async Task GetAllDivesAsync_ShouldReturnAllDives()
    {
        // Arrange
        var dives = new List<Dive>
        {
            new Dive
            {
                DiveId = 1,
                DiveName = "Dive 1",
                UserId = 1,
                DiveDate = DateTime.Now,
                DivePoints = new List<DivePoint>
                {
                    new DivePoint { DivePointId = 1, Latitude = 34.05, Longitude = -118.25 },
                    new DivePoint { DivePointId = 2, Latitude = 36.16, Longitude = -115.15 }
                },
                DivePhotos = new List<DivePhoto>
                {
                    new DivePhoto { DivePhotoId = 1, Url = "http://example.com/photo1.jpg" },
                    new DivePhoto { DivePhotoId = 2, Url = "http://example.com/photo2.jpg" }
                }
            },
            new Dive
            {
                DiveId = 2,
                DiveName = "Dive 2",
                UserId = 1,
                DiveDate = DateTime.Now,
                DivePoints = new List<DivePoint>
                {
                    new DivePoint { DivePointId = 3, Latitude = 40.71, Longitude = -74.01 }
                },
                DivePhotos = new List<DivePhoto>
                {
                    new DivePhoto { DivePhotoId = 3, Url = "http://example.com/photo3.jpg" }
                }
            }
        };

        _mockStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(dives);

        // Act
        var result = await _diveService.GetAllDivesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Dive 1", result[0].DiveName);
        Assert.Equal(2, result[0].DivePoints.Count); // Vérifie le nombre de points
        Assert.Equal(2, result[0].DivePhotos.Count); // Vérifie le nombre de photos

        Assert.Equal("Dive 2", result[1].DiveName);
        Assert.Equal(1, result[1].DivePoints.Count); // Vérifie le nombre de points
        Assert.Equal(1, result[1].DivePhotos.Count); // Vérifie le nombre de photos
    }

    [Fact]
    public async Task GetDiveByIdAsync_ShouldReturnDiveWithPointsAndPhotos()
    {
        // Arrange
        var dive = new Dive
        {
            DiveId = 1,
            DiveName = "Dive 1",
            UserId = 1,
            DiveDate = DateTime.Now,
            DivePoints = new List<DivePoint>
            {
                new DivePoint { DivePointId = 1, Latitude = 34.05, Longitude = -118.25 }
            },
            DivePhotos = new List<DivePhoto>
            {
                new DivePhoto { DivePhotoId = 1, Url = "http://example.com/photo1.jpg" }
            }
        };

        // Configurer le mock pour retourner une plongée spécifique
        _mockStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dive);

        // Act
        var result = await _diveService.GetDiveByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Dive 1", result?.DiveName);
        Assert.Equal(1, result?.DivePoints.Count); // Vérifie le nombre de points
        Assert.Equal(1, result?.DivePhotos.Count); // Vérifie le nombre de photos
    }
    [Fact]
    public async Task AddDiveAsync_ShouldCallAddAsyncWithPointsAndPhotos()
    {
        // Arrange
        var dive = new Dive
        {
            DiveId = 1,
            DiveName = "Dive 1",
            UserId = 1,
            DiveDate = DateTime.Now,
            DivePoints = new List<DivePoint>
            {
                new DivePoint { DivePointId = 1, Latitude = 34.05, Longitude = -118.25 }
            },
            DivePhotos = new List<DivePhoto>
            {
                new DivePhoto { DivePhotoId = 1, Url = "http://example.com/photo1.jpg" }
            }
        };

        // Act
        await _diveService.AddDiveAsync(dive);

        // Assert
        _mockStorageService.Verify(s => s.AddAsync(dive), Times.Once);
    }
}