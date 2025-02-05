using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using Moq;

namespace DiveHub.Tests;

public class DivePhotoServiceTests
{
    private readonly Mock<IStorageService<DivePhoto>> _mockDivePhotoStorageService;
    private readonly DivePhotoService _divePhotoService;

    public DivePhotoServiceTests()
    {
        _mockDivePhotoStorageService = new Mock<IStorageService<DivePhoto>>();
        _divePhotoService = new DivePhotoService(_mockDivePhotoStorageService.Object);
    }

    [Fact]
    public async Task GetAllDivePhotosAsync_ShouldReturnAllDivePhotos()
    {
        // Arrange
        var divePhotos = new List<DivePhoto>
        {
            new DivePhoto { DivePhotoId = 1, DiveId = 1, FileName = "photo1.jpg", Url = "http://example.com/photo1.jpg" },
            new DivePhoto { DivePhotoId = 2, DiveId = 2, FileName = "photo2.jpg", Url = "http://example.com/photo2.jpg" }
        };
        _mockDivePhotoStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(divePhotos);

        // Act
        var result = await _divePhotoService.GetAllDivePhotosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("photo1.jpg", result[0].FileName);
        Assert.Equal("photo2.jpg", result[1].FileName);
    }

    [Fact]
    public async Task GetDivePhotoByIdAsync_ShouldReturnDivePhoto()
    {
        // Arrange
        var divePhoto = new DivePhoto { DivePhotoId = 1, DiveId = 1, FileName = "photo1.jpg", Url = "http://example.com/photo1.jpg" };
        _mockDivePhotoStorageService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(divePhoto);

        // Act
        var result = await _divePhotoService.GetDivePhotoByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.DivePhotoId);
        Assert.Equal("photo1.jpg", result?.FileName);
    }

    [Fact]
    public async Task GetDivePhotoByDiveIdAsync_ShouldReturnDivePhotosForDive()
    {
        // Arrange
        var divePhotos = new List<DivePhoto>
        {
            new DivePhoto { DivePhotoId = 1, DiveId = 1, FileName = "photo1.jpg", Url = "http://example.com/photo1.jpg" },
            new DivePhoto { DivePhotoId = 2, DiveId = 1, FileName = "photo2.jpg", Url = "http://example.com/photo2.jpg" },
            new DivePhoto { DivePhotoId = 3, DiveId = 2, FileName = "photo3.jpg", Url = "http://example.com/photo3.jpg" }
        };
        _mockDivePhotoStorageService.Setup(s => s.GetAllAsync()).ReturnsAsync(divePhotos);

        // Act
        var result = await _divePhotoService.GetDivePhotoByDiveIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, dp => Assert.Equal(1, dp.DiveId));
    }

    [Fact]
    public async Task AddDivePhotoAsync_ShouldCallAddAsync()
    {
        // Arrange
        var divePhoto = new DivePhoto { DivePhotoId = 1, DiveId = 1, FileName = "photo1.jpg", Url = "http://example.com/photo1.jpg" };

        // Act
        await _divePhotoService.AddDivePhotoAsync(divePhoto);

        // Assert
        _mockDivePhotoStorageService.Verify(s => s.AddAsync(divePhoto), Times.Once);
    }

    [Fact]
    public async Task UpdateDivePhotoAsync_ShouldCallUpdateAsync()
    {
        // Arrange
        var divePhoto = new DivePhoto { DivePhotoId = 1, DiveId = 1, FileName = "photo1.jpg", Url = "http://example.com/photo1.jpg" };

        // Act
        await _divePhotoService.UpdateDivePhotoAsync(divePhoto);

        // Assert
        _mockDivePhotoStorageService.Verify(s => s.UpdateAsync(divePhoto), Times.Once);
    }

    [Fact]
    public async Task DeleteDivePhotoAsync_ShouldCallDeleteAsync()
    {
        // Arrange
        var divePhotoId = 1;

        // Act
        await _divePhotoService.DeleteDivePhotoAsync(divePhotoId);

        // Assert
        _mockDivePhotoStorageService.Verify(s => s.DeleteAsync(divePhotoId), Times.Once);
    }
}