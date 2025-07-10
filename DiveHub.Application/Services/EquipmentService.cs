using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Infrastructure.repositories;


namespace DiveHub.Application.Services
{
    public class EquipmentService(IEquipmentRepository equipmentRepository,
    IMapper mapper) : IEquipmentService
    {
        public async Task<EquipmentDto> CreateEquipmentAsync(EquipmentSaveDto equipmentSaveDto)
        {
            var equipment = mapper.Map<Equipment>(equipmentSaveDto);
            await equipmentRepository.AddAsync(equipment);
            return mapper.Map<EquipmentDto>(equipment);
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentsAsync()
        {
            var equipment = await equipmentRepository.GetAllAsync();
            return mapper.Map<List<EquipmentDto>>(equipment);
        }

        public async Task UpdateEquipmentAsync(EquipmentDto equipmentDto)
        {
            var existingEquipment = await equipmentRepository.GetByIdAsync(equipmentDto.EquipmentId);
            if (existingEquipment is null)
                throw new InvalidOperationException($"L'équipement avec l'ID {equipmentDto.EquipmentId} n'existe pas.");

            mapper.Map(equipmentDto, existingEquipment);
            await equipmentRepository.UpdateAsync(existingEquipment);
        }
        public async Task DeleteEquipmentAsync(int id)
        {
            await equipmentRepository.DeleteAsync(id);
        }
    }
}
