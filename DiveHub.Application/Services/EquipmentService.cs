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
        public async Task<EquipmentDto> CreateEquipmentAsync(EquipmentSaveDto equipementSaveDto)
        {
            var equipment = mapper.Map<Equipment>(equipementSaveDto);
            await equipmentRepository.AddAsync(equipment);
            return mapper.Map<EquipmentDto>(equipment);
        }
    }
}
