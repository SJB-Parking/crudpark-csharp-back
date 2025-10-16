using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Models.DTOs.Responses;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Repositories.Interfaces;
using CrudPark_Back.Services.Interfaces;

namespace CrudPark_Back.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public CustomerService(ICustomerRepository customerRepository, IVehicleRepository vehicleRepository)
    {
        _customerRepository = customerRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(MapToResponse);
    }

    public async Task<CustomerResponse?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer == null ? null : MapToResponse(customer);
    }

    public async Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request)
    {
        // Validar email único
        if (await _customerRepository.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("El email ya está registrado");

        // Crear cliente
        var customer = new Customer
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            IdentificationNumber = request.IdentificationNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        customer = await _customerRepository.CreateAsync(customer);

        // Agregar vehículos si se proporcionaron
        if (request.VehicleIds != null && request.VehicleIds.Any())
        {
            bool isFirst = true;
            foreach (var vehicleId in request.VehicleIds)
            {
                await AddVehicleToCustomerAsync(customer.Id, vehicleId, isFirst);
                isFirst = false;
            }
        }

        // Recargar con relaciones
        var createdCustomer = await _customerRepository.GetByIdAsync(customer.Id);
        return MapToResponse(createdCustomer!);
    }

    public async Task<CustomerResponse> UpdateCustomerAsync(int id, UpdateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new KeyNotFoundException("Cliente no encontrado");

        // Validar email único si se está cambiando
        if (!string.IsNullOrEmpty(request.Email) && request.Email != customer.Email)
        {
            if (await _customerRepository.EmailExistsAsync(request.Email, id))
                throw new InvalidOperationException("El email ya está registrado");
        }

        // Actualizar campos
        if (!string.IsNullOrEmpty(request.FullName))
            customer.FullName = request.FullName;
        
        if (!string.IsNullOrEmpty(request.Email))
            customer.Email = request.Email;
        
        if (request.Phone != null)
            customer.Phone = request.Phone;
        
        if (request.IdentificationNumber != null)
            customer.IdentificationNumber = request.IdentificationNumber;

        customer = await _customerRepository.UpdateAsync(customer);

        // Recargar con relaciones
        var updatedCustomer = await _customerRepository.GetByIdAsync(customer.Id);
        return MapToResponse(updatedCustomer!);
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        return await _customerRepository.DeleteAsync(id);
    }

    public async Task<bool> AddVehicleToCustomerAsync(int customerId, int vehicleId, bool isPrimary = false)
    {
        // Validar que el cliente existe
        if (!await _customerRepository.ExistsAsync(customerId))
            throw new KeyNotFoundException("Cliente no encontrado");

        // Validar que el vehículo existe
        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
        if (vehicle == null)
            throw new KeyNotFoundException("Vehículo no encontrado");

        // Aquí normalmente agregarías la lógica para crear CustomerVehicle
        // Por ahora retornamos true
        return true;
    }

    private CustomerResponse MapToResponse(Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            Phone = customer.Phone,
            IdentificationNumber = customer.IdentificationNumber,
            Vehicles = customer.CustomerVehicles?.Select(cv => new VehicleResponse
            {
                Id = cv.Vehicle.Id,
                LicensePlate = cv.Vehicle.LicensePlate,
                VehicleType = cv.Vehicle.VehicleType.ToString(),
                Brand = cv.Vehicle.Brand,
                Model = cv.Vehicle.Model,
                Color = cv.Vehicle.Color,
                IsPrimary = cv.IsPrimary
            }).ToList() ?? new List<VehicleResponse>()
        };
    }
}