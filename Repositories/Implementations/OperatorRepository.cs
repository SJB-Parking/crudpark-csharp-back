using CrudPark_Back.Data;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Repositories.Implementations;

public class OperatorRepository : IOperatorRepository
{
    private readonly ApplicationDbContext _context;

    public OperatorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Operator>> GetAllAsync()
    {
        return await _context.Operators
            .OrderBy(o => o.FullName)
            .ToListAsync();
    }

    public async Task<Operator?> GetByIdAsync(int id)
    {
        return await _context.Operators
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Operator?> GetByUsernameAsync(string username)
    {
        return await _context.Operators
            .FirstOrDefaultAsync(o => o.Username == username);
    }

    public async Task<IEnumerable<Operator>> GetActiveOperatorsAsync()
    {
        return await _context.Operators
            .Where(o => o.IsActive)
            .OrderBy(o => o.FullName)
            .ToListAsync();
    }

    public async Task<Operator> CreateAsync(Operator operatorEntity)
    {
        _context.Operators.Add(operatorEntity);
        await _context.SaveChangesAsync();
        return operatorEntity;
    }

    public async Task<Operator> UpdateAsync(Operator operatorEntity)
    {
        operatorEntity.UpdatedAt = DateTime.UtcNow;
        _context.Operators.Update(operatorEntity);
        await _context.SaveChangesAsync();
        return operatorEntity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var operatorEntity = await _context.Operators.FindAsync(id);
        if (operatorEntity == null) return false;

        operatorEntity.IsActive = false;
        operatorEntity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UsernameExistsAsync(string username, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Operators
                .AnyAsync(o => o.Username == username && o.Id != excludeId.Value);
        }
        return await _context.Operators.AnyAsync(o => o.Username == username);
    }
}