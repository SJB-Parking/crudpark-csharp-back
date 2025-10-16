using CrudPark_Back.Models.Entities;

namespace CrudPark_Back.Repositories.Interfaces;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetByIdAsync(int id);
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
    Task<Vehicle> CreateAsync(Vehicle vehicle);
    Task<Vehicle> UpdateAsync(Vehicle vehicle);
    Task<bool> DeleteAsync(int id);
    Task<bool> LicensePlateExistsAsync(string licensePlate, int? excludeId = null);
}