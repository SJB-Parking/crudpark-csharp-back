using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;

namespace CrudPark_Back.Services.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<VehicleResponse>> GetAllVehiclesAsync();
    Task<VehicleResponse?> GetVehicleByIdAsync(int id);
    Task<VehicleResponse?> GetVehicleByLicensePlateAsync(string licensePlate);
    Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request);
    Task<VehicleResponse> UpdateVehicleAsync(int id, UpdateVehicleRequest request);
    Task<bool> DeleteVehicleAsync(int id);
}