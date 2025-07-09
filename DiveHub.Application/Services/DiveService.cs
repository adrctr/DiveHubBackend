﻿using AutoMapper;
using DiveHub.Application.Dto;
using DiveHub.Application.Interfaces;
using DiveHub.Core.Entities;
using DiveHub.Infrastructure.repositories;
using Microsoft.EntityFrameworkCore;
namespace DiveHub.Application.Services;

public class DiveService(
    IDiveRepository diveRepository,
    IEquipmentRepository equipmentRepository,
    IMapper mapper) : IDiveService
{
    public async Task<DiveDto> CreateDiveAsync(DiveSaveDto diveSaveDto, int userId)
    {
        // Mapper uniquement les propriétés simples (ne pas mapper Equipments ici !)
        Dive dive = mapper.Map<Dive>(diveSaveDto);
        dive.UserId = userId;

        // Récupérer les EquipmentId sélectionnés
        var equipmentIds = diveSaveDto.Equipments
            .Select(e => e.EquipmentId)
            .Distinct()
            .ToList();

        if (equipmentIds.Any())
        {
            // Charger les équipements existants depuis la base de données
            var equipments = await equipmentRepository.GetEquipmentsByIdsAsync(equipmentIds);

            // Vérifier qu'ils existent tous (sécurité)
            if (equipments.Count != equipmentIds.Count)
                throw new InvalidOperationException("Un ou plusieurs équipements sélectionnés n'existent pas.");

            // Associer les équipements existants à la plongée
            dive.Equipments = equipments;
        }

        // Sauvegarder
        await diveRepository.AddAsync(dive);

        return mapper.Map<DiveDto>(dive);
    }
    
    public async Task<DiveDto?> GetDiveByIdAsync(int diveId)
    {
        var dive = await diveRepository.GetByIdAsync(diveId);
        return mapper.Map<DiveDto?>(dive);
    }

    public async Task<IEnumerable<DiveDto>> GetAllDivesAsync()
    {
        var dives = await diveRepository.GetDivesWihDetails();
        return mapper.Map<List<DiveDto>>(dives);
    }

    public async Task UpdateDiveAsync(DiveDto diveDto)
    {
        var existingDive = await diveRepository.GetDiveByIdAsync(diveDto.DiveId);
        if (existingDive is null)
            throw new InvalidOperationException($"La plongée avec l'ID {diveDto.DiveId} n'existe pas.");

        // Mise à jour des champs simples
        existingDive.DiveName = diveDto.DiveName;
        existingDive.DiveDate = diveDto.DiveDate;
        existingDive.Description = diveDto.Description;
        existingDive.Depth = diveDto.Depth;
        existingDive.Duration = diveDto.Duration;

        // Extraire les EquipmentIds du DTO
        var equipmentIds = diveDto.Equipments.Select(e => e.EquipmentId).ToList();

        // Supprimer les équipements non sélectionnés
        foreach (var equipment in existingDive.Equipments.ToList())
        {
            if (!equipmentIds.Contains(equipment.EquipmentId))
            {
                existingDive.Equipments.Remove(equipment);
            }
        }

        // Ajouter les nouveaux équipements s'ils ne sont pas déjà liés
        foreach (var equipmentId in equipmentIds)
        {
            if (!existingDive.Equipments.Any(e => e.EquipmentId == equipmentId))
            {
                var equipment = new Equipment { EquipmentId = equipmentId };
                existingDive.Equipments.Add(equipment);
            }
        }
        await diveRepository.UpdateAsync(existingDive);
    }

    public async Task DeleteDiveAsync(int diveId)
    {
        await diveRepository.DeleteAsync(diveId);
    }
}