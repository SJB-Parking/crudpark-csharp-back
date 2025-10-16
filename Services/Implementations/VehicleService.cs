using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using CrudPark_Back.Services.Interfaces;

namespace CrudPark_Back.Services.Implementations;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IEnumerable<VehicleResponse>> GetAllVehiclesAsync()
    {
        var vehicles = await _vehicleRepository.GetAllAsync();
        return vehicles.Select(MapToResponse);
    }

    public async Task<VehicleResponse?> GetVehicleByIdAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        return vehicle == null ? null : MapToResponse(vehicle);
    }

    public async Task<VehicleResponse?> GetVehicleByLicensePlateAsync(string licensePlate)
    {
        var vehicle = await _vehicleRepository.GetByLicensePlateAsync(licensePlate);
        return vehicle == null ? null : MapToResponse(vehicle);
    }

    public async Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request)
    {
        // Validar placa única
        if (await _vehicleRepository.LicensePlateExistsAsync(request.LicensePlate))
            throw new InvalidOperationException("La placa ya está registrada");

        var vehicle = new Vehicle
        {
            LicensePlate = request.LicensePlate.ToUpper(),
            VehicleType = request.VehicleType,
            Brand = request.Brand,
            Model = request.Model,
            Color = request.Color,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        vehicle = await _vehicleRepository.CreateAsync(vehicle);
        return MapToResponse(vehicle);
    }

    public async Task<VehicleResponse> UpdateVehicleAsync(int id, UpdateVehicleRequest request)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle == null)
            throw new KeyNotFoundException("Vehículo no encontrado");

        // Actualizar campos
        if (request.VehicleType.HasValue)
            vehicle.VehicleType = request.VehicleType.Value;
        
        if (!string.IsNullOrEmpty(request.Brand))
            vehicle.Brand = request.Brand;
        
        if (!string.IsNullOrEmpty(request.Model))
            vehicle.Model = request.Model;
        
        if (!string.IsNullOrEmpty(request.Color))
            vehicle.Color = request.Color;

        vehicle = await _vehicleRepository.UpdateAsync(vehicle);
        return MapToResponse(vehicle);
    }

    public async Task<bool> DeleteVehicleAsync(int id)
    {
        return await _vehicleRepository.DeleteAsync(id);
    }

    private VehicleResponse MapToResponse(Vehicle vehicle)
    {
        return new VehicleResponse
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            VehicleType = vehicle.VehicleType.ToString(),
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            Color = vehicle.Color,
            IsPrimary = false // Se determina por la relación CustomerVehicle
        };
    }
}