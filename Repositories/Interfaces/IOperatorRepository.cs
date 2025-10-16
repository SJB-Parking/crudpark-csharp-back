using CrudPark_Back.Models.Entities;

namespace CrudPark_Back.Repositories.Interfaces;

public interface IOperatorRepository
{
    Task<IEnumerable<Operator>> GetAllAsync();
    Task<Operator?> GetByIdAsync(int id);
    Task<Operator?> GetByUsernameAsync(string username);
    Task<IEnumerable<Operator>> GetActiveOperatorsAsync();
    Task<Operator> CreateAsync(Operator operatorEntity);
    Task<Operator> UpdateAsync(Operator operatorEntity);
    Task<bool> DeleteAsync(int id);
    Task<bool> UsernameExistsAsync(string username, int? excludeId = null);
}