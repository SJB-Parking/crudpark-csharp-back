using CrudPark_Back.Models.Entities;
using CrudPark_Back.Models.Enums;

namespace CrudPark_Back.Repositories.Interfaces;

public interface IRateRepository
{
    Task<IEnumerable<Rate>> GetAllAsync();
    Task<Rate?> GetByIdAsync(int id);
    Task<Rate?> GetActiveRateAsync();
    Task<Rate?> GetActiveRateByVehicleTypeAsync(VehicleType vehicleType);  // ⭐ AGREGAR
    Task<Rate> CreateAsync(Rate rate);
    Task<Rate> UpdateAsync(Rate rate);
    Task<bool> DeleteAsync(int id);
    Task DeactivateAllRatesAsync();
    Task DeactivateRatesByVehicleTypeAsync(VehicleType vehicleType);  // ⭐ AGREGAR
}