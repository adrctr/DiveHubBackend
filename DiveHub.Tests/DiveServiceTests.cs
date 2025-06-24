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
    public class DiveServiceTests
    {
        private readonly Mock<IDiveRepository> _diveRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DiveService _diveService;

        public DiveServiceTests()
        {
            _diveRepositoryMock = new Mock<IDiveRepository>();
            _mapperMock = new Mock<IMapper>();

            _diveService = new DiveService(
                _diveRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task CreateDiveAsync_Should_Map_And_Add_Dive_With_Equipments()
        {
            // Arrange
            var diveSaveDto = new DiveSaveDto
            {
                DiveName = "Plongée à Bali",
                DiveDate = DateTime.UtcNow,
                Equipments =
                [
                    new() { EquipmentId = 1, EquipmentName = "Bouteille 12L" },
                    new() { EquipmentId = 2, EquipmentName = "Gilet stabilisateur" },
                    new() { EquipmentId = 3, EquipmentName = "Go Pro" }
                ]
            };


            var diveEntity = new Dive
            {
                DiveId = 1,
                DiveName = diveSaveDto.DiveName,
                DiveDate = diveSaveDto.DiveDate,
                Equipments =
                [
                    new() { EquipmentId = 1, EquipmentName = "Bouteille 12L" },
                    new() { EquipmentId = 2, EquipmentName = "Gilet stabilisateur" },
                    new() { EquipmentId = 3, EquipmentName = "Go Pro" }
                ]
            };

            var expectedDiveDto = new DiveDto
            {
                DiveId = 1,
                DiveName = diveSaveDto.DiveName,
                DiveDate = diveSaveDto.DiveDate,
                Equipments = diveSaveDto.Equipments
            };

            // Setup mapper behavior
            _mapperMock
                .Setup(m => m.Map<Dive>(diveSaveDto))
                .Returns(diveEntity);

            _mapperMock
                .Setup(m => m.Map<DiveDto>(diveEntity))
                .Returns(expectedDiveDto);

            // Act
            var result = await _diveService.CreateDiveAsync(diveSaveDto, 1);

            // Assert
            _diveRepositoryMock.Verify(r => r.AddAsync(It.Is<Dive>(d =>
                d.DiveName == diveSaveDto.DiveName &&
                d.DiveDate == diveSaveDto.DiveDate &&
                d.Equipments.Count == diveSaveDto.Equipments.Count &&
                d.Equipments.All(e => diveSaveDto.Equipments.Any(dto => dto.EquipmentId == e.EquipmentId && dto.EquipmentName == e.EquipmentName))

            )), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(expectedDiveDto.DiveId, result.DiveId);
            Assert.Equal(expectedDiveDto.DiveName, result.DiveName);
            Assert.Equal(expectedDiveDto.DiveDate, result.DiveDate);
            Assert.Equal(expectedDiveDto.Equipments.Count, result.Equipments.Count);

            foreach (var equipmentDto in expectedDiveDto.Equipments)
            {
                var matchingEquipment = result.Equipments.FirstOrDefault(e => e.EquipmentId == equipmentDto.EquipmentId);
                Assert.NotNull(matchingEquipment);
                Assert.Equal(equipmentDto.EquipmentName, matchingEquipment.EquipmentName);
            }
        }
    }
}
