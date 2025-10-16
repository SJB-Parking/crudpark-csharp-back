using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;

namespace CrudPark_Back.Services.Interfaces;

public interface IOperatorService
{
    Task<IEnumerable<OperatorResponse>> GetAllOperatorsAsync();
    Task<OperatorResponse?> GetOperatorByIdAsync(int id);
    Task<IEnumerable<OperatorResponse>> GetActiveOperatorsAsync();
    Task<OperatorResponse> CreateOperatorAsync(CreateOperatorRequest request);
    Task<OperatorResponse> UpdateOperatorAsync(int id, UpdateOperatorRequest request);
    Task<bool> DeleteOperatorAsync(int id);
    Task<OperatorResponse?> ValidateOperatorCredentialsAsync(string username, string password);
}