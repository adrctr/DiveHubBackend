using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Infrastructure.repositories;
using Moq;

namespace DiveHub.Tests;

public class DiveServiceTests
{
    private readonly Mock<IDiveRepository> _mockDiveRepository;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DiveService _diveService;

    public DiveServiceTests()
    {
        _mockDiveRepository = new Mock<IDiveRepository>();
        _mapperMock = new Mock<IMapper>();

        _diveService = new DiveService(_mockDiveRepository.Object,_mapperMock.Object);
    }

    [Fact]
    public async Task CreateDiveAsync_Should_Map_Save_And_Return_DiveDto()
    {
        // Arrange
        var diveSaveDto = new DiveSaveDto { /* initialise tes données */ };
        var dive = new Dive { DiveId = 1, UserId = 123 };
        var diveDto = new DiveDto { DiveId = 1 };

        _mapperMock.Setup(m => m.Map<Dive>(diveSaveDto)).Returns(dive);
        _mapperMock.Setup(m => m.Map<DiveDto>(dive)).Returns(diveDto);

        // Act
        var result = await _diveService.CreateDiveAsync(diveSaveDto, 123);

        // Assert
        _mockDiveRepository.Verify(r => r.AddAsync(dive), Times.Once);
        Assert.Equal(diveDto.DiveId, result.DiveId);
    }
}