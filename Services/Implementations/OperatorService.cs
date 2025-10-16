using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using CrudPark_Back.Services.Interfaces;

namespace CrudPark_Back.Services.Implementations;

public class OperatorService : IOperatorService
{
    private readonly IOperatorRepository _operatorRepository;

    public OperatorService(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public async Task<IEnumerable<OperatorResponse>> GetAllOperatorsAsync()
    {
        var operators = await _operatorRepository.GetAllAsync();
        return operators.Select(MapToResponse);
    }

    public async Task<OperatorResponse?> GetOperatorByIdAsync(int id)
    {
        var operatorEntity = await _operatorRepository.GetByIdAsync(id);
        return operatorEntity == null ? null : MapToResponse(operatorEntity);
    }

    public async Task<IEnumerable<OperatorResponse>> GetActiveOperatorsAsync()
    {
        var operators = await _operatorRepository.GetActiveOperatorsAsync();
        return operators.Select(MapToResponse);
    }

    public async Task<OperatorResponse> CreateOperatorAsync(CreateOperatorRequest request)
    {
        // Validar username único
        if (await _operatorRepository.UsernameExistsAsync(request.Username))
            throw new InvalidOperationException("El nombre de usuario ya existe");

        // Hashear la contraseña
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var operatorEntity = new Operator
        {
            FullName = request.FullName,
            Email = request.Email,
            Username = request.Username,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        operatorEntity = await _operatorRepository.CreateAsync(operatorEntity);
        return MapToResponse(operatorEntity);
    }

    public async Task<OperatorResponse> UpdateOperatorAsync(int id, UpdateOperatorRequest request)
    {
        var operatorEntity = await _operatorRepository.GetByIdAsync(id);
        if (operatorEntity == null)
            throw new KeyNotFoundException("Operador no encontrado");

        // Actualizar campos
        if (!string.IsNullOrEmpty(request.FullName))
            operatorEntity.FullName = request.FullName;
        
        if (!string.IsNullOrEmpty(request.Email))
            operatorEntity.Email = request.Email;
        
        if (!string.IsNullOrEmpty(request.Password))
            operatorEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        if (request.IsActive.HasValue)
            operatorEntity.IsActive = request.IsActive.Value;

        operatorEntity = await _operatorRepository.UpdateAsync(operatorEntity);
        return MapToResponse(operatorEntity);
    }

    public async Task<bool> DeleteOperatorAsync(int id)
    {
        return await _operatorRepository.DeleteAsync(id);
    }

    public async Task<OperatorResponse?> ValidateOperatorCredentialsAsync(string username, string password)
    {
        var operatorEntity = await _operatorRepository.GetByUsernameAsync(username);
        
        if (operatorEntity == null || !operatorEntity.IsActive)
            return null;

        // Verificar la contraseña
        if (!BCrypt.Net.BCrypt.Verify(password, operatorEntity.PasswordHash))
            return null;

        return MapToResponse(operatorEntity);
    }

    private OperatorResponse MapToResponse(Operator operatorEntity)
    {
        return new OperatorResponse
        {
            Id = operatorEntity.Id,
            FullName = operatorEntity.FullName,
            Email = operatorEntity.Email,
            Username = operatorEntity.Username,
            IsActive = operatorEntity.IsActive,
            CreatedAt = operatorEntity.CreatedAt
        };
    }
}