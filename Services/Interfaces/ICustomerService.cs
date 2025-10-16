using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;

namespace CrudPark_Back.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync();
    Task<CustomerResponse?> GetCustomerByIdAsync(int id);
    Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request);
    Task<CustomerResponse> UpdateCustomerAsync(int id, UpdateCustomerRequest request);
    Task<bool> DeleteCustomerAsync(int id);
    Task<bool> AddVehicleToCustomerAsync(int customerId, int vehicleId, bool isPrimary = false);
}