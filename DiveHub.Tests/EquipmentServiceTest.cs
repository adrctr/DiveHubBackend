using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Infrastructure.repositories;
using Moq;


namespace DiveHub.Application.Tests
{
    public class EquipmentServiceTests
    {
        private readonly Mock<IEquipmentRepository> _equipmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly EquipmentService _service;

        public EquipmentServiceTests()
        {
            _equipmentRepositoryMock = new Mock<IEquipmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new EquipmentService(_equipmentRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateEquipmentAsync_ShouldAddAndReturnDto()
        {
            // Arrange
            var saveDto = new EquipmentSaveDto { EquipmentName = "Palmes" };
            var equipment = new Equipment { EquipmentId = 1, EquipmentName = "Palmes" };
            var dto = new EquipmentDto { EquipmentId = 1, EquipmentName = "Palmes" };

            _mapperMock.Setup(m => m.Map<Equipment>(saveDto)).Returns(equipment);
            _equipmentRepositoryMock.Setup(r => r.AddAsync(equipment)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<EquipmentDto>(equipment)).Returns(dto);

            // Act
            var result = await _service.CreateEquipmentAsync(saveDto);

            // Assert
            Assert.Equal(dto, result);
            _equipmentRepositoryMock.Verify(r => r.AddAsync(equipment), Times.Once);
        }

        [Fact]
        public async Task GetAllEquipmentsAsync_ShouldReturnMappedDtos()
        {
            // Arrange
            var equipments = new List<Equipment>
            {
                new Equipment { EquipmentId = 1, EquipmentName = "Palmes" },
                new Equipment { EquipmentId = 2, EquipmentName = "Masque" }
            };
            var dtos = new List<EquipmentDto>
            {
                new EquipmentDto { EquipmentId = 1, EquipmentName = "Palmes" },
                new EquipmentDto { EquipmentId = 2, EquipmentName = "Masque" }
            };

            _equipmentRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(equipments);
            _mapperMock.Setup(m => m.Map<List<EquipmentDto>>(equipments)).Returns(dtos);

            // Act
            var result = await _service.GetAllEquipmentsAsync();

            // Assert
            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task UpdateEquipmentAsync_ShouldUpdateWhenExists()
        {
            // Arrange
            var dto = new EquipmentDto { EquipmentId = 1, EquipmentName = "Tuba" };
            var equipment = new Equipment { EquipmentId = 1, EquipmentName = "Ancien" };

            _equipmentRepositoryMock.Setup(r => r.GetByIdAsync(dto.EquipmentId)).ReturnsAsync(equipment);
            _mapperMock.Setup(m => m.Map(dto, equipment));
            _equipmentRepositoryMock.Setup(r => r.UpdateAsync(equipment)).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateEquipmentAsync(dto);

            // Assert
            _equipmentRepositoryMock.Verify(r => r.UpdateAsync(equipment), Times.Once);
        }

        [Fact]
        public async Task UpdateEquipmentAsync_ShouldThrowIfNotExists()
        {
            // Arrange
            var dto = new EquipmentDto { EquipmentId = 99, EquipmentName = "Inexistant" };
            _equipmentRepositoryMock.Setup(r => r.GetByIdAsync(dto.EquipmentId)).ReturnsAsync((Equipment)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateEquipmentAsync(dto));
        }

        [Fact]
        public async Task DeleteEquipmentAsync_ShouldCallRepository()
        {
            // Arrange
            int id = 5;
            _equipmentRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteEquipmentAsync(id);

            // Assert
            _equipmentRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}
