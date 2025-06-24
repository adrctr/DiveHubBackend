using DiveHub.Application.Dto;
using DiveHub.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveHub.Application.Interfaces
{
    public interface IEquipmentService
    {
        Task<EquipmentDto> CreateEquipmentAsync(EquipmentSaveDto equipmentSaveDto);
    }
}
