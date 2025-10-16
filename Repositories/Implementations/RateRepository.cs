using CrudPark_Back.Data;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Repositories.Implementations;

public class RateRepository : IRateRepository
{
    private readonly ApplicationDbContext _context;

    public RateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rate>> GetAllAsync()
    {
        return await _context.Rates
            .OrderByDescending(r => r.EffectiveFrom)
            .ToListAsync();
    }

    public async Task<Rate?> GetByIdAsync(int id)
    {
        return await _context.Rates
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Rate?> GetActiveRateAsync()
    {
        return await _context.Rates
            .Where(r => r.IsActive)
            .OrderByDescending(r => r.EffectiveFrom)
            .FirstOrDefaultAsync();
    }

    public async Task<Rate> CreateAsync(Rate rate)
    {
        _context.Rates.Add(rate);
        await _context.SaveChangesAsync();
        return rate;
    }

    public async Task<Rate> UpdateAsync(Rate rate)
    {
        rate.UpdatedAt = DateTime.UtcNow;
        _context.Rates.Update(rate);
        await _context.SaveChangesAsync();
        return rate;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var rate = await _context.Rates.FindAsync(id);
        if (rate == null) return false;

        _context.Rates.Remove(rate);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeactivateAllRatesAsync()
    {
        var activeRates = await _context.Rates
            .Where(r => r.IsActive)
            .ToListAsync();

        foreach (var rate in activeRates)
        {
            rate.IsActive = false;
            rate.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }
}