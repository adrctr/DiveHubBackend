using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Infrastructure.repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DiveHub.Application.Tests
{
    public class DiveServiceTests
    {
        private readonly Mock<IDiveRepository> _diveRepoMock;
        private readonly Mock<IEquipmentRepository> _equipmentRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DiveService _service;

        public DiveServiceTests()
        {
            _diveRepoMock = new Mock<IDiveRepository>();
            _equipmentRepoMock = new Mock<IEquipmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new DiveService(
                _diveRepoMock.Object,
                _equipmentRepoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task CreateDiveAsync_Should_Add_Dive_And_Return_Dto()
        {
            // Arrange
            var saveDto = new DiveSaveDto
            {
                DiveName = "Test",
                DiveDate = DateTime.Today,
                Depth = 10f,
                Duration = 60,
                Description = "Desc",
                Equipments = new List<EquipmentDto> { new EquipmentDto { EquipmentId = 1 } }
            };

            var diveEntity = new Dive { DiveId = 1, UserId = 42 };
            var diveDto = new DiveDto { DiveId = 1, DiveName = "Test" };

            _mapperMock.Setup(m => m.Map<Dive>(saveDto)).Returns(diveEntity);
            _equipmentRepoMock
                .Setup(r => r.GetEquipmentsByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<Equipment> { new Equipment { EquipmentId = 1 } });
            _mapperMock.Setup(m => m.Map<DiveDto>(diveEntity)).Returns(diveDto);

            // Act
            var result = await _service.CreateDiveAsync(saveDto, userId: 42);

            // Assert
            _diveRepoMock.Verify(r => r.AddAsync(diveEntity), Times.Once);
            Assert.Equal(diveDto, result);
        }

        [Fact]
        public async Task GetDiveByIdAsync_Should_Return_Dto_When_Found()
        {
            // Arrange
            var dive = new Dive { DiveId = 5 };
            var dto = new DiveDto { DiveId = 5 };
            _diveRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(dive);
            _mapperMock.Setup(m => m.Map<DiveDto?>(dive)).Returns(dto);

            // Act
            var result = await _service.GetDiveByIdAsync(5);

            // Assert
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetAllDivesAsync_Should_Return_List_Of_Dtos()
        {
            // Arrange
            var dives = new List<Dive> { new Dive { DiveId = 2 } };
            var dtos = new List<DiveDto> { new DiveDto { DiveId = 2 } };
            _diveRepoMock.Setup(r => r.GetDivesWihDetails()).ReturnsAsync(dives);
            _mapperMock.Setup(m => m.Map<List<DiveDto>>(dives)).Returns(dtos);

            // Act
            var result = await _service.GetAllDivesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result.First().DiveId);
        }

        [Fact]
        public async Task UpdateDiveAsync_Should_Throw_When_Not_Found()
        {
            // Arrange
            _diveRepoMock.Setup(r => r.GetDiveByIdAsync(10)).ReturnsAsync((Dive?)null);
            var dto = new DiveDto { DiveId = 10 };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.UpdateDiveAsync(dto));
        }

        [Fact]
        public async Task UpdateDiveAsync_Should_Update_Equipments_Correctly()
        {
            // Arrange existing dive with one equipment
            var existing = new Dive
            {
                DiveId = 3,
                Equipments = new List<Equipment> { new Equipment { EquipmentId = 1 } }
            };
            var dto = new DiveDto
            {
                DiveId = 3,
                Equipments = new List<EquipmentDto>
                {
                    new EquipmentDto { EquipmentId = 2 }
                }
            };
            _diveRepoMock.Setup(r => r.GetDiveByIdAsync(3)).ReturnsAsync(existing);
            _equipmentRepoMock
                .Setup(r => r.GetEquipmentsByIdsAsync(It.Is<List<int>>(l => l.SequenceEqual(new List<int> { 2 }))))
                .ReturnsAsync(new List<Equipment> { new Equipment { EquipmentId = 2 } });

            // Act
            await _service.UpdateDiveAsync(dto);

            // Assert
            Assert.Single(existing.Equipments);
            Assert.Contains(existing.Equipments, e => e.EquipmentId == 2);
            _diveRepoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Fact]
        public async Task DeleteDiveAsync_Should_Call_Repository()
        {
            // Act
            await _service.DeleteDiveAsync(7);

            // Assert
            _diveRepoMock.Verify(r => r.DeleteAsync(7), Times.Once);
        }
    }
}
