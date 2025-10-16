using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;

namespace CrudPark_Back.Services.Interfaces;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionResponse>> GetAllSubscriptionsAsync();
    Task<SubscriptionResponse?> GetSubscriptionByIdAsync(int id);
    Task<IEnumerable<SubscriptionResponse>> GetActiveSubscriptionsAsync();
    Task<IEnumerable<SubscriptionResponse>> GetExpiringSoonAsync(int daysThreshold = 3);
    Task<IEnumerable<SubscriptionResponse>> GetExpiredSubscriptionsAsync();
    Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request);
    Task<SubscriptionResponse> UpdateSubscriptionAsync(int id, UpdateSubscriptionRequest request);
    Task<bool> DeleteSubscriptionAsync(int id);
    Task<bool> AddVehicleToSubscriptionAsync(int subscriptionId, AddVehicleToSubscriptionRequest request);
    Task<bool> RemoveVehicleFromSubscriptionAsync(int subscriptionId, int vehicleId);
    Task CheckAndNotifyExpiringSubscriptionsAsync();
}