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
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DiveService _service;

        public DiveServiceTests()
        {
            _diveRepoMock = new Mock<IDiveRepository>();
            _equipmentRepoMock = new Mock<IEquipmentRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new DiveService(
                _diveRepoMock.Object,
                _equipmentRepoMock.Object,
                _userRepoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task CreateDiveAsync_Should_Add_Dive_And_Return_Dto()
        {
            // Arrange
            var diveSaveDto = new DiveSaveDto
            {
                DiveName = "Test Dive",
                Depth = 25.5f,
                Duration = 45,
                DiveDate = DateTime.Now,
                Description = "Test description",
                Equipments = new List<EquipmentDto> { new EquipmentDto { EquipmentId = 1 } }
            };

            var user = new User { UserId = 1, Auth0UserId = "auth0|123456" };
            var equipments = new List<Equipment> { new Equipment { EquipmentId = 1 } };
            var dive = new Dive { DiveId = 1, UserId = 1, DiveName = "Test Dive" };
            var diveDto = new DiveDto { DiveId = 1, DiveName = "Test Dive" };

            _userRepoMock.Setup(r => r.GetByAuth0UserIdAsync("auth0|123456")).ReturnsAsync(user);
            _equipmentRepoMock.Setup(r => r.GetEquipmentsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(equipments);
            _mapperMock.Setup(m => m.Map<Dive>(diveSaveDto)).Returns(dive);
            _mapperMock.Setup(m => m.Map<DiveDto>(dive)).Returns(diveDto);

            // Act
            var result = await _service.CreateDiveAsync(diveSaveDto, "auth0|123456");

            // Assert
            Assert.Equal(diveDto, result);
            _diveRepoMock.Verify(r => r.AddAsync(dive), Times.Once);
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
            var user = new User { UserId = 42, Auth0UserId = "auth0|123456" };
            var dives = new List<Dive> { new Dive { DiveId = 2, UserId = 42 } };
            var dtos = new List<DiveDto> { new DiveDto { DiveId = 2 } };
            _userRepoMock.Setup(r => r.GetByAuth0UserIdAsync("auth0|123456")).ReturnsAsync(user);
            _diveRepoMock.Setup(r => r.GetDivesWihDetails(42)).ReturnsAsync(dives);
            _mapperMock.Setup(m => m.Map<List<DiveDto>>(dives)).Returns(dtos);

            // Act
            var result = await _service.GetAllDivesAsync("auth0|123456");

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
            // Remplacez la ligne incorrecte suivante :
            // .Setup(r => r.GetEquipmentsByIdsAsync(It.Is<List<int>>(l => l.SequenceEqual(new List<int> { 2 })))
            // .ReturnsAsync(new List<Equipment> { new Equipment { EquipmentId = 2 } });

            // Par la version correcte ci-dessous :
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