using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DiveHub.Infrastructure.repositories;

public interface IEquipmentRepository : IRepository<Equipment>
{
}

public class EquipmentRepository(SQLiteDbContext context) : GenericRepository<Equipment>(context), IEquipmentRepository
{
}