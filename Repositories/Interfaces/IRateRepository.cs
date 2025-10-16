using CrudPark_Back.Models.Entities;

namespace CrudPark_Back.Repositories.Interfaces;

public interface IRateRepository
{
    Task<IEnumerable<Rate>> GetAllAsync();
    Task<Rate?> GetByIdAsync(int id);
    Task<Rate?> GetActiveRateAsync();
    Task<Rate> CreateAsync(Rate rate);
    Task<Rate> UpdateAsync(Rate rate);
    Task<bool> DeleteAsync(int id);
    Task DeactivateAllRatesAsync();
}