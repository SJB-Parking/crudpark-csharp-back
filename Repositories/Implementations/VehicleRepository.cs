using CrudPark_Back.Data;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Repositories.Implementations;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles
            .Include(v => v.CustomerVehicles)
                .ThenInclude(cv => cv.Customer)
            .OrderBy(v => v.LicensePlate)
            .ToListAsync();
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _context.Vehicles
            .Include(v => v.CustomerVehicles)
                .ThenInclude(cv => cv.Customer)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        return await _context.Vehicles
            .Include(v => v.SubscriptionVehicles)
                .ThenInclude(sv => sv.MonthlySubscription)
            .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate.ToUpper());
    }

    public async Task<Vehicle> CreateAsync(Vehicle vehicle)
    {
        vehicle.LicensePlate = vehicle.LicensePlate.ToUpper();
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
    {
        vehicle.UpdatedAt = DateTime.UtcNow;
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null) return false;

        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LicensePlateExistsAsync(string licensePlate, int? excludeId = null)
    {
        licensePlate = licensePlate.ToUpper();
        if (excludeId.HasValue)
        {
            return await _context.Vehicles
                .AnyAsync(v => v.LicensePlate == licensePlate && v.Id != excludeId.Value);
        }
        return await _context.Vehicles.AnyAsync(v => v.LicensePlate == licensePlate);
    }
}