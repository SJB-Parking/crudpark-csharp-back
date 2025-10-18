using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;
using CrudPark_Back.Models.Enums;

namespace CrudPark_Back.Services.Interfaces;

public interface IRateService
{
    Task<IEnumerable<RateResponse>> GetAllRatesAsync();
    Task<RateResponse?> GetRateByIdAsync(int id);
    Task<RateResponse?> GetActiveRateAsync();
    Task<RateResponse> CreateRateAsync(CreateRateRequest request);
    Task<RateResponse> UpdateRateAsync(int id, UpdateRateRequest request);
    Task<bool> DeleteRateAsync(int id);
    Task<decimal> CalculateParkingFeeAsync(DateTime entryTime, DateTime exitTime, VehicleType vehicleType);  // ⭐ ACTUALIZADO
}