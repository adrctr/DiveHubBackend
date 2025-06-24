using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Core.Entities;

namespace DiveHub.Application.Mapping;

/// <summary>
/// Configures mappings for AutoMapper between domain entities and DTOs.
/// This class ensures that entities like User and Dive can be easily mapped to their respective DTOs, and vice versa.
/// </summary>
public class MappingProfile : Profile
{    
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// Configures bidirectional mappings between entities and DTOs.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        
        CreateMap<Dive, DiveDto>().ReverseMap();
        CreateMap<DiveSaveDto, Dive>().ReverseMap();
        CreateMap<DiveDetailDto, Dive>().ReverseMap();

        CreateMap<Equipment, EquipmentDto>().ReverseMap();
        CreateMap<EquipmentSaveDto, Equipment>().ReverseMap();

    }
}