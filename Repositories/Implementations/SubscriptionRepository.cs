using CrudPark_Back.Data;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Repositories.Implementations;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MonthlySubscription>> GetAllAsync()
    {
        return await _context.MonthlySubscriptions
            .Include(s => s.Customer)
            .Include(s => s.SubscriptionVehicles)
                .ThenInclude(sv => sv.Vehicle)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<MonthlySubscription?> GetByIdAsync(int id)
    {
        return await _context.MonthlySubscriptions
            .Include(s => s.Customer)
            .Include(s => s.SubscriptionVehicles)
                .ThenInclude(sv => sv.Vehicle)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<MonthlySubscription>> GetActiveSubscriptionsAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.MonthlySubscriptions
            .Include(s => s.Customer)
            .Include(s => s.SubscriptionVehicles)
                .ThenInclude(sv => sv.Vehicle)
            .Where(s => s.IsActive && s.EndDate >= today)
            .ToListAsync();
    }

    public async Task<IEnumerable<MonthlySubscription>> GetExpiringSoonAsync(int daysThreshold = 3)
    {
        var today = DateTime.UtcNow.Date;
        var thresholdDate = today.AddDays(daysThreshold);
        
        return await _context.MonthlySubscriptions
            .Include(s => s.Customer)
            .Include(s => s.SubscriptionVehicles)
                .ThenInclude(sv => sv.Vehicle)
            .Where(s => s.IsActive && s.EndDate >= today && s.EndDate <= thresholdDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<MonthlySubscription>> GetExpiredSubscriptionsAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.MonthlySubscriptions
            .Include(s => s.Customer)
            .Where(s => s.EndDate < today)
            .ToListAsync();
    }

    public async Task<IEnumerable<MonthlySubscription>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.MonthlySubscriptions
            .Include(s => s.SubscriptionVehicles)
                .ThenInclude(sv => sv.Vehicle)
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<MonthlySubscription?> GetByVehicleIdAsync(int vehicleId)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.MonthlySubscriptions
            .Include(s => s.Customer)
            .Include(s => s.SubscriptionVehicles)
                .ThenInclude(sv => sv.Vehicle)
            .Where(s => s.IsActive 
                && s.StartDate <= today 
                && s.EndDate >= today
                && s.SubscriptionVehicles.Any(sv => sv.VehicleId == vehicleId))
            .FirstOrDefaultAsync();
    }

    public async Task<MonthlySubscription> CreateAsync(MonthlySubscription subscription)
    {
        _context.MonthlySubscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<MonthlySubscription> UpdateAsync(MonthlySubscription subscription)
    {
        subscription.UpdatedAt = DateTime.UtcNow;
        _context.MonthlySubscriptions.Update(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var subscription = await _context.MonthlySubscriptions.FindAsync(id);
        if (subscription == null) return false;

        subscription.IsActive = false;
        subscription.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddVehicleToSubscriptionAsync(int subscriptionId, int vehicleId)
    {
        var subscription = await _context.MonthlySubscriptions
            .Include(s => s.SubscriptionVehicles)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId);

        if (subscription == null) return false;

        // Verificar que no exceda el máximo de vehículos
        if (subscription.SubscriptionVehicles.Count >= subscription.MaxVehicles)
            return false;

        // Verificar que el vehículo no esté ya agregado
        if (subscription.SubscriptionVehicles.Any(sv => sv.VehicleId == vehicleId))
            return false;

        var subscriptionVehicle = new SubscriptionVehicle
        {
            SubscriptionId = subscriptionId,
            VehicleId = vehicleId,
            AddedAt = DateTime.UtcNow
        };

        _context.SubscriptionVehicles.Add(subscriptionVehicle);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveVehicleFromSubscriptionAsync(int subscriptionId, int vehicleId)
    {
        var subscriptionVehicle = await _context.SubscriptionVehicles
            .FirstOrDefaultAsync(sv => sv.SubscriptionId == subscriptionId && sv.VehicleId == vehicleId);

        if (subscriptionVehicle == null) return false;

        _context.SubscriptionVehicles.Remove(subscriptionVehicle);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SubscriptionCodeExistsAsync(string code, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.MonthlySubscriptions
                .AnyAsync(s => s.SubscriptionCode == code && s.Id != excludeId.Value);
        }
        return await _context.MonthlySubscriptions.AnyAsync(s => s.SubscriptionCode == code);
    }
}