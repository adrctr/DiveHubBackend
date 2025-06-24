using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Infrastructure.repositories;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace DiveHub.Tests.Services
{
    public class EquipmentServiceTests
    {
        private readonly Mock<IEquipmentRepository> _equipmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly EquipmentService _equipmentService;

        public EquipmentServiceTests()
        {
            _equipmentRepositoryMock = new Mock<IEquipmentRepository>();
            _mapperMock = new Mock<IMapper>();

            _equipmentService = new EquipmentService(
                _equipmentRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task CreateEquipmentAsync_Should_Map_And_Add_Equipment()
        {
            // Arrange
            var equipmentSaveDto = new EquipmentSaveDto
            {
                EquipmentName = "Ordinateur de plongée"
            };

            var equipmentEntity = new Equipment
            {
                EquipmentId = 1,
                EquipmentName = equipmentSaveDto.EquipmentName
            };

            var expectedEquipmentDto = new EquipmentDto
            {
                EquipmentId = 1,
                EquipmentName = equipmentSaveDto.EquipmentName
            };

            // Setup mapper behavior
            _mapperMock
                .Setup(m => m.Map<Equipment>(equipmentSaveDto))
                .Returns(equipmentEntity);

            _mapperMock
                .Setup(m => m.Map<EquipmentDto>(equipmentEntity))
                .Returns(expectedEquipmentDto);

            // Act
            var result = await _equipmentService.CreateEquipmentAsync(equipmentSaveDto);

            // Assert
            _equipmentRepositoryMock.Verify(r => r.AddAsync(It.Is<Equipment>(e =>
                e.EquipmentName == equipmentSaveDto.EquipmentName
            )), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(expectedEquipmentDto.EquipmentId, result.EquipmentId);
            Assert.Equal(expectedEquipmentDto.EquipmentName, result.EquipmentName);
        }
    }
}
