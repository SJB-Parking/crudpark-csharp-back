using CrudPark_Back.Models.Entities;

namespace CrudPark_Back.Repositories.Interfaces;

public interface ISubscriptionRepository
{
    Task<IEnumerable<MonthlySubscription>> GetAllAsync();
    Task<MonthlySubscription?> GetByIdAsync(int id);
    Task<IEnumerable<MonthlySubscription>> GetActiveSubscriptionsAsync();
    Task<IEnumerable<MonthlySubscription>> GetExpiringSoonAsync(int daysThreshold = 3);
    Task<IEnumerable<MonthlySubscription>> GetExpiredSubscriptionsAsync();
    Task<IEnumerable<MonthlySubscription>> GetByCustomerIdAsync(int customerId);
    Task<MonthlySubscription?> GetByVehicleIdAsync(int vehicleId);
    Task<MonthlySubscription> CreateAsync(MonthlySubscription subscription);
    Task<MonthlySubscription> UpdateAsync(MonthlySubscription subscription);
    Task<bool> DeleteAsync(int id);
    Task<bool> AddVehicleToSubscriptionAsync(int subscriptionId, int vehicleId);
    Task<bool> RemoveVehicleFromSubscriptionAsync(int subscriptionId, int vehicleId);
    Task<bool> SubscriptionCodeExistsAsync(string code, int? excludeId = null);
}