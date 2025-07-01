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
                d.Equipments.All(e =>
                    diveSaveDto.Equipments.Any(dto =>
                        dto.EquipmentId == e.EquipmentId && dto.EquipmentName == e.EquipmentName))

            )), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(expectedDiveDto.DiveId, result.DiveId);
            Assert.Equal(expectedDiveDto.DiveName, result.DiveName);
            Assert.Equal(expectedDiveDto.DiveDate, result.DiveDate);
            Assert.Equal(expectedDiveDto.Equipments.Count, result.Equipments.Count);

            foreach (var equipmentDto in expectedDiveDto.Equipments)
            {
                var matchingEquipment =
                    result.Equipments.FirstOrDefault(e => e.EquipmentId == equipmentDto.EquipmentId);
                Assert.NotNull(matchingEquipment);
                Assert.Equal(equipmentDto.EquipmentName, matchingEquipment.EquipmentName);
            }
        }
        [Fact]
        public async Task UpdateDiveAsync_Should_Update_Fields_And_Sync_Equipments()
        {
            // Arrange
            var diveDto = new DiveDto
            {
                DiveId = 1,
                DiveName = "Plongée à Marseille",
                DiveDate = new DateTime(2024, 6, 1),
                Depth = 30,
                Duration = 45,
                Description = "Plongée sur épave",
                Equipments = new List<EquipmentDto>
                {
                    new() { EquipmentId = 1, EquipmentName = "Bouteille 12L" },
                    new() { EquipmentId = 2, EquipmentName = "Gilet stabilisateur" }
                }
            };

            var existingDive = new Dive
            {
                DiveId = 1,
                DiveName = "Ancien nom",
                DiveDate = new DateTime(2024, 5, 1),
                Depth = 10,
                Duration = 20,
                Description = "Ancienne description",
                Equipments = new List<Equipment>
                {
                    new() { EquipmentId = 1, EquipmentName = "Bouteille 12L" },
                    new() { EquipmentId = 3, EquipmentName = "Go Pro" }
                }
            };

            var diveRepositoryMock = new Mock<IDiveRepository>();

            diveRepositoryMock
                .Setup(r => r.GetDiveByIdAsync(diveDto.DiveId))
                .ReturnsAsync(existingDive);

            var service = new DiveService(diveRepositoryMock.Object, Mock.Of<IMapper>());

            // Act
            await service.UpdateDiveAsync(diveDto);

            // Assert

            // Vérifie les champs simples
            Assert.Equal(diveDto.DiveName, existingDive.DiveName);
            Assert.Equal(diveDto.DiveDate, existingDive.DiveDate);
            Assert.Equal(diveDto.Description, existingDive.Description);
            Assert.Equal(diveDto.Depth, existingDive.Depth);
            Assert.Equal(diveDto.Duration, existingDive.Duration);

            // Vérifie que l'équipement 3 (Go Pro) a été retiré
            Assert.DoesNotContain(existingDive.Equipments, e => e.EquipmentId == 3);

            // Vérifie que l'équipement 2 a été ajouté
            Assert.Contains(existingDive.Equipments, e => e.EquipmentId == 2);

            // Vérifie qu'il y a exactement 2 équipements maintenant
            Assert.Equal(2, existingDive.Equipments.Count);

            // Vérifie que la méthode UpdateAsync a bien été appelée
            diveRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Dive>(d =>
                d.DiveId == diveDto.DiveId &&
                d.DiveName == diveDto.DiveName &&
                d.Equipments.Any(e => e.EquipmentId == 1) &&
                d.Equipments.Any(e => e.EquipmentId == 2) &&
                !d.Equipments.Any(e => e.EquipmentId == 3)
            )), Times.Once);
        }
    }
}
